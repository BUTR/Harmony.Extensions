using System.Linq;

namespace HarmonyLibTests.Traverse.Assets
{
#pragma warning disable CS0414
#pragma warning disable IDE0052

    public class Traverse2Properties
    {
        public static string[] testStrings = new string[] { "test01", "test02", "test03", "test04", "test05", "test06", "test07" };
        public static string[] propertyNames = new string[] { "PublicProperty", "PublicPrivateProperty", "AutoProperty", "BaseProperty1", "BaseProperty2", "BaseProperty3", "ImmediateProperty" };
    }

    public static class Traverse2Properties_Static
    {
        static string StaticProperty => "test2";
    }

    public class Traverse2Properties_AccessModifiers : Traverse2_BaseClass
    {
        string _publicPropertyField;
        public string PublicProperty
        {
            get => _publicPropertyField;
            set => _publicPropertyField = value;
        }

        string _publicPrivatePropertyField;
        public string PublicPrivateProperty
        {
            get => _publicPrivatePropertyField;
            set => _publicPrivatePropertyField = value;
        }

        string AutoProperty { get; set; }

        protected override string BaseProperty1
        {
            get => base.BaseProperty1;
            set => base.BaseProperty1 = value;
        }

        // BaseProperty2 defined and used in base class

        string _basePropertyField3;
        public new string BaseProperty3
        {
            get => _basePropertyField3;
            set => _basePropertyField3 = value;
        }

        string ImmediateProperty => Traverse2Properties.testStrings.Last();

        // TODO: should this really be suppressed?
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Traverse2Properties_AccessModifiers(string[] s)
        {
            PublicProperty = s[0];
            PublicPrivateProperty = s[1];
            AutoProperty = s[2];
            BaseProperty1 = s[3];
            BaseProperty2 = s[4];
            BaseProperty3 = s[5];
            // immediateProperty is readonly
        }

        public string GetTestProperty(int n)
        {
            switch (n)
            {
                case 0:
                    return PublicProperty;
                case 1:
                    return PublicPrivateProperty;
                case 2:
                    return AutoProperty;
                case 3:
                    return BaseProperty1;
                case 4:
                    return BaseProperty2;
                case 5:
                    return BaseProperty3;
                case 6:
                    return ImmediateProperty;
                default:
                    return null;
            }
        }

        public void SetTestProperty(int n, string value)
        {
            switch (n)
            {
                case 0:
                    PublicProperty = value;
                    break;
                case 1:
                    PublicPrivateProperty = value;
                    break;
                case 2:
                    AutoProperty = value;
                    break;
                case 3:
                    BaseProperty1 = value;
                    break;
                case 4:
                    BaseProperty2 = value;
                    break;
                case 5:
                    BaseProperty3 = value;
                    break;
                case 6:
                    // ImmediateProperty = value;
                    break;
            }
        }

        public override string ToString()
        {
            return "TraverseProperties_AccessModifiers";
        }
    }

    public class Traverse2Properties_SubClass : Traverse2_BaseClass
    {
    }

#pragma warning restore IDE0052
#pragma warning restore CS0414
}