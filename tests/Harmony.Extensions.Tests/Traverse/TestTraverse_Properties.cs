using HarmonyLib.BUTR.Extensions;
using HarmonyLibTests.Traverse.Assets;

using NUnit.Framework;

namespace HarmonyLibTests.Traverse
{
    [TestFixture]
	public class TestTraverse2_Properties : TestLogger
	{
		// Traverse2.ToString() should return the value of a Traverse2d property
		//
		[Test]
		public void Traverse2_Property_ToString()
		{
			var instance = new TraverseProperties_AccessModifiers(TraverseProperties.testStrings);

			var trv = Traverse2.Create(instance).Property(TraverseProperties.propertyNames[0]);
			Assert.AreEqual(TraverseProperties.testStrings[0], trv.ToString());
		}

		// Traverse2.Property() should return static properties
		//
		[Test]
		public void Traverse2_Property_Static()
		{
			var instance = new Traverse_BaseClass();

			var trv1 = Traverse2.Create(instance).Property("StaticProperty");
			Assert.AreEqual("test1", trv1.GetValue());


			var trv2 = Traverse2.Create(typeof(TraverseProperties_Static)).Property("StaticProperty");
			Assert.AreEqual("test2", trv2.GetValue());
		}

		// Traverse2.GetValue() should return the value of a Traverse2d property
		// regardless of its access modifier
		//
		[Test]
		public void Traverse2_Property_GetValue()
		{
			var instance = new TraverseProperties_AccessModifiers(TraverseProperties.testStrings);
			var trv = Traverse2.Create(instance);

			for (var i = 0; i < TraverseProperties.testStrings.Length; i++)
			{
				var name = TraverseProperties.propertyNames[i];
				var ptrv = trv.Property(name);
				Assert.NotNull(ptrv);
				Assert.AreEqual(TraverseProperties.testStrings[i], ptrv.GetValue());
				Assert.AreEqual(TraverseProperties.testStrings[i], ptrv.GetValue<string>());
			}
		}

		// Traverse2.SetValue() should set the value of a Traverse2d property
		// regardless of its access modifier
		//
		[Test]
		public void Traverse2_Property_SetValue()
		{
			var instance = new TraverseProperties_AccessModifiers(TraverseProperties.testStrings);
			var trv = Traverse2.Create(instance);

			for (var i = 0; i < TraverseProperties.testStrings.Length - 1; i++)
			{
				var newValue = "newvalue" + i;

				// before
				Assert.AreEqual(TraverseProperties.testStrings[i], instance.GetTestProperty(i));

				var name = TraverseProperties.propertyNames[i];
				var ptrv = trv.Property(name);
				Assert.NotNull(ptrv);
				_ = ptrv.SetValue(newValue);

				// after
				Assert.AreEqual(newValue, instance.GetTestProperty(i));
				Assert.AreEqual(newValue, ptrv.GetValue());
				Assert.AreEqual(newValue, ptrv.GetValue<string>());
			}
		}
	}
}