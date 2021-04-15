using HarmonyLib.BUTR.Extensions;

using NUnit.Framework;

using System;
using System.Reflection;

namespace HarmonyLibTests.Tools
{
    [TestFixture]
    public partial class TestNull : TestLogger
    {
        private class TestClass
        {
            private TestDelegate TestMember;
        }

        private struct TestStruct
        {
            private TestDelegate TestMember;
        }

        private delegate void TestDelegate();

        [Test]
        public void Test_AccessTools2_Constructors()
        {
            Assert.IsNull(AccessTools2.DeclaredConstructor(null!));
            Assert.IsNull(AccessTools2.Constructor(((Type) null)!));
            Assert.IsNull(AccessTools2.Constructor(((string) null)!));
        }

        [Test]
        public void Test_AccessTools2_Fields()
        {
            Assert.IsNull(AccessTools2.DeclaredField(null!, null!));
            Assert.IsNull(AccessTools2.Field(null!, null!));
            Assert.IsNull(AccessTools2.Field(null!));
        }

        [Test]
        public void Test_AccessTools2_Properties()
        {
            Assert.IsNull(AccessTools2.DeclaredProperty(null!, null!));
            Assert.IsNull(AccessTools2.Property(null!, null!));
            Assert.IsNull(AccessTools2.Property(null!));

            Assert.IsNull(AccessTools2.DeclaredPropertyGetter(null!, null!));
            Assert.IsNull(AccessTools2.DeclaredPropertySetter(null!, null!));

            Assert.IsNull(AccessTools2.PropertyGetter(null!, null!));
            Assert.IsNull(AccessTools2.PropertyGetter(null!));

            Assert.IsNull(AccessTools2.PropertySetter(null!, null!));
            Assert.IsNull(AccessTools2.PropertySetter(null!));
        }

        [Test]
        public void Test_AccessTools2_Methods()
        {
            Assert.IsNull(AccessTools2.DeclaredMethod(null!, null!));
            Assert.IsNull(AccessTools2.Method(((Type) null)!, null!));
            Assert.IsNull(AccessTools2.Method(null!));
        }

        [Test]
        public void Test_AccessTools2_Delegates()
        {
            Assert.IsNull(AccessTools2.GetDeclaredConstructorDelegate<TestDelegate>(null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegate<TestDelegate>(null!, null!));
            Assert.IsNull(AccessTools2.GetDeclaredDelegate<TestDelegate>(null!, null!, null, null));
            Assert.IsNull(AccessTools2.GetDeclaredDelegateObjectInstance<TestDelegate>(null!, null!));

            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(((ConstructorInfo) null)!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(((MethodInfo) null)!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(((Type) null)!, null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(null!, null!, null!, null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>((object) null, null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>((object) null, null!, null!, null!));
            Assert.IsNull(AccessTools2.GetDelegate<TestDelegate>(((string) null)!));
        }

        [Test]
        public void Test_AccessTools2_FieldRef()
        {
            Assert.IsNull(AccessTools2.FieldRefAccess<TestClass, TestDelegate>(((FieldInfo) null)!));
            Assert.IsNull(AccessTools2.FieldRefAccess<TestClass, TestDelegate>(((string) null)!));
            Assert.IsNull(AccessTools2.FieldRefAccess<TestDelegate>(((FieldInfo) null)!));
            Assert.IsNull(AccessTools2.FieldRefAccess<TestDelegate>(null!, null!));
        }

        [Test]
        public void Test_AccessTools2_StaticFieldRef()
        {
            Assert.IsNull(AccessTools2.StaticFieldRefAccess<TestDelegate>(((FieldInfo) null)!));
            Assert.IsNull(AccessTools2.StaticFieldRefAccess<TestDelegate>(null!, null!));
        }

        [Test]
        public void Test_AccessTools2_StructFieldRef()
        {
            Assert.IsNull(AccessTools2.StructFieldRefAccess<TestStruct, TestDelegate>(((FieldInfo) null)!));
            Assert.IsNull(AccessTools2.StructFieldRefAccess<TestStruct, TestDelegate>(((string) null)!));
        }
    }
}