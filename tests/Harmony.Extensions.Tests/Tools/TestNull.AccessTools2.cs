using HarmonyLib.BUTR.Extensions;

using NUnit.Framework;

using System;
using System.Reflection;

namespace HarmonyLibTests.Tools
{
    public partial class TestNull
    {
        private class TestClass
        {
            private readonly TestDelegate TestMember;
        }

        private struct TestStruct
        {
            private readonly TestDelegate TestMember;
        }

        private delegate void TestDelegate();
        private delegate void TestInstanceDelegate(TestClass instance);
        private delegate void TestObjectInstanceDelegate(object instance);

        [Test]
        public void Test_AccessTools2_Constructors()
        {
            Assert.IsNull(AccessTools2.DeclaredConstructor(null!, null, false));
            Assert.IsNull(AccessTools2.DeclaredConstructor(null!, null, true));
            Assert.IsNull(AccessTools2.Constructor(((Type) null)!, null, false));
            Assert.IsNull(AccessTools2.Constructor(((Type) null)!, null, true));
            Assert.IsNull(AccessTools2.Constructor(((string) null)!, null, false));
            Assert.IsNull(AccessTools2.Constructor(((string) null)!, null, true));
            Assert.IsNull(AccessTools2.Constructor("NonExistingType", null, false));
            Assert.IsNull(AccessTools2.Constructor("NonExistingType", null, true));
        }

        [Test]
        public void Test_AccessTools2_Fields()
        {
            Assert.IsNull(AccessTools2.DeclaredField(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.DeclaredField(typeof(TestClass), "NonExistingField"));
            Assert.IsNull(AccessTools2.DeclaredField(null!, null!));
            Assert.IsNull(AccessTools2.DeclaredField(null!, "NonExistingField"));
            Assert.IsNull(AccessTools2.Field(null!, null!));
            Assert.IsNull(AccessTools2.Field(null!, "NonExistingField"));
            Assert.IsNull(AccessTools2.Field(null!));
        }

        [Test]
        public void Test_AccessTools2_Properties()
        {
            Assert.IsNull(AccessTools2.DeclaredProperty(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.DeclaredProperty(typeof(TestClass), "NonExistingProperty"));
            Assert.IsNull(AccessTools2.DeclaredProperty(null!, null!));
            Assert.IsNull(AccessTools2.DeclaredProperty(null!, "NonExistingProperty"));
            Assert.IsNull(AccessTools2.Property(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.Property(typeof(TestClass), "NonExistingProperty"));
            Assert.IsNull(AccessTools2.Property(null!, null!));
            Assert.IsNull(AccessTools2.Property(null!, "NonExistingProperty"));
            Assert.IsNull(AccessTools2.Property(null!));

            Assert.IsNull(AccessTools2.DeclaredPropertyGetter(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.DeclaredPropertyGetter(typeof(TestClass), "NonExistingProperty"));
            Assert.IsNull(AccessTools2.DeclaredPropertyGetter(null!, null!));
            Assert.IsNull(AccessTools2.DeclaredPropertyGetter(null!, "NonExistingProperty"));

            Assert.IsNull(AccessTools2.DeclaredPropertySetter(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.DeclaredPropertySetter(typeof(TestClass), "NonExistingProperty"));
            Assert.IsNull(AccessTools2.DeclaredPropertySetter(null!, null!));
            Assert.IsNull(AccessTools2.DeclaredPropertySetter(null!, "NonExistingProperty"));

            Assert.IsNull(AccessTools2.PropertyGetter(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.PropertyGetter(typeof(TestClass), "NonExistingProperty"));
            Assert.IsNull(AccessTools2.PropertyGetter(null!, null!));
            Assert.IsNull(AccessTools2.PropertyGetter(null!, "NonExistingProperty"));
            Assert.IsNull(AccessTools2.PropertyGetter(null!));

            Assert.IsNull(AccessTools2.PropertySetter(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.PropertySetter(typeof(TestClass), "NonExistingProperty"));
            Assert.IsNull(AccessTools2.PropertySetter(null!, null!));
            Assert.IsNull(AccessTools2.PropertySetter(null!, "NonExistingProperty"));
            Assert.IsNull(AccessTools2.PropertySetter(null!));
        }

        [Test]
        public void Test_AccessTools2_Methods()
        {
            Assert.IsNull(AccessTools2.DeclaredMethod(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.DeclaredMethod(typeof(TestClass), "NonExistingMethod"));
            Assert.IsNull(AccessTools2.DeclaredMethod(null!, null!));
            Assert.IsNull(AccessTools2.DeclaredMethod(null!, "NonExistingMethod"));
            Assert.IsNull(AccessTools2.Method(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.Method(typeof(TestClass), "NonExistingMethod"));
            Assert.IsNull(AccessTools2.Method(((Type) null)!, null!));
            Assert.IsNull(AccessTools2.Method(((Type) null)!, "NonExistingMethod"));
            Assert.IsNull(AccessTools2.Method(null!));
        }

        [Test]
        public void Test_AccessTools2_Delegates()
        {
            Assert.IsNull(AccessTools2.GetDeclaredConstructorDelegate<TestDelegate>(null!));

            Assert.IsNull(AccessTools2.GetConstructorDelegate<TestDelegate>(((Type) null)!));
            Assert.IsNull(AccessTools2.GetConstructorDelegate<TestDelegate>(((string) null)!));

            Assert.IsNull(AccessTools2.GetDeclaredPropertyGetterDelegate<TestDelegate>(null!, null!));
            Assert.IsNull(AccessTools2.GetDeclaredPropertySetterDelegate<TestDelegate>(null!, null!));

            Assert.IsNull(AccessTools2.GetPropertyGetterDelegate<TestDelegate>(((PropertyInfo) null)!));
            Assert.IsNull(AccessTools2.GetPropertySetterDelegate<TestDelegate>(((PropertyInfo) null)!));
            Assert.IsNull(AccessTools2.GetPropertyGetterDelegate<TestDelegate>(((string) null)!));
            Assert.IsNull(AccessTools2.GetPropertySetterDelegate<TestDelegate>(((string) null)!));
            Assert.IsNull(AccessTools2.GetPropertyGetterDelegate<TestDelegate>(null!, null!));
            Assert.IsNull(AccessTools2.GetPropertySetterDelegate<TestDelegate>(null!, null!));

            Assert.IsNull(AccessTools2.GetDeclaredDelegate<TestDelegate>(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegate<TestDelegate>(typeof(TestClass), "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDeclaredDelegate<TestDelegate>(((Type) null)!, null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegate<TestDelegate>(((Type) null)!, "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDeclaredDelegate<TestDelegate>(typeof(TestClass), null!, null!, null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegate<TestDelegate>(typeof(TestClass), "NonExistingDelegateMethod", null!, null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegate<TestDelegate>(null!, null!, null!, null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegate<TestDelegate>(null!, "NonExistingDelegateMethod", null!, null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegate<TestDelegate, object>(new object(), ((string) null)!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegate<TestDelegate, object>(new object(), ((string) null)!, null!));

            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestDelegate>(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestDelegate>(typeof(TestClass), null!, null, null));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestDelegate>(typeof(TestClass), "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestDelegate>(typeof(TestClass), "NonExistingDelegateMethod", null, null));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestDelegate>(null!, null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestDelegate>(null!, null!, null, null));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestDelegate>(null!, "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestDelegate>(null!, "NonExistingDelegateMethod", null, null));

            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestInstanceDelegate>(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestInstanceDelegate>(typeof(TestClass), null!, null, null));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestInstanceDelegate>(typeof(TestClass), "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestInstanceDelegate>(typeof(TestClass), "NonExistingDelegateMethod", null, null));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestInstanceDelegate>(null!, null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestInstanceDelegate>(null!, null!, null, null));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestInstanceDelegate>(null!, "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestInstanceDelegate>(null!, "NonExistingDelegateMethod", null, null));

            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestObjectInstanceDelegate>(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestObjectInstanceDelegate>(typeof(TestClass), null!, null, null));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestObjectInstanceDelegate>(typeof(TestClass), "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestObjectInstanceDelegate>(typeof(TestClass), "NonExistingDelegateMethod", null, null));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestObjectInstanceDelegate>(null!, null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestObjectInstanceDelegate>(null!, null!, null, null));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestObjectInstanceDelegate>(null!, "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestObjectInstanceDelegate>(null!, "NonExistingDelegateMethod", null, null));

            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestDelegate>(((MethodInfo) null)!));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestDelegate>(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestDelegate>(typeof(TestClass), null!, null, null));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestDelegate>(typeof(TestClass), "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestDelegate>(typeof(TestClass), "NonExistingDelegateMethod", null, null));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestDelegate>(null!, null!));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestDelegate>(null!, null!, null, null));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestDelegate>(null!, "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestDelegate>(null!, "NonExistingDelegateMethod", null, null));

            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestInstanceDelegate>(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestInstanceDelegate>(typeof(TestClass), null!, null, null));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestInstanceDelegate>(typeof(TestClass), "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestInstanceDelegate>(typeof(TestClass), "NonExistingDelegateMethod", null, null));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestInstanceDelegate>(null!, null!));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestInstanceDelegate>(null!, null!, null, null));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestInstanceDelegate>(null!, "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestInstanceDelegate>(null!, "NonExistingDelegateMethod", null, null));

            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestObjectInstanceDelegate>(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestObjectInstanceDelegate>(typeof(TestClass), null!, null, null));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestObjectInstanceDelegate>(typeof(TestClass), "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestObjectInstanceDelegate>(typeof(TestClass), "NonExistingDelegateMethod", null, null));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestObjectInstanceDelegate>(null!, null!));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestObjectInstanceDelegate>(null!, null!, null, null));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestObjectInstanceDelegate>(null!, "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDelegateObjectInstance<TestObjectInstanceDelegate>(null!, "NonExistingDelegateMethod", null, null));

            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(((ConstructorInfo) null)!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(((MethodInfo) null)!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(typeof(TestClass), "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(((Type) null)!, null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(((Type) null)!, "NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(typeof(TestClass), null!, null!, null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(typeof(TestClass), "NonExistingDelegateMethod", null!, null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(null!, null!, null!, null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(null!, "NonExistingDelegateMethod", null!, null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(new object(), null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate, object>(new object(), ((string) null)!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate, object>(new object(), ((string) null)!, null, null));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate, object>(new object(), ((MethodInfo) null)!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>((object) null, null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(new object(), null!, null!, null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(new object(), "NonExistingDelegateMethod", null, null));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>((object) null, null!, null!, null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>((object) null, "NonExistingDelegateMethod", null, null));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(((string) null)!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>("NonExistingDelegateMethod"));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(new object(), null!, null!, null, null));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(null, typeof(TestClass), null!, null, null));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(null, null!, "NonExistingDelegateMethod", null, null));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(null!, null!, null!, null, null));
        }

        [Test]
        public void Test_AccessTools2_FieldRef()
        {
            Assert.IsNull(AccessTools2.FieldRefAccess<TestClass, TestDelegate>(((FieldInfo) null)!));
            Assert.IsNull(AccessTools2.FieldRefAccess<TestClass, TestDelegate>(((string) null)!));
            Assert.IsNull(AccessTools2.FieldRefAccess<TestDelegate>(((FieldInfo) null)!));
            Assert.IsNull(AccessTools2.FieldRefAccess<TestDelegate>(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.FieldRefAccess<TestDelegate>(typeof(TestStruct), null!));
            Assert.IsNull(AccessTools2.FieldRefAccess<TestDelegate>(null!, null!));
        }

        [Test]
        public void Test_AccessTools2_StaticFieldRef()
        {
            Assert.IsNull(AccessTools2.StaticFieldRefAccess<TestDelegate>(((FieldInfo) null)!));
            Assert.IsNull(AccessTools2.StaticFieldRefAccess<TestDelegate>(typeof(TestClass), null!));
            Assert.IsNull(AccessTools2.StaticFieldRefAccess<TestDelegate>(null!, null!));
        }

        [Test]
        public void Test_AccessTools2_StructFieldRef()
        {
            Assert.IsNull(AccessTools2.StructFieldRefAccess<TestStruct, TestDelegate>(((FieldInfo) null)!));
            Assert.IsNull(AccessTools2.StructFieldRefAccess<TestStruct, TestDelegate>(((string) null)!));
        }

        [Test]
        public void Test_AccessTools2_Utilities()
        {
            Assert.IsNull(AccessTools2.TypeByName(null!));
            Assert.IsNull(AccessTools2.FindIncludingBaseTypes<TestClass>(null!, null!));
            Assert.IsNull(AccessTools2.FindIncludingBaseTypes<TestClass>(typeof(TestClass), null!));
            Assert.IsEmpty(AccessTools2.GetTypesFromAssembly(null!));
        }
    }
}