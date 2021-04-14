using HarmonyLib;

namespace HarmonyLibTests.Tools
{
    public partial class TestRefAccess
    {
        static IATestCase<T, F> ATestCase<T, F>(AccessTools.StructFieldRef<T, F> fieldRef) where T : struct
        {
            return new StructFieldRefTestCase<T, F>(fieldRef);
        }

        class StructFieldRefTestCase<T, F> : IATestCase<T, F> where T : struct
        {
            readonly AccessTools.StructFieldRef<T, F> fieldRef;

            public StructFieldRefTestCase(AccessTools.StructFieldRef<T, F> fieldRef)
            {
                this.fieldRef = fieldRef;
            }

            public F Get(ref T instance)
            {
                return fieldRef(ref instance);
            }

            public void Set(ref T instance, F value)
            {
                fieldRef(ref instance) = value;
            }
        }
    }
}