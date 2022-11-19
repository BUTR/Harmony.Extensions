using System;
using System.Collections.Generic;
using System.Linq;

namespace HarmonyLibTests.Traverse.Assets
{
    public class Traverse2_ExtraClass
    {
        public readonly string someString = "-";
        public readonly Traverse2_BaseClass baseClass = new Traverse2_BaseClass();

        public Traverse2_ExtraClass(string val)
        {
            someString = val;
        }
    }

    public class Traverse2_BaseClass
    {
        string _basePropertyField1;
        protected virtual string BaseProperty1
        {
            get => _basePropertyField1;
            set => _basePropertyField1 = value;
        }

        string _basePropertyField2;
        protected virtual string BaseProperty2
        {
            get => _basePropertyField2;
            set => _basePropertyField2 = value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public string BaseProperty3
        {
            get => throw new Exception();
            set => throw new Exception();
        }

        static string staticField = "test1";
        private string baseField = "base-field";

        private static string StaticProperty { get; set; } = "test1";
        private string BaseProperty { get; set; } =  "base-property";

        private string BaseMethod() { return "base-method"; }
    }

    public class Traverse2Types<T> where T : new()
    {
#pragma warning disable IDE0052
#pragma warning disable CS0414
        private readonly int IntField;
        private readonly string StringField;
        private readonly Type TypeField;
        private readonly IEnumerable<bool> ListOfBoolField;
        private readonly Dictionary<T, List<string>> MixedField;
#pragma warning restore CS0414
#pragma warning restore IDE0052

        public T key;

        public Traverse2Types()
        {
            IntField = 100;
            StringField = "hello";
            TypeField = typeof(Console);
            ListOfBoolField = (new bool[] { false, true }).Select(b => !b);

            var d = new Dictionary<T, List<string>>();
            var l = new List<string> { "world" };
            key = new T();
            d.Add(key, l);
            MixedField = d;
        }
    }

    public class Traverse2NestedTypes
    {
        class InnerClass1
        {
            class InnerClass2
            {
#pragma warning disable IDE0052
#pragma warning disable CS0414
                private string field;
#pragma warning restore CS0414
#pragma warning restore IDE0052

                public InnerClass2()
                {
                    field = "helloInstance";
                }
            }

#pragma warning disable IDE0052
            readonly InnerClass2 inner2;
#pragma warning restore IDE0052

            public InnerClass1()
            {
                inner2 = new InnerClass2();
            }
        }

        class InnerStaticFieldClass1
        {
            class InnerStaticFieldClass2
            {
#pragma warning disable CS0414
                static string field = "helloStatic";
#pragma warning restore CS0414
            }

#pragma warning disable IDE0052
            static InnerStaticFieldClass2 inner2 = new InnerStaticFieldClass2();
#pragma warning restore IDE0052
        }

        protected static class InnerStaticClass1
        {
            internal static class InnerStaticClass2
            {
                internal static string field;
            }
        }

#pragma warning disable IDE0052
        readonly InnerClass1 innerInstance;
        static InnerStaticFieldClass1 innerStatic = new InnerStaticFieldClass1();
#pragma warning restore IDE0052

        public Traverse2NestedTypes(string staticValue)
        {
            innerInstance = new InnerClass1();
            InnerStaticClass1.InnerStaticClass2.field = staticValue;
        }
    }
}