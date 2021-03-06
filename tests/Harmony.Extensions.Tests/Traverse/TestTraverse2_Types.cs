using HarmonyLib.BUTR.Extensions;
using HarmonyLibTests.Traverse.Assets;

using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

namespace HarmonyLibTests.Traverse
{
    [TestFixture]
    public class TestTraverse2_Types : TestLogger
    {
        class InnerClass { }

        [Test]
        public void Traverse2_Types()
        {
            var instance = new Traverse2Types<InnerClass>();
            var trv = Traverse2.Create(instance);

            Assert.AreEqual(
                100,
                trv.Field("IntField").GetValue<int>()
            );

            Assert.AreEqual(
                "hello",
                trv.Field("StringField").GetValue<string>()
            );

            var boolArray = trv.Field("ListOfBoolField").GetValue<IEnumerable<bool>>().ToArray();
            Assert.AreEqual(true, boolArray[0]);
            Assert.AreEqual(false, boolArray[1]);

            var mixed = trv.Field("MixedField").GetValue<Dictionary<InnerClass, List<string>>>();
            var key = trv.Field("key").GetValue<InnerClass>();

            _ = mixed.TryGetValue(key, out var value);
            Assert.AreEqual("world", value.First());

            var trvEmpty = Traverse2.Create(instance).Type("FooBar");
            TestTraverse2_Basics.AssertIsEmpty(trvEmpty);
        }

        [Test]
        public void Traverse2_InnerInstance()
        {
            var instance = new Traverse2NestedTypes(null);

            var trv1 = Traverse2.Create(instance);
            var field1 = trv1.Field("innerInstance").Field("inner2").Field("field");
            _ = field1.SetValue("somevalue");

            var trv2 = Traverse2.Create(instance);
            var field2 = trv2.Field("innerInstance").Field("inner2").Field("field");
            Assert.AreEqual("somevalue", field2.GetValue());
        }

#if !NET5_0 // writing to static fields after init not allowed in NET5
        [Test]
        public void Traverse2_InnerStatic()
        {
            var trv1 = Traverse2.Create(typeof(Traverse2NestedTypes));
            var field1 = trv1.Field("innerStatic").Field("inner2").Field("field");
            _ = field1.SetValue("somevalue1");

            var trv2 = Traverse2.Create(typeof(Traverse2NestedTypes));
            var field2 = trv2.Field("innerStatic").Field("inner2").Field("field");
            Assert.AreEqual("somevalue1", field2.GetValue());

            _ = new Traverse2NestedTypes("somevalue2");
            var value = Traverse2
                .Create(typeof(Traverse2NestedTypes))
                .Type("InnerStaticClass1")
                .Type("InnerStaticClass2")
                .Field("field")
                .GetValue<string>();
            Assert.AreEqual("somevalue2", value);
        }
#endif
    }
}