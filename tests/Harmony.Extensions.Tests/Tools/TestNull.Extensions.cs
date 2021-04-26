using HarmonyLib.BUTR.Extensions;

using NUnit.Framework;

namespace HarmonyLibTests.Tools
{
    public partial class TestNull
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