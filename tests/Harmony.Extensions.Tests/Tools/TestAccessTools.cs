using HarmonyLib;
using HarmonyLib.BUTR.Extensions;
using HarmonyLibTests.Tools.Assets;

using NUnit.Framework;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLibTests.Tools
{
    [TestFixture]
    public partial class Test_AccessTools2 : TestLogger
    {
        [OneTimeSetUp]
        public void CreateAndUnloadTestDummyAssemblies()
        {
            TestTools.RunInIsolationContext(CreateTestDummyAssemblies);
        }

        // Comment out following attribute if you want to keep the dummy assembly files after the test runs.
        [OneTimeTearDown]
        public void DeleteTestDummyAssemblies()
        {
            foreach (var dummyAssemblyFileName in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "HarmonyTestsDummyAssembly*"))
            {
                try
                {
                    File.Delete(dummyAssemblyFileName);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Could not delete {dummyAssemblyFileName} during {nameof(DeleteTestDummyAssemblies)} due to {ex}");
                }
            }
        }

        static void CreateTestDummyAssemblies(ITestIsolationContext context)
        {
            var dummyAssemblyA = DefineAssembly("HarmonyTestsDummyAssemblyA",
                moduleBuilder => moduleBuilder.DefineType("HarmonyTestsDummyAssemblyA.Class1", TypeAttributes.Public));
            // Explicitly NOT saving HarmonyTestsDummyAssemblyA.
            var dummyAssemblyB = DefineAssembly("HarmonyTestsDummyAssemblyB",
                moduleBuilder => moduleBuilder.DefineType("HarmonyTestsDummyAssemblyB.Class1", TypeAttributes.Public,
                    parent: dummyAssemblyA.GetType("HarmonyTestsDummyAssemblyA.Class1")),
                moduleBuilder => moduleBuilder.DefineType("HarmonyTestsDummyAssemblyB.Class2", TypeAttributes.Public));
            // HarmonyTestsDummyAssemblyB, if loaded, becomes an invalid assembly due to missing HarmonyTestsDummyAssemblyA.
            SaveAssembly(dummyAssemblyB);
            // HarmonyTestsDummyAssemblyC is just another (valid) assembly to be loaded after HarmonyTestsDummyAssemblyB.
            var dummyAssemblyC = DefineAssembly("HarmonyTestsDummyAssemblyC",
                moduleBuilder => moduleBuilder.DefineType("HarmonyTestsDummyAssemblyC.Class1", TypeAttributes.Public));
            SaveAssembly(dummyAssemblyC);
        }

        static AssemblyBuilder DefineAssembly(string assemblyName, params Func<ModuleBuilder, TypeBuilder>[] defineTypeFuncs)
        {
#if NETCOREAPP
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.RunAndCollect);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("module");
#else
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Save,
                AppDomain.CurrentDomain.BaseDirectory);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("module", assemblyName + ".dll");
#endif
            foreach (var defineTypeFunc in defineTypeFuncs)
                _ = defineTypeFunc(moduleBuilder)?.CreateType();
            return assemblyBuilder;
        }

        static void SaveAssembly(AssemblyBuilder assemblyBuilder)
        {
            var assemblyFileName = assemblyBuilder.GetName().Name + ".dll";
#if NETCOREAPP
            // For some reason, ILPack requires referenced dynamic assemblies to be passed in rather than looking them up itself.
            var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var referencedDynamicAssemblies = assemblyBuilder.GetReferencedAssemblies()
                .Select(referencedAssemblyName => currentAssemblies.FirstOrDefault(assembly => assembly.FullName == referencedAssemblyName.FullName))
                .Where(referencedAssembly => referencedAssembly is object && referencedAssembly.IsDynamic)
                .ToArray();
            // ILPack currently has an issue where the dynamic assembly has an assembly reference to the runtime assembly (System.Private.CoreLib)
            // rather than reference assembly (System.Runtime). This causes issues for decompilers, but is fine for loading via Assembly.Load et all,
            // since the .NET Core runtime assemblies are definitely already accessible and loaded.
            new Lokad.ILPack.AssemblyGenerator().GenerateAssembly(assemblyBuilder, referencedDynamicAssemblies,
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyFileName));
#else
            assemblyBuilder.Save(assemblyFileName);
#endif
        }

        [Test, NonParallelizable]
        public void Test_AccessTools2_TypeByName_CurrentAssemblies()
        {
            Assert.NotNull(AccessTools2.TypeByName(typeof(Harmony).FullName!));
            Assert.NotNull(AccessTools2.TypeByName(typeof(Test_AccessTools2).FullName!));
            Assert.Null(AccessTools2.TypeByName("HarmonyTestsDummyAssemblyA.Class1"));
            Assert.Null(AccessTools2.TypeByName("HarmonyTestsDummyAssemblyB.Class1"));
            Assert.Null(AccessTools2.TypeByName("HarmonyTestsDummyAssemblyB.Class2"));
            Assert.Null(AccessTools2.TypeByName("HarmonyTestsDummyAssemblyC.Class1"));
            Assert.Null(AccessTools2.TypeByName("IAmALittleTeaPot.ShortAndStout"));
        }

        [Test, NonParallelizable]
        public void Test_AccessTools2_TypeByName_InvalidAssembly()
        {
            TestTools.RunInIsolationContext(TestTypeByNameWithInvalidAssembly);
            // Sanity check that TypeByName works as if the test dummy assemblies never existed.
            Test_AccessTools2_TypeByName_CurrentAssemblies();
        }

        [Test, NonParallelizable]
        public void Test_AccessTools2_TypeByName_NoInvalidAssembly()
        {
            TestTools.RunInIsolationContext(TestTypeByNameWithNoInvalidAssembly);
            // Sanity check that TypeByName works as if the test dummy assemblies never existed.
            Test_AccessTools2_TypeByName_CurrentAssemblies();
        }

        static void TestTypeByNameWithInvalidAssembly(ITestIsolationContext context)
        {
            // HarmonyTestsDummyAssemblyB has a dependency on HarmonyTestsDummyAssemblyA, but we've ensured that
            // HarmonyTestsDummyAssemblyA.dll is NOT available (i.e. not in HarmonyTests output dir).
            context.AssemblyLoad("HarmonyTestsDummyAssemblyB");
            context.AssemblyLoad("HarmonyTestsDummyAssemblyC");
            // Even if 0Harmony.dll isn't loaded yet and thus would be automatically loaded after the invalid assemblies,
            // TypeByName tries Type.GetType first, which always works for a type in the executing assembly (0Harmony.dll).
            Assert.NotNull(AccessTools2.TypeByName(typeof(Harmony).FullName!));
            // The current executing assembly (HarmonyTests.dll) was definitely already loaded before above loads.
            Assert.NotNull(AccessTools2.TypeByName(typeof(Test_AccessTools2).FullName!));
            // HarmonyTestsDummyAssemblyA is explicitly missing, so it's the same as the unknown type case - see below.
            Assert.Null(AccessTools2.TypeByName("HarmonyTestsDummyAssemblyA.Class1"));
            // HarmonyTestsDummyAssemblyB.GetTypes() should throw ReflectionTypeLoadException due to missing HarmonyTestsDummyAssemblyA,
            // but this is caught and returns successfully loaded types.
            // HarmonyTestsDummyAssemblyB.Class1 depends on HarmonyTestsDummyAssemblyA, so it's not loaded successfully.
            Assert.Null(AccessTools2.TypeByName("HarmonyTestsDummyAssemblyB.Class1"));
            // HarmonyTestsDummyAssemblyB.Class2 doesn't depend on HarmonyTestsDummyAssemblyA, so it's loaded successfully.
            Assert.NotNull(AccessTools2.TypeByName("HarmonyTestsDummyAssemblyB.Class2"));
            // TypeByName's search should find HarmonyTestsDummyAssemblyB before HarmonyTestsDummyAssemblyC, but this is fine.
            Assert.NotNull(AccessTools2.TypeByName("HarmonyTestsDummyAssemblyC.Class1"));
            // TypeByName's search for an unknown type should always find HarmonyTestsDummyAssemblyB first, which is again fine.
            Assert.Null(AccessTools2.TypeByName("IAmALittleTeaPot.ShortAndStout"));
        }

        static void TestTypeByNameWithNoInvalidAssembly(ITestIsolationContext context)
        {
            context.AssemblyLoad("HarmonyTestsDummyAssemblyC");
            Assert.NotNull(AccessTools2.TypeByName(typeof(Harmony).FullName!));
            Assert.NotNull(AccessTools2.TypeByName(typeof(Test_AccessTools2).FullName!));
            Assert.Null(AccessTools2.TypeByName("HarmonyTestsDummyAssemblyA.Class1"));
            Assert.Null(AccessTools2.TypeByName("HarmonyTestsDummyAssemblyB.Class1"));
            Assert.Null(AccessTools2.TypeByName("HarmonyTestsDummyAssemblyB.Class2"));
            Assert.NotNull(AccessTools2.TypeByName("HarmonyTestsDummyAssemblyC.Class1"));
            Assert.Null(AccessTools2.TypeByName("IAmALittleTeaPot.ShortAndStout"));
        }

        [Test]
        public void Test_AccessTools2_Field1()
        {
            var type = typeof(AccessToolsClass);

            Assert.Null(AccessTools2.DeclaredField(null!, null!));
            Assert.Null(AccessTools2.DeclaredField(type, null!));
            Assert.Null(AccessTools2.DeclaredField(null!, "field1"));
            Assert.Null(AccessTools2.DeclaredField(type, "unknown"));

            var field = AccessTools2.DeclaredField(type, "field1");
            Assert.NotNull(field);
            Assert.AreEqual(type, field.DeclaringType);
            Assert.AreEqual("field1", field.Name);
        }

        [Test]
        public void Test_AccessTools2_Field2()
        {
            var classType = typeof(AccessToolsClass);
            Assert.NotNull(AccessTools2.Field(classType, "field1"));
            Assert.NotNull(AccessTools2.DeclaredField(classType, "field1"));
            Assert.Null(AccessTools2.Field(classType, "unknown"));
            Assert.Null(AccessTools2.DeclaredField(classType, "unknown"));

            var subclassType = typeof(AccessToolsSubClass);
            Assert.NotNull(AccessTools2.Field(subclassType, "field1"));
            Assert.Null(AccessTools2.DeclaredField(subclassType, "field1"));
            Assert.Null(AccessTools2.Field(subclassType, "unknown"));
            Assert.Null(AccessTools2.DeclaredField(subclassType, "unknown"));

            var structType = typeof(AccessToolsStruct);
            Assert.NotNull(AccessTools2.Field(structType, "structField1"));
            Assert.NotNull(AccessTools2.DeclaredField(structType, "structField1"));
            Assert.Null(AccessTools2.Field(structType, "unknown"));
            Assert.Null(AccessTools2.DeclaredField(structType, "unknown"));

            var interfaceType = typeof(IAccessToolsType);
            Assert.Null(AccessTools2.Field(interfaceType, "unknown"));
            Assert.Null(AccessTools2.DeclaredField(interfaceType, "unknown"));
        }

        [Test]
        public void Test_AccessTools2_Property1()
        {
            var type = typeof(AccessToolsClass);

            Assert.Null(AccessTools2.Property(null!, null!));
            Assert.Null(AccessTools2.Property(type, null!));
            Assert.Null(AccessTools2.Property(null!, "Property1"));
            Assert.Null(AccessTools2.Property(type, "unknown"));

            var prop = AccessTools2.Property(type, "Property1");
            Assert.NotNull(prop);
            Assert.AreEqual(type, prop.DeclaringType);
            Assert.AreEqual("Property1", prop.Name);
        }

        [Test]
        public void Test_AccessTools2_Property2()
        {
            var classType = typeof(AccessToolsClass);
            Assert.NotNull(AccessTools2.Property(classType, "Property1"));
            Assert.NotNull(AccessTools2.DeclaredProperty(classType, "Property1"));
            Assert.Null(AccessTools2.Property(classType, "unknown"));
            Assert.Null(AccessTools2.DeclaredProperty(classType, "unknown"));

            var subclassType = typeof(AccessToolsSubClass);
            Assert.NotNull(AccessTools2.Property(subclassType, "Property1"));
            Assert.Null(AccessTools2.DeclaredProperty(subclassType, "Property1"));
            Assert.Null(AccessTools2.Property(subclassType, "unknown"));
            Assert.Null(AccessTools2.DeclaredProperty(subclassType, "unknown"));

            var structType = typeof(AccessToolsStruct);
            Assert.NotNull(AccessTools2.Property(structType, "Property1"));
            Assert.NotNull(AccessTools2.DeclaredProperty(structType, "Property1"));
            Assert.Null(AccessTools2.Property(structType, "unknown"));
            Assert.Null(AccessTools2.DeclaredProperty(structType, "unknown"));

            var interfaceType = typeof(IAccessToolsType);
            Assert.NotNull(AccessTools2.Property(interfaceType, "Property1"));
            Assert.NotNull(AccessTools2.DeclaredProperty(interfaceType, "Property1"));
            Assert.Null(AccessTools2.Property(interfaceType, "unknown"));
            Assert.Null(AccessTools2.DeclaredProperty(interfaceType, "unknown"));
        }

        [Test]
        public void Test_AccessTools2_PropertyIndexer()
        {
            var classType = typeof(AccessToolsClass);
            Assert.NotNull(AccessTools2.Property(classType, "Item"));
            Assert.NotNull(AccessTools2.DeclaredProperty(classType, "Item"));

            var subclassType = typeof(AccessToolsSubClass);
            Assert.NotNull(AccessTools2.Property(subclassType, "Item"));
            Assert.Null(AccessTools2.DeclaredProperty(subclassType, "Item"));

            var structType = typeof(AccessToolsStruct);
            Assert.NotNull(AccessTools2.Property(structType, "Item"));
            Assert.NotNull(AccessTools2.DeclaredProperty(structType, "Item"));

            var interfaceType = typeof(IAccessToolsType);
            Assert.NotNull(AccessTools2.Property(interfaceType, "Item"));
            Assert.NotNull(AccessTools2.DeclaredProperty(interfaceType, "Item"));
        }

        [Test]
        public void Test_AccessTools2_Method1()
        {
            var type = typeof(AccessToolsClass);

            Assert.Null(AccessTools2.Method(null!));
            Assert.Null(AccessTools2.Method(type, null!));
            Assert.Null(AccessTools2.Method(null!, "Method1"));
            Assert.Null(AccessTools2.Method(type, "unknown"));

            var m1 = AccessTools2.Method(type, "Method1");
            Assert.NotNull(m1);
            Assert.AreEqual(type, m1.DeclaringType);
            Assert.AreEqual("Method1", m1.Name);

            var m2 = AccessTools2.Method("HarmonyLibTests.Assets.AccessToolsClass:Method1");
            Assert.NotNull(m2);
            Assert.AreEqual(type, m2.DeclaringType);
            Assert.AreEqual("Method1", m2.Name);

            var m3 = AccessTools2.Method(type, "Method1", new Type[] { });
            Assert.NotNull(m3);

            var m4 = AccessTools2.Method(type, "SetField", new Type[] { typeof(string) });
            Assert.NotNull(m4);
        }

        [Test]
        public void Test_AccessTools2_Method2()
        {
            var classType = typeof(AccessToolsClass);
            Assert.NotNull(AccessTools2.Method(classType, "Method1"));
            Assert.NotNull(AccessTools2.DeclaredMethod(classType, "Method1"));
            Assert.Null(AccessTools2.Method(classType, "unknown"));
            Assert.Null(AccessTools2.DeclaredMethod(classType, "unknown"));

            var subclassType = typeof(AccessToolsSubClass);
            Assert.NotNull(AccessTools2.Method(subclassType, "Method1"));
            Assert.Null(AccessTools2.DeclaredMethod(subclassType, "Method1"));
            Assert.Null(AccessTools2.Method(subclassType, "unknown"));
            Assert.Null(AccessTools2.DeclaredMethod(subclassType, "unknown"));

            var structType = typeof(AccessToolsStruct);
            Assert.NotNull(AccessTools2.Method(structType, "Method1"));
            Assert.NotNull(AccessTools2.DeclaredMethod(structType, "Method1"));
            Assert.Null(AccessTools2.Method(structType, "unknown"));
            Assert.Null(AccessTools2.DeclaredMethod(structType, "unknown"));

            var interfaceType = typeof(IAccessToolsType);
            Assert.NotNull(AccessTools2.Method(interfaceType, "Method1"));
            Assert.NotNull(AccessTools2.DeclaredMethod(interfaceType, "Method1"));
            Assert.Null(AccessTools2.Method(interfaceType, "unknown"));
            Assert.Null(AccessTools2.DeclaredMethod(interfaceType, "unknown"));
        }
    }
}