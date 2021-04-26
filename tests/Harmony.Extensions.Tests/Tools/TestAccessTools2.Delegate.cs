using HarmonyLib.BUTR.Extensions;
using HarmonyLibTests.Tools.Assets;

using NUnit.Framework;

using System;
using System.Reflection;

namespace HarmonyLibTests.Tools
{
    public partial class Test_AccessTools2
    {
        delegate string MethodDel(int n, ref float f);
        delegate string OpenMethodDel<T>(T instance, int n, ref float f);

        static readonly MethodInfo interfaceTest = typeof(AccessTools2MethodDelegate.IInterface).GetMethod("Test");
        static readonly MethodInfo baseTest = typeof(AccessTools2MethodDelegate.Base).GetMethod("Test");
        static readonly MethodInfo derivedTest = typeof(AccessTools2MethodDelegate.Derived).GetMethod("Test");
        static readonly MethodInfo structTest = typeof(AccessTools2MethodDelegate.Struct).GetMethod("Test");
        static readonly MethodInfo staticTest = typeof(AccessTools2MethodDelegate).GetMethod("Test");

        [Test]
        public void Test_AccessTools2_GetDelegate_ClosedInstanceDelegates()
        {
            var f = 789f;
            var baseInstance = new AccessTools2MethodDelegate.Base();
            var derivedInstance = new AccessTools2MethodDelegate.Derived();
            var structInstance = new AccessTools2MethodDelegate.Struct();
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
            var baseInstance = new AccessTools2MethodDelegate.Base();
            var derivedInstance = new AccessTools2MethodDelegate.Derived();
            var structInstance = new AccessTools2MethodDelegate.Struct();
            Assert.AreEqual("base test 456 790 1", AccessTools2.GetDelegate<MethodDel>(baseInstance, interfaceTest)!(456, ref f));
            Assert.AreEqual("derived test 456 791 1", AccessTools2.GetDelegate<MethodDel>(derivedInstance, interfaceTest)!(456, ref f));
            Assert.AreEqual("struct result 456 792 1", AccessTools2.GetDelegate<MethodDel>(structInstance, interfaceTest)!(456, ref f));
        }

        [Test]
        public void Test_AccessTools2_GetDelegate_OpenInstanceDelegates()
        {
            var f = 789f;
            var baseInstance = new AccessTools2MethodDelegate.Base();
            var derivedInstance = new AccessTools2MethodDelegate.Derived();
            var structInstance = new AccessTools2MethodDelegate.Struct();
            Assert.AreEqual("base test 456 790 1", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.Base>>(baseTest)!(baseInstance, 456, ref f));
            Assert.AreEqual("derived test 456 791 1", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.Base>>(baseTest)!(derivedInstance, 456, ref f));
            _ = Assert.Throws(typeof(InvalidCastException), () => AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.Base>>(derivedTest)!(baseInstance, 456, ref f));
            Assert.AreEqual("derived test 456 792 2", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.Base>>(derivedTest)!(derivedInstance, 456, ref f));
            //AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Derived>>(derivedTest)(baseInstance, 456, ref f); // expected compile error
            Assert.AreEqual("derived test 456 793 3", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.Derived>>(derivedTest)!(derivedInstance, 456, ref f));
            Assert.AreEqual("struct result 456 794 1", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.Struct>>(structTest)!(structInstance, 456, ref f));
        }

        [Test]
        public void Test_AccessTools2_GetDelegate_OpenInstanceDelegates_DelegateInterfaceInstanceType()
        {
            var f = 789f;
            var baseInstance = new AccessTools2MethodDelegate.Base();
            var derivedInstance = new AccessTools2MethodDelegate.Derived();
            var structInstance = new AccessTools2MethodDelegate.Struct();
            Assert.AreEqual("base test 456 790 1", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.IInterface>>(baseTest)!(baseInstance, 456, ref f));
            Assert.AreEqual("derived test 456 791 1", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.IInterface>>(baseTest)!(derivedInstance, 456, ref f));
            _ = Assert.Throws(typeof(InvalidCastException), () => AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.IInterface>>(derivedTest)!(baseInstance, 456, ref f));
            Assert.AreEqual("derived test 456 792 2", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.IInterface>>(derivedTest)!(derivedInstance, 456, ref f));
            Assert.AreEqual("struct result 456 793 1", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.IInterface>>(structTest)!(structInstance, 456, ref f));
        }

        [Test]
        public void Test_AccessTools2_GetDelegate_OpenInstanceDelegates_InterfaceMethod()
        {
            var f = 789f;
            var baseInstance = new AccessTools2MethodDelegate.Base();
            var derivedInstance = new AccessTools2MethodDelegate.Derived();
            var structInstance = new AccessTools2MethodDelegate.Struct();
            Assert.AreEqual("base test 456 790 1", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.IInterface>>(interfaceTest)!(baseInstance, 456, ref f));
            Assert.AreEqual("derived test 456 791 1", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.IInterface>>(interfaceTest)!(derivedInstance, 456, ref f));
            Assert.AreEqual("struct result 456 792 1", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.IInterface>>(interfaceTest)!(structInstance, 456, ref f));
            Assert.AreEqual("base test 456 793 2", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.Base>>(interfaceTest)!(baseInstance, 456, ref f));
            Assert.AreEqual("derived test 456 794 2", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.Base>>(interfaceTest)!(derivedInstance, 456, ref f));
            //AccessTools2.GetDelegate<OpenMethodDel<AccessToolsMethodDelegate.Derived>>(interfaceTest)!(baseInstance, 456, ref f)); // expected compile error
            Assert.AreEqual("derived test 456 795 3", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.Derived>>(interfaceTest)!(derivedInstance, 456, ref f));
            Assert.AreEqual("struct result 456 796 1", AccessTools2.GetDelegate<OpenMethodDel<AccessTools2MethodDelegate.Struct>>(interfaceTest)!(structInstance, 456, ref f));
        }

        [Test]
        public void Test_AccessTools2_GetDelegate_StaticDelegates_InterfaceMethod()
        {
            var f = 789f;
            Assert.AreEqual("static test 456 790 1", AccessTools2.GetDelegate<MethodDel>(staticTest)!(456, ref f));
            Assert.IsNull(AccessTools2.GetDelegate<MethodDel>(new AccessTools2MethodDelegate.Base(), staticTest)?.Invoke(456, ref f));
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