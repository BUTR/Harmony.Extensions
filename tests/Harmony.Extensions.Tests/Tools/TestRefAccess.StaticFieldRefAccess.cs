using HarmonyLib;

namespace HarmonyLibTests.Tools
{
    public partial class TestRefAccess
    {
        // AccessTools2.StaticFieldRefAccess
        static IATestCase<T, F> ATestCase<T, F>(AccessTools.FieldRef<F> fieldRef) where T : class
        {
            return new StaticFieldRefTestCase<T, F>(fieldRef);
        }

        class StaticFieldRefTestCase<T, F> : IATestCase<T, F> where T : class
        {
            readonly AccessTools.FieldRef<F> fieldRef;

            public StaticFieldRefTestCase(AccessTools.FieldRef<F> fieldRef)
            {
                this.fieldRef = fieldRef;
            }

            public F Get(ref T instance)
            {
                return fieldRef();
            }

            public void Set(ref T instance, F value)
            {
                fieldRef() = value;
            }
        }
    }
}