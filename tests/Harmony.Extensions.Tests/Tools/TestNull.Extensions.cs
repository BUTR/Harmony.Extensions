using HarmonyLib.BUTR.Extensions;

using NUnit.Framework;

using System;
using System.Reflection;

namespace HarmonyLibTests.Tools
{
    [TestFixture]
    public partial class TestNull : TestLogger
    {
        private static void TestMethod() { }

        [Test]
        public void Test_HarmonyExtensions_Constructors()
        {
            Assert.IsFalse(HarmonyExtensions.TryPatch(null!, null, null, null, null, null));
            Assert.IsFalse(HarmonyExtensions.TryPatch(null!, SymbolExtensions2.GetMethodInfo(() => TestMethod()), null, null, null, null));
            Assert.IsNull(HarmonyExtensions.TryCreateReversePatcher(null!, null, null));
            Assert.IsNull(HarmonyExtensions.TryCreateReversePatcher(null!, SymbolExtensions2.GetMethodInfo(() => TestMethod()), null));
        }
    }
}