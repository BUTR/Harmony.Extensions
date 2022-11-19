using HarmonyLib.BUTR.Extensions;
using HarmonyLibTests.Traverse.Assets;

using NUnit.Framework;

namespace HarmonyLibTests.Traverse
{
    [TestFixture]
    public class TestTraverse2_Fields : TestLogger
    {
        // Traverse2.ToString() should return the value of a Traverse2d field
        //
        [Test]
        public void Traverse2_Field_ToString()
        {
            var instance = new Traverse2Fields_AccessModifiers(Traverse2Fields.testStrings);

            var trv = Traverse2.Create(instance).Field(Traverse2Fields.fieldNames[0]);
            Assert.AreEqual(Traverse2Fields.testStrings[0], trv.ToString());
        }

        // Traverse2.GetValue() should return the value of a Traverse2d field
        // regardless of its access modifier
        //
        [Test]
        public void Traverse2_Field_GetValue()
        {
            var instance = new Traverse2Fields_AccessModifiers(Traverse2Fields.testStrings);
            var trv = Traverse2.Create(instance);

            for (var i = 0; i < Traverse2Fields.testStrings.Length; i++)
            {
                var name = Traverse2Fields.fieldNames[i];
                var ftrv = trv.Field(name);
                Assert.NotNull(ftrv);

                Assert.AreEqual(Traverse2Fields.testStrings[i], ftrv.GetValue());
                Assert.AreEqual(Traverse2Fields.testStrings[i], ftrv.GetValue<string>());

                ftrv.SetValue("test");
                Assert.AreEqual("test", ftrv.GetValue());
            }
        }

        // Traverse2.Field() should return the value of a Traverse2d static field
        //
        [Test]
        public void Traverse2_Field_Static()
        {
            var instance = new Traverse2_BaseClass();

            var trv1 = Traverse2.Create(instance).Field("staticField");
            Assert.AreEqual("test1", trv1.GetValue());

            trv1.SetValue("test_1");
            Assert.AreEqual("test_1", trv1.GetValue());


            var trv2 = Traverse2.Create(typeof(Traverse2Fields_Static)).Field("staticField");
            Assert.AreEqual("test2", trv2.GetValue());

            trv2.SetValue("test_2");
            Assert.AreEqual("test_2", trv2.GetValue());
        }

        // Traverse2.Field().Field() should continue the Traverse2 chain for static and non-static fields
        //
        [Test]
        public void Traverse2_Static_Field_Instance_Field()
        {
            var extra = new Traverse2_ExtraClass("test1");
            Assert.AreEqual("test1", Traverse2.Create(extra).Field("someString").GetValue());

            Assert.AreEqual("test2", Traverse2Fields_Static.extraClassInstance.someString, "direct");

            var trv = Traverse2.Create(typeof(Traverse2Fields_Static));
            var trv2 = trv.Field("extraClassInstance");
            Assert.AreEqual(typeof(Traverse2_ExtraClass), trv2.GetValue().GetType());
            Assert.AreEqual("test2", trv2.Field("someString").GetValue(), "Traverse2");
        }

        // Traverse2.Field().Field() should continue the Traverse2 chain for static and non-static fields
        //
        [Test]
        public void Traverse2_Instance_Field_Static_Field()
        {
            var instance = new Traverse2_ExtraClass("test3");
            Assert.AreEqual(typeof(Traverse2_BaseClass), instance.baseClass.GetType());

            var trv1 = Traverse2.Create(instance);
            Assert.NotNull(trv1, "trv1");

            var trv2 = trv1.Field("baseClass");
            Assert.NotNull(trv2, "trv2");

            var val = trv2.GetValue();
            Assert.NotNull(val, "val");
            Assert.AreEqual(typeof(Traverse2_BaseClass), val.GetType());

            var trv3 = trv2.Field("baseField");
            Assert.NotNull(trv3, "trv3");
            Assert.AreEqual("base-field", trv3.GetValue());
        }

        // Traverse2.SetValue() should set the value of a Traverse2d field
        // regardless of its access modifier
        //
        [Test]
        public void Traverse2_Field_SetValue()
        {
            var instance = new Traverse2Fields_AccessModifiers(Traverse2Fields.testStrings);
            var trv = Traverse2.Create(instance);

            for (var i = 0; i < Traverse2Fields.testStrings.Length; i++)
            {
                var newValue = "newvalue" + i;

                // before
                Assert.AreEqual(Traverse2Fields.testStrings[i], instance.GetTestField(i));

                var name = Traverse2Fields.fieldNames[i];
                var ftrv = trv.Field(name);
                _ = ftrv.SetValue(newValue);

                // after
                Assert.AreEqual(newValue, instance.GetTestField(i));
                Assert.AreEqual(newValue, ftrv.GetValue());
                Assert.AreEqual(newValue, ftrv.GetValue<string>());
            }
        }
    }
}