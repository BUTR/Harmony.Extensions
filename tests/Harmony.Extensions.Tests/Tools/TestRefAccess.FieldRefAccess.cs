using HarmonyLib;

namespace HarmonyLibTests.Tools
{
    public partial class TestRefAccess
    {
        // AccessTools2.FieldRefAccess
        // Note: This can't have generic class constraint since there are some FieldRefAccess methods that work with struct static fields.
        static IATestCase<T, F> ATestCase<T, F>(AccessTools.FieldRef<T, F> fieldRef) where T : class
        {
            return new ClassFieldRefTestCase<T, F>(fieldRef);
        }

        class ClassFieldRefTestCase<T, F> : IATestCase<T, F> where T : class
        {
            readonly AccessTools.FieldRef<T, F> fieldRef;

            public ClassFieldRefTestCase(AccessTools.FieldRef<T, F> fieldRef)
            {
                this.fieldRef = fieldRef;
            }

            public F Get(ref T instance)
            {
                return fieldRef(instance);
            }

            public void Set(ref T instance, F value)
            {
                fieldRef(instance) = value;
            }
        }
    }
}