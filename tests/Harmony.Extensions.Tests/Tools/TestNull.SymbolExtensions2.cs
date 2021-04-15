using HarmonyLib.BUTR.Extensions;

using NUnit.Framework;

using System;
using System.Linq.Expressions;

namespace HarmonyLibTests.Tools
{
    [TestFixture]
    public partial class TestNull : TestLogger
    {
        [Test]
        public void Test_SymbolExtensions2_Constructors()
        {
            Assert.IsNull(SymbolExtensions2.GetConstructorInfo<TestClass, TestDelegate>(null!));
            Assert.IsNull(SymbolExtensions2.GetConstructorInfo<TestClass>(null!));
            Assert.IsNull(SymbolExtensions2.GetConstructorInfo(null!));
        }

        [Test]
        public void Test_SymbolExtensions2_Fields()
        {
            Assert.IsNull(SymbolExtensions2.GetFieldInfo<TestClass, TestDelegate>(null!));
            Assert.IsNull(SymbolExtensions2.GetFieldInfo<TestClass>(null!));
            Assert.IsNull(SymbolExtensions2.GetFieldInfo(null!));
        }

        [Test]
        public void Test_SymbolExtensions2_Properties()
        {
            Assert.IsNull(SymbolExtensions2.GetPropertyInfo<TestClass, TestDelegate>(null!));
            Assert.IsNull(SymbolExtensions2.GetPropertyInfo<TestClass>(null!));
            Assert.IsNull(SymbolExtensions2.GetPropertyInfo(null!));

            Assert.IsNull(SymbolExtensions2.GetPropertyGetter<TestClass, TestDelegate>(null!));
            Assert.IsNull(SymbolExtensions2.GetPropertyGetter<TestClass>(null!));
            Assert.IsNull(SymbolExtensions2.GetPropertyGetter(null!));

            Assert.IsNull(SymbolExtensions2.GetPropertySetter<TestClass, TestDelegate>(null!));
            Assert.IsNull(SymbolExtensions2.GetPropertySetter<TestClass>(null!));
            Assert.IsNull(SymbolExtensions2.GetPropertySetter(null!));
        }

        [Test]
        public void Test_SymbolExtensions2_Methods()
        {
            Assert.IsNull(SymbolExtensions2.GetMethodInfo<TestClass, TestDelegate>(null!));
            Assert.IsNull(SymbolExtensions2.GetMethodInfo<TestClass>(null!));
            Assert.IsNull(SymbolExtensions2.GetMethodInfo(null!));
        }

        [Test]
        public void Test_SymbolExtensions2_FieldRef()
        {
            Assert.IsNull(SymbolExtensions2.FieldRefAccess<TestClass, TestDelegate>(null!));
            Assert.IsNull(SymbolExtensions2.FieldRefAccess<TestClass>(null!));
        }

        [Test]
        public void Test_SymbolExtensions2_StaticFieldRef()
        {
            Assert.IsNull(SymbolExtensions2.StaticFieldRefAccess<object>(((Expression<Func<object>>) null)!));
            Assert.IsNull(SymbolExtensions2.StaticFieldRefAccess<object>(((LambdaExpression) null)!));
        }

        [Test]
        public void Test_SymbolExtensions2_StructFieldRef()
        {
            Assert.IsNull(SymbolExtensions2.StructFieldRefAccess<TestStruct, TestDelegate>(((Expression<Func<object>>) null)!));
            Assert.IsNull(SymbolExtensions2.StructFieldRefAccess<TestStruct, TestDelegate>(((LambdaExpression) null)!));
        }
    }
}