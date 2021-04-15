using HarmonyLib.BUTR.Extensions;
using HarmonyLibTests.Assets;

using NUnit.Framework;

using System;
using System.Reflection;

namespace HarmonyLibTests.Tools
{
    [TestFixture]
    public partial class Test_AccessTools2 : TestLogger
    {
        delegate string MethodDel(int n, ref float f);
        delegate string OpenMethodDel<T>(T instance, int n, ref float f);

        static readonly MethodInfo interfaceTest = typeof(AccessToolsMethodDelegate.IInterface).GetMethod("Test");
        static readonly MethodInfo baseTest = typeof(AccessToolsMethodDelegate.Base).GetMethod("Test");
        static readonly MethodInfo derivedTest = typeof(AccessToolsMethodDelegate.Derived).GetMethod("Test");
        static readonly MethodInfo structTest = typeof(AccessToolsMethodDelegate.Struct).GetMethod("Test");
        static readonly MethodInfo staticTest = typeof(AccessToolsMethodDelegate).GetMethod("Test");

        [Test]
        public void Test_AccessTools2_GetDelegate_ClosedInstanceDelegates()
        {
            var f = 789f;
            var baseInstance = new AccessToolsMethodDelegate.Base();
            var derivedInstance = new AccessToolsMethodDelegate.Derived();
            var structInstance = new AccessToolsMethodDelegate.Struct();
            Assert.AreEqual("base test 456 790 1", AccessTools2.GetDelegate<MethodDel>(baseInstance, baseTest)!(456, ref f));
            Assert.IsNull(AccessTools2.GetDelegate<MethodDel>(baseInstance, derivedTest)?.Invoke(456, ref f));
            Assert.AreEqual("derived test 456 791 1", AccessTools2.GetDelegate<MethodDel>(derivedInstance, baseTest)!(456, ref f));
            Assert.AreEqual("derived test 456 792 2", AccessTools2.GetDelegate<MethodDel>(derivedInstance, derivedTest)!(456, ref f));
            Assert.AreEqual("struct result 456 793 1", AccessTools2.GetDelegate<MethodDel>(structInstance, structTest)!(456, ref f));
        }

        [Test]
        public void Test_AccessTools2_GetDelegate_ClosedInstanceDelegates_InterfaceMethod()
        {
            var f = 789f;
            var baseInstance = new AccessToolsMethodDelegate.Base();
            var derivedInstance = new AccessToolsMethodDelegate.Derived();
            var structInstance = new AccessToolsMethodDelegate.Struct();
            Assert.AreEqual("base test 456 790 1", AccessTools2.GetDelegate<MethodDel>(baseInstance, interfaceTest)!(456, ref f));
            Assert.AreEqual("derived test 456 791 1", AccessTools2.GetDelegate<MethodDel>(derivedInstance, interfaceTest)!(456, ref f));
            Assert.AreEqual("struct result 456 792 1", AccessTools2.GetDelegate<MethodDel>(structInstance, interfaceTest)!(456, ref f));
        }

        [Test]
        public void Test_AccessTools2_GetDelegate_OpenInstanceDelegates()
        {
            var f = 789f;
            var baseInstance = new AccessToolsMethodDelegate.Base();
            var derivedInstance = new AccessToolsMethodDelegate.Derived();
            var structInstance = new AccessToolsMethodDelegate.Struct();
            Assert.AreEqual("base test 456 790 1", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Base>>(baseTest)!(baseInstance, 456, ref f));
            Assert.AreEqual("derived test 456 791 1", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Base>>(baseTest)!(derivedInstance, 456, ref f));
            _ = Assert.Throws(typeof(InvalidCastException), () => AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Base>>(derivedTest)!(baseInstance, 456, ref f));
            Assert.AreEqual("derived test 456 792 2", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Base>>(derivedTest)!(derivedInstance, 456, ref f));
            //AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Derived>>(derivedTest)(baseInstance, 456, ref f); // expected compile error
            Assert.AreEqual("derived test 456 793 3", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Derived>>(derivedTest)!(derivedInstance, 456, ref f));
            Assert.AreEqual("struct result 456 794 1", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Struct>>(structTest)!(structInstance, 456, ref f));
        }

        [Test]
        public void Test_AccessTools2_GetDelegate_OpenInstanceDelegates_DelegateInterfaceInstanceType()
        {
            var f = 789f;
            var baseInstance = new AccessToolsMethodDelegate.Base();
            var derivedInstance = new AccessToolsMethodDelegate.Derived();
            var structInstance = new AccessToolsMethodDelegate.Struct();
            Assert.AreEqual("base test 456 790 1", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.IInterface>>(baseTest)!(baseInstance, 456, ref f));
            Assert.AreEqual("derived test 456 791 1", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.IInterface>>(baseTest)!(derivedInstance, 456, ref f));
            _ = Assert.Throws(typeof(InvalidCastException), () => AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.IInterface>>(derivedTest)!(baseInstance, 456, ref f));
            Assert.AreEqual("derived test 456 792 2", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.IInterface>>(derivedTest)!(derivedInstance, 456, ref f));
            Assert.AreEqual("struct result 456 793 1", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.IInterface>>(structTest)!(structInstance, 456, ref f));
        }

        [Test]
        public void Test_AccessTools2_GetDelegate_OpenInstanceDelegates_InterfaceMethod()
        {
            var f = 789f;
            var baseInstance = new AccessToolsMethodDelegate.Base();
            var derivedInstance = new AccessToolsMethodDelegate.Derived();
            var structInstance = new AccessToolsMethodDelegate.Struct();
            Assert.AreEqual("base test 456 790 1", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.IInterface>>(interfaceTest)!(baseInstance, 456, ref f));
            Assert.AreEqual("derived test 456 791 1", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.IInterface>>(interfaceTest)!(derivedInstance, 456, ref f));
            Assert.AreEqual("struct result 456 792 1", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.IInterface>>(interfaceTest)!(structInstance, 456, ref f));
            Assert.AreEqual("base test 456 793 2", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Base>>(interfaceTest)!(baseInstance, 456, ref f));
            Assert.AreEqual("derived test 456 794 2", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Base>>(interfaceTest)!(derivedInstance, 456, ref f));
            //AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Derived>>(interfaceTest)!(baseInstance, 456, ref f)); // expected compile error
            Assert.AreEqual("derived test 456 795 3", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Derived>>(interfaceTest)!(derivedInstance, 456, ref f));
            Assert.AreEqual("struct result 456 796 1", AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Struct>>(interfaceTest)!(structInstance, 456, ref f));
        }

        [Test]
        public void Test_AccessTools2_GetDelegate_StaticDelegates_InterfaceMethod()
        {
            var f = 789f;
            Assert.AreEqual("static test 456 790 1", AccessTools2.GetDelegate<MethodDel>(staticTest)!(456, ref f));
            Assert.IsNull(AccessTools2.GetDelegate<MethodDel>(new AccessToolsMethodDelegate.Base(), staticTest)?.Invoke(456, ref f));
        }

        [Test]
        public void Test_AccessTools2_GetDelegate_InvalidDelegates()
        {
            Assert.IsNull(AccessTools2.GetDelegate<Action>(interfaceTest));
            Assert.IsNull(AccessTools2.GetDelegate<Func<bool>>(baseTest));
            Assert.IsNull(AccessTools2.GetDelegate<Action<string>>(derivedTest));
            Assert.IsNull(AccessTools2.GetDelegate<Func<int, float, string>>(structTest));
        }
    }
}