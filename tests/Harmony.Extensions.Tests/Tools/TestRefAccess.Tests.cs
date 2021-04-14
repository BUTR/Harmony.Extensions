using HarmonyLib;
using HarmonyLib.BUTR.Extensions;
using HarmonyLibTests.Assets;

using NUnit.Framework;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace HarmonyLibTests.Tools
{
    public partial class TestRefAccess
	{
        static Dictionary<string, IATestCase<T, F>> AvailableTestCases_FieldRefAccess_ByName<T, F>(string fieldName) where T : class
		{
			return new()
            {
				//["FieldRefAccess<T, F>(fieldName)(instance)"] = ATestCase<T, F>(instance => ref AccessTools.FieldRefAccess<T, F>(fieldName)(instance)),
				//["FieldRefAccess<T, F>(instance, fieldName)"] = ATestCase<T, F>(instance => ref AccessTools.FieldRefAccess<T, F>(instance, fieldName)),
				["FieldRefAccess<F>(typeof(T), fieldName)(instance)"] = ATestCase<T, F>(instance => ref AccessTools2.FieldRefAccess<F>(typeof(T), fieldName)!(instance)),
				["FieldRefAccess<F>(typeof(T), fieldName)()"] = ATestCase<T, F>(instance => ref AccessTools2.FieldRefAccess<F>(typeof(T), fieldName)!()),
			};
		}

		static Dictionary<string, IATestCase<T, F>> AvailableTestCases_FieldRefAccess_ByFieldInfo<T, F>(FieldInfo field) where T : class
		{
			return new()
            {
				["FieldRefAccess<T, F>(field)(instance)"] = ATestCase<T, F>(instance => ref AccessTools2.FieldRefAccess<T, F>(field)!(instance)),
				["FieldRefAccess<T, F>(field)()"] = ATestCase<T, F>(instance => ref AccessTools2.FieldRefAccess<T, F>(field)!()),
				//["FieldRefAccess<T, F>(instance, field)"] = ATestCase<T, F>(instance => ref AccessTools.FieldRefAccess<T, F>(instance, field)),
			};
		}

		static Dictionary<string, IATestCase<T, F>> AvailableTestCases_StructFieldRefAccess<T, F>(FieldInfo field, string fieldName) where T : struct
		{
			return new()
            {
				["StructFieldRefAccess<T, F>(fieldName)(ref instance)"] = ATestCase((ref T instance) => ref AccessTools2.StructFieldRefAccess<T, F>(fieldName)!(ref instance)),
				//["StructFieldRefAccess<T, F>(ref instance, fieldName)"] = ATestCase((ref T instance) => ref AccessTools.StructFieldRefAccess<T, F>(ref instance, fieldName)),
				["StructFieldRefAccess<T, F>(field)(ref instance)"] = ATestCase((ref T instance) => ref AccessTools2.StructFieldRefAccess<T, F>(field)!(ref instance)),
				//["StructFieldRefAccess<T, F>(ref instance, field)"] = ATestCase((ref T instance) => ref AccessTools.StructFieldRefAccess<T, F>(ref instance, field)),
			};
		}

		static Dictionary<string, IATestCase<T, F>> AvailableTestCases_StaticFieldRefAccess_ByName<T, F>(string fieldName) where T : class
		{
			return new()
            {
				//["StaticFieldRefAccess<T, F>(fieldName)"] = ATestCase<T, F>(() => ref AccessTools.StaticFieldRefAccess<T, F>(fieldName)),
				//["StaticFieldRefAccess<F>(typeof(T), fieldName)"] = ATestCase<T, F>(() => ref AccessTools.StaticFieldRefAccess<F>(typeof(T), fieldName)),
			};
		}

        static Dictionary<string, IATestCase<T, F>> AvailableTestCases_StaticFieldRefAccess_ByFieldInfo<T, F>(FieldInfo field) where T : class
		{
			return new()
            {
				["StaticFieldRefAccess<F>(field)()"] = ATestCase<T, F>(() => ref AccessTools2.StaticFieldRefAccess<F>(field)!()),
				//["StaticFieldRefAccess<T, F>(field)"] = ATestCase<T, F>(() => ref AccessTools.StaticFieldRefAccess<T, F>(field)),
			};
		}


        [Test]
		public void Test_ClassInstance_ProtectedString()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsClass), "field1");
				var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
				TestSuite_Class<AccessToolsClass, AccessToolsClass, string>(
					field, "field1test", expectedCaseToConstraint);
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, string>(
					field, "field1test", expectedCaseToConstraint);
				TestSuite_Class<AccessToolsClass, AccessToolsSubClass, string>(
					field, "field1test", expectedCaseToConstraint);
				TestSuite_Class<IAccessToolsType, AccessToolsClass, string>(
					field, "field1test", FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, AccessToolsClass, string>(
					field, "field1test", FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, string, string>(
					field, "field1test", IncompatibleInstanceType(expectedCaseToConstraint));
				TestSuite_Class<string, string, string>(
					field, "field1test", IncompatibleTypeT(expectedCaseToConstraint));
				TestSuite_Struct<int, string>(
					field, "field1test", expectedCaseToConstraint_ClassInstance_StructT);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, object>(
					field, "field1test", expectedCaseToConstraint);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, IComparable>(
					field, "field1test", expectedCaseToConstraint);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, string[]>(
					field, new[] { "should always throw" }, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, int>(
					field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_ClassInstance_PublicReadonlyFloat()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsClass), "field2");
				var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
				TestSuite_Class<AccessToolsClass, AccessToolsClass, float>(
					field, 314f, expectedCaseToConstraint);
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, float>(
					field, 314f, expectedCaseToConstraint);
				TestSuite_Class<AccessToolsClass, AccessToolsSubClass, float>(
					field, 314f, expectedCaseToConstraint);
				TestSuite_Class<IAccessToolsType, AccessToolsClass, float>(
					field, 314f, FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, AccessToolsClass, float>(
					field, 314f, FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, string, float>(
					field, 314f, IncompatibleInstanceType(expectedCaseToConstraint));
				TestSuite_Class<string, string, float>(
					field, 314f, IncompatibleTypeT(expectedCaseToConstraint));
				TestSuite_Struct<int, float>(
					field, 314f, expectedCaseToConstraint_ClassInstance_StructT);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, object>(
					field, 314f, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, ValueType>(
					field, 314f, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, float?>(
					field, 314f, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, IComparable>(
					field, 314f, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, double>(
					field, 314f, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_ClassStatic_PublicLong()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsClass), "field3");
				var expectedCaseToConstraint = expectedCaseToConstraint_ClassStatic;
				// Note: As this is as static field, instance type is ignored, so IncompatibleInstanceType is never needed.
				TestSuite_Class<AccessToolsClass, AccessToolsClass, long>(
					field, 314L, expectedCaseToConstraint);
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, long>(
					field, 314L, expectedCaseToConstraint);
				TestSuite_Class<IAccessToolsType, AccessToolsClass, long>(
					field, 314L, FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, AccessToolsClass, long>(
					field, 314L, FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, string, long>(
					field, 314L, FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<string, string, long>(
					field, 314L, FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Struct<int, long>(
					field, 314L, expectedCaseToConstraint_ClassStatic_StructT);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, object>(
					field, 314L, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, ValueType>(
					field, 314L, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, long?>(
					field, 314L, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, IComparable>(
					field, 314L, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, double>(
					field, 314L, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_ClassStatic_PrivateReadonlyString()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsClass), "field4");
				var expectedCaseToConstraint = expectedCaseToConstraint_ClassStatic;
				// Note: As this is as static field, instance type is ignored, so IncompatibleInstanceType is never needed.
				TestSuite_Class<AccessToolsClass, AccessToolsClass, string>(
					field, "field4test", expectedCaseToConstraint);
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, string>(
					field, "field4test", expectedCaseToConstraint);
				TestSuite_Class<IAccessToolsType, AccessToolsClass, string>(
					field, "field4test", FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, AccessToolsClass, string>(
					field, "field4test", FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, string, string>(
					field, "field4test", FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<string, string, string>(
					field, "field4test", FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Struct<int, string>(
					field, "field4test", expectedCaseToConstraint_ClassStatic_StructT);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, object>(
					field, "field4test", expectedCaseToConstraint);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, IComparable>(
					field, "field4test", expectedCaseToConstraint);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, IEnumerable<string>>(
					field, new[] { "should always throw" }, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, int>(
					field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}


		[Test]
		public void Test_ClassInstance_PrivateClassFieldType()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsClass), "field5");
				var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
				// Type of field is AccessToolsClass.Inner, which is a private class.
				static IInner TestValue()
				{
					return AccessToolsClass.NewInner(987);
				}
				TestSuite_Class<AccessToolsClass, AccessToolsClass, IInner>(
					field, TestValue(), expectedCaseToConstraint);
				TestSuite_Class<IAccessToolsType, AccessToolsClass, IInner>(
					field, TestValue(), FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, AccessToolsClass, IInner>(
					field, TestValue(), FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, string, IInner>(
					field, TestValue(), IncompatibleInstanceType(expectedCaseToConstraint));
				TestSuite_Class<string, string, IInner>(
					field, TestValue(), IncompatibleTypeT(expectedCaseToConstraint));
				TestSuite_Struct<int, IInner>(
					field, TestValue(), expectedCaseToConstraint_ClassInstance_StructT);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, object>(
					field, TestValue(), expectedCaseToConstraint);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, string[]>(
					field, new[] { "should always throw" }, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, int>(
					field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_ClassInstance_ArrayOfPrivateClassFieldType()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsClass), "field6");
				var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
				// Type of field is AccessToolsClass.Inner[], the element type of which is a private class.
				static IList TestValue()
				{
					// IInner[] can't be cast to AccessTools.Inner[], so must create an actual AccessTools.Inner[].
					var array = (IList)Array.CreateInstance(AccessTools.Inner(typeof(AccessToolsClass), "Inner"), 2);
					array[0] = AccessToolsClass.NewInner(123);
					array[1] = AccessToolsClass.NewInner(456);
					return array;
				}
				TestSuite_Class<AccessToolsClass, AccessToolsClass, IList>(
					field, TestValue(), expectedCaseToConstraint);
				TestSuite_Class<IAccessToolsType, AccessToolsClass, IList>(
					field, TestValue(), FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, AccessToolsClass, IList>(
					field, TestValue(), FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, string, IList>(
					field, TestValue(), IncompatibleInstanceType(expectedCaseToConstraint));
				TestSuite_Class<string, string, IList>(
					field, TestValue(), IncompatibleTypeT(expectedCaseToConstraint));
				TestSuite_Struct<int, IList>(
					field, TestValue(), expectedCaseToConstraint_ClassInstance_StructT);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, IInner[]>(
					field, (IInner[])TestValue(), expectedCaseToConstraint); // AccessTools.Inner[] can be cast to IInner[]
				TestSuite_Class<AccessToolsClass, AccessToolsClass, object>(
					field, TestValue(), expectedCaseToConstraint);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, IList>(
					field, TestValue(), expectedCaseToConstraint);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, string[]>(
					field, new[] { "should always throw" }, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, int>(
					field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_ClassInstance_PrivateStructFieldType()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsClass), "field7");
				var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
				// Type of field is AccessToolsClass.InnerStruct, which is a private struct.
				// As it's a value type and references cannot be made to boxed value type instances, FieldRefValue will never work.
				static IInner TestValue()
				{
					return AccessToolsClass.NewInnerStruct(-987);
				}
				TestSuite_Class<AccessToolsClass, AccessToolsClass, IInner>(
					field, TestValue(), IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<int, IInner>(
					field, TestValue(), expectedCaseToConstraint_ClassInstance_StructT);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, object>(
					field, TestValue(), IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, ValueType>(
					field, (ValueType)TestValue(), IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, int>(
					field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_ClassInstance_ListOfPrivateStructFieldType()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsClass), "field8");
				var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
				// Type of field is List<AccessToolsClass.Inner>, the element type of which is a private struct.
				// Although AccessToolsClass.Inner is a value type, List is not, so FieldRefValue works normally.
				static IList TestValue()
				{
					// List<IInner> can't be cast to List<AccessTools.Inner>, so must create an actual List<AccessTools.Inner>.
					var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(AccessTools.Inner(typeof(AccessToolsClass), "InnerStruct")));
					_ = list.Add(AccessToolsClass.NewInnerStruct(-123));
					_ = list.Add(AccessToolsClass.NewInnerStruct(-456));
					return list;
				}
				TestSuite_Class<AccessToolsClass, AccessToolsClass, IList>(
					field, TestValue(), expectedCaseToConstraint);
				TestSuite_Class<IAccessToolsType, AccessToolsClass, IList>(
					field, TestValue(), FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, AccessToolsClass, IList>(
					field, TestValue(), FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, string, IList>(
					field, TestValue(), IncompatibleInstanceType(expectedCaseToConstraint));
				TestSuite_Class<string, string, IList>(
					field, TestValue(), IncompatibleTypeT(expectedCaseToConstraint));
				TestSuite_Struct<int, IList>(
					field, TestValue(), expectedCaseToConstraint_ClassInstance_StructT);
				// List<T> is invariant - List<AccessTools.Inner> cannot be cast to List<IInner> nor vice versa,
				// so can't do TestSuite_Class<AccessToolsClass, AccessToolsClass, List<IInner>(...).
				Assert.That(TestValue(), Is.Not.InstanceOf(typeof(List<IInner>)));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, object>(
					field, TestValue(), expectedCaseToConstraint);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, IList>(
					field, TestValue(), expectedCaseToConstraint);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, string[]>(
					field, new[] { "should always throw" }, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, int>(
					field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_ClassInstance_InternalEnumFieldType()
        {
            Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsClass), "field9");
				var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
				TestSuite_Class<AccessToolsClass, AccessToolsClass, DayOfWeek>(
					field, DayOfWeek.Thursday, expectedCaseToConstraint);
				TestSuite_Struct<int, DayOfWeek>(
					field, DayOfWeek.Thursday, expectedCaseToConstraint_ClassInstance_StructT);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, object>(
					field, DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, Enum>(
					field, DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, IComparable>(
					field, DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, byte>(
					field, (byte)DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, int>(
					field, (int)DayOfWeek.Thursday, expectedCaseToConstraint);
				TestSuite_Class<AccessToolsClass, AccessToolsClass, uint>(
					field, (int)DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, int?>(
					field, (int)DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, long>(
					field, (long)DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsClass, AccessToolsClass, float>(
					field, (float)DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_SubClassInstance_PrivateString()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsSubClass), "subclassField1");
				var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
				TestSuite_Class<AccessToolsClass, AccessToolsClass, string>(
					field, "subclassField1test", IncompatibleInstanceType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, string>(
					field, "subclassField1test", expectedCaseToConstraint);
				TestSuite_Class<AccessToolsClass, AccessToolsSubClass, string>(
					field, "subclassField1test", FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<IAccessToolsType, AccessToolsSubClass, string>(
					field, "subclassField1test", FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, AccessToolsSubClass, string>(
					field, "subclassField1test", FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, AccessToolsClass, string>(
					field, "subclassField1test", IncompatibleInstanceType(expectedCaseToConstraint));
				TestSuite_Class<object, string, string>(
					field, "subclassField1test", IncompatibleInstanceType(expectedCaseToConstraint));
				TestSuite_Class<string, string, string>(
					field, "subclassField1test", IncompatibleTypeT(expectedCaseToConstraint));
				TestSuite_Struct<int, string>(
					field, "subclassField1test", expectedCaseToConstraint_ClassInstance_StructT);
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, object>(
					field, "subclassField1test", expectedCaseToConstraint);
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, IComparable>(
					field, "subclassField1test", expectedCaseToConstraint);
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, Exception>(
					field, new Exception("should always throw"), IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_SubClassStatic_InternalInt()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsSubClass), "subclassField2");
				var expectedCaseToConstraint = expectedCaseToConstraint_ClassStatic;
				// Note: As this is as static field, instance type is ignored, so IncompatibleInstanceType is never needed.
				TestSuite_Class<AccessToolsClass, AccessToolsClass, int>(
					field, 123, FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, int>(
					field, 123, expectedCaseToConstraint);
				TestSuite_Class<IAccessToolsType, AccessToolsSubClass, int>(
					field, 123, FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, AccessToolsSubClass, int>(
					field, 123, FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, AccessToolsClass, int>(
					field, 123, FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<object, string, int>(
					field, 123, FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Class<string, string, int>(
					field, 123, FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Struct<int, int>(
					field, 123, expectedCaseToConstraint_ClassStatic_StructT);
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, object>(
					field, 123, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, ValueType>(
					field, 123, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, int?>(
					field, 123, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, IComparable>(
					field, 123, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Class<AccessToolsSubClass, AccessToolsSubClass, double>(
					field, 123, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_StructInstance_PublicString()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsStruct), "structField1");
				var expectedCaseToConstraint = expectedCaseToConstraint_StructInstance;
				var expectedCaseToConstraintClassT = expectedCaseToConstraint_StructInstance_ClassT;
				TestSuite_Struct<AccessToolsStruct, string>(
					field, "structField1test", expectedCaseToConstraint);
				TestSuite_Class<IAccessToolsType, AccessToolsStruct, string>(
					field, "structField1test", expectedCaseToConstraintClassT);
				TestSuite_Class<object, AccessToolsStruct, string>(
					field, "structField1test", expectedCaseToConstraintClassT);
				TestSuite_Class<object, string, string>(
					field, "structField1test", IncompatibleInstanceType(expectedCaseToConstraintClassT));
				TestSuite_Class<string, string, string>(
					field, "structField1test", IncompatibleTypeT(expectedCaseToConstraintClassT));
				TestSuite_Struct<int, string>(
					field, "structField1test", IncompatibleTypeT(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, object>(
					field, "structField1test", expectedCaseToConstraint);
				TestSuite_Struct<AccessToolsStruct, IComparable>(
					field, "structField1test", expectedCaseToConstraint);
				TestSuite_Struct<AccessToolsStruct, List<string>>(
					field, new List<string> { "should always throw" }, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_StructInstance_PrivateReadonlyInt()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsStruct), "structField2");
				var expectedCaseToConstraint = expectedCaseToConstraint_StructInstance;
				var expectedCaseToConstraintClassT = expectedCaseToConstraint_StructInstance_ClassT;
				TestSuite_Struct<AccessToolsStruct, int>(
					field, 1234, expectedCaseToConstraint);
				TestSuite_Class<IAccessToolsType, AccessToolsStruct, int>(
					field, 1234, expectedCaseToConstraintClassT);
				TestSuite_Class<object, AccessToolsStruct, int>(
					field, 1234, expectedCaseToConstraintClassT);
				TestSuite_Class<object, string, int>(
					field, 1234, IncompatibleInstanceType(expectedCaseToConstraintClassT));
				TestSuite_Class<string, string, int>(
					field, 1234, IncompatibleTypeT(expectedCaseToConstraintClassT));
				TestSuite_Struct<int, int>(
					field, 1234, IncompatibleTypeT(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, object>(
					field, 1234, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, ValueType>(
					field, 1234, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, int?>(
					field, 1234, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, IComparable>(
					field, 1234, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, long>(
					field, 1234, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_StructStatic_PrivateInt()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsStruct), "structField3");
				var expectedCaseToConstraint = expectedCaseToConstraint_StructStatic;
				var expectedCaseToConstraintClassT = expectedCaseToConstraint_StructStatic_ClassT;
				// Note: As this is as static field, instance type is ignored, so IncompatibleInstanceType is never needed.
				TestSuite_Struct<AccessToolsStruct, int>(
					field, 4321, expectedCaseToConstraint);
				TestSuite_Class<IAccessToolsType, AccessToolsStruct, int>(
					field, 4321, expectedCaseToConstraintClassT);
				TestSuite_Class<object, AccessToolsStruct, int>(
					field, 4321, expectedCaseToConstraintClassT);
				TestSuite_Class<object, string, int>(
					field, 4321, expectedCaseToConstraintClassT);
				TestSuite_Class<string, string, int>(
					field, 4321, FieldMissingOnTypeT(expectedCaseToConstraintClassT));
				TestSuite_Struct<int, int>(
					field, 4321, FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, object>(
					field, 4321, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, ValueType>(
					field, 4321, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, int?>(
					field, 4321, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, IComparable>(
					field, 4321, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, long>(
					field, 4321, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_StructStatic_PublicReadonlyString()
		{
			Assert.Multiple(() =>
			{
				var field = AccessTools2.Field(typeof(AccessToolsStruct), "structField4");
				var expectedCaseToConstraint = expectedCaseToConstraint_StructStatic;
				var expectedCaseToConstraintClassT = expectedCaseToConstraint_StructStatic_ClassT;
				// Note: As this is as static field, instance type is ignored, so IncompatibleInstanceType is never needed.
				TestSuite_Struct<AccessToolsStruct, string>(
					field, "structField4test", expectedCaseToConstraint);
				TestSuite_Class<IAccessToolsType, AccessToolsStruct, string>(
					field, "structField4test", expectedCaseToConstraintClassT);
				TestSuite_Class<object, AccessToolsStruct, string>(
					field, "structField4test", expectedCaseToConstraintClassT);
				TestSuite_Class<object, string, string>(
					field, "structField4test", expectedCaseToConstraintClassT);
				TestSuite_Class<string, string, string>(
					field, "structField4test", FieldMissingOnTypeT(expectedCaseToConstraintClassT));
				TestSuite_Struct<int, string>(
					field, "structField4test", FieldMissingOnTypeT(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, object>(
					field, "structField4test", expectedCaseToConstraint);
				TestSuite_Struct<AccessToolsStruct, IComparable>(
					field, "structField4test", expectedCaseToConstraint);
				TestSuite_Struct<AccessToolsStruct, int>(
					field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}

		[Test]
		public void Test_StructInstance_PrivateEnumFieldType()
		{
			Assert.Multiple(() =>
			{
				// Note: AccessToolsStruct.InnerEnum is private, so can't be specified as F here.
				var field = AccessTools2.Field(typeof(AccessToolsStruct), "structField5");
				var expectedCaseToConstraint = expectedCaseToConstraint_StructInstance;
				var expectedCaseToConstraintClassT = expectedCaseToConstraint_StructInstance_ClassT;
                TestSuite_Struct<AccessToolsStruct, byte>(
                    field, 3, expectedCaseToConstraint);
                TestSuite_Class<IAccessToolsType, AccessToolsStruct, byte>(
                    field, 3, expectedCaseToConstraintClassT);
				TestSuite_Class<object, AccessToolsStruct, byte>(
                    field, 3, expectedCaseToConstraintClassT);
				TestSuite_Class<object, string, byte>(
                    field, 3, IncompatibleInstanceType(expectedCaseToConstraintClassT));
				TestSuite_Class<string, string, byte>(
                    field, 3, IncompatibleTypeT(expectedCaseToConstraintClassT));
                TestSuite_Struct<int, byte>(
                    field, 3, IncompatibleTypeT(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, object>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, object>(
                    field, AccessToolsStruct.NewInnerEnum(3), IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, Enum>(
                    field, AccessToolsStruct.NewInnerEnum(3), IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, IComparable>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, byte>(
                    field, 3, expectedCaseToConstraint);
				TestSuite_Struct<AccessToolsStruct, sbyte>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, byte?>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, int>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, long>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
				TestSuite_Struct<AccessToolsStruct, float>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
			});
		}
	}
}