using HarmonyLib.BUTR.Extensions;

using NUnit.Framework;

using System;
using System.Linq.Expressions;

namespace HarmonyLibTests.Tools
{
    public partial class TestNull
    {
        [Test]
        public void Test_SymbolExtensions2_Constructors()
        {
            Assert.IsNull(SymbolExtensions2.GetConstructorInfo<TestClass, TestDelegate>(((Expression<Func<TestClass, TestDelegate>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetConstructorInfo<TestClass>(((Expression<Func<TestClass>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetConstructorInfo(((LambdaExpression) null)!));
        }

        [Test]
        public void Test_SymbolExtensions2_Fields()
        {
            Assert.IsNull(SymbolExtensions2.GetFieldInfo<TestClass, TestDelegate>(((Expression<Func<TestClass, TestDelegate>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetFieldInfo<TestClass>(((Expression<Func<TestClass>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetFieldInfo(((LambdaExpression) null)!));
        }

        [Test]
        public void Test_SymbolExtensions2_Properties()
        {
            Assert.IsNull(SymbolExtensions2.GetPropertyInfo<TestClass, TestDelegate>(((Expression<Func<TestClass, TestDelegate>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetPropertyInfo<TestClass>(((Expression<Func<TestClass>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetPropertyInfo(((LambdaExpression) null)!));

            Assert.IsNull(SymbolExtensions2.GetPropertyGetter<TestClass, TestDelegate>(((Expression<Func<TestClass, TestDelegate>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetPropertyGetter<TestClass>(((Expression<Func<TestClass>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetPropertyGetter(((LambdaExpression) null)!));

            Assert.IsNull(SymbolExtensions2.GetPropertySetter<TestClass, TestDelegate>(((Expression<Func<TestClass, TestDelegate>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetPropertySetter<TestClass>(((Expression<Func<TestClass>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetPropertySetter(((LambdaExpression) null)!));
        }

        [Test]
        public void Test_SymbolExtensions2_Methods()
        {
            Assert.IsNull(SymbolExtensions2.GetMethodInfo<TestClass, TestDelegate>(((Expression<Func<TestClass, TestDelegate>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetMethodInfo<TestClass>(((Expression<Action<TestClass>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetMethodInfo(((Expression<Action>) null)!));
            Assert.IsNull(SymbolExtensions2.GetMethodInfo(((LambdaExpression) null)!));
        }

        [Test]
        public void Test_SymbolExtensions2_FieldRef()
        {
            Assert.IsNull(SymbolExtensions2.GetFieldRefAccess<TestClass, TestDelegate>(((Expression<Func<TestClass, TestDelegate>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetFieldRefAccess<TestClass, TestDelegate>(((LambdaExpression) null)!));
            Assert.IsNull(SymbolExtensions2.GetFieldRefAccess<TestClass>(((Expression<Func<TestClass>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetFieldRefAccess<TestClass>(((LambdaExpression) null)!));
            Assert.IsNull(SymbolExtensions2.GetFieldRefAccess<TestDelegate>(((Expression<Func<TestDelegate>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetFieldRefAccess<TestDelegate>(((LambdaExpression) null)!));
        }

        [Test]
        public void Test_SymbolExtensions2_StaticFieldRef()
        {
            Assert.IsNull(SymbolExtensions2.GetStaticFieldRefAccess<object>(((Expression<Func<object>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetStaticFieldRefAccess<object>(((LambdaExpression) null)!));
        }

        [Test]
        public void Test_SymbolExtensions2_StructFieldRef()
        {
            Assert.IsNull(SymbolExtensions2.GetStructFieldRefAccess<TestStruct, TestDelegate>(((Expression<Func<TestDelegate>>) null)!));
            Assert.IsNull(SymbolExtensions2.GetStructFieldRefAccess<TestStruct, TestDelegate>(((LambdaExpression) null)!));
        }
    }
}