using HarmonyLib;
using HarmonyLib.BUTR.Extensions;
using HarmonyLibTests.Tools.Assets;

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
                ["FieldRefAccess<T, F>(fieldName)(instance)"] = ATestCase<T, F>(instance => ref AccessTools2.FieldRefAccess<T, F>(fieldName)!(instance)),
                ["FieldRefAccess<F>(typeof(T), fieldName)(instance)"] = ATestCase<T, F>(instance => ref AccessTools2.FieldRefAccess<F>(typeof(T), fieldName)!(instance)),
                ["FieldRefAccess<F>(typeof(T), fieldName)()"] = ATestCase<T, F>(instance => ref AccessTools2.FieldRefAccess<F>(typeof(T), fieldName)!()),
            };
        }

        static Dictionary<string, IATestCase<T, F>> AvailableTestCases_FieldRefAccess_ByFieldInfo<T, F>(FieldInfo field) where T : class
        {
            return new()
            {
                ["FieldRefAccess<T, F>(field)(instance)"] = ATestCase<T, F>(instance => ref AccessTools2.FieldRefAccess<T, F>(field)!(instance)),
                ["FieldRefAccess<T, F>(field)()"] = ATestCase<T, F>(instance => ref AccessTools2.FieldRefAccess<T, F>(field)!())
            };
        }

        static Dictionary<string, IATestCase<T, F>> AvailableTestCases_StructFieldRefAccess<T, F>(FieldInfo field, string fieldName) where T : struct
        {
            return new()
            {
                ["StructFieldRefAccess<T, F>(fieldName)(ref instance)"] = ATestCase((ref T instance) => ref AccessTools2.StructFieldRefAccess<T, F>(fieldName)!(ref instance)),
                ["StructFieldRefAccess<T, F>(field)(ref instance)"] = ATestCase((ref T instance) => ref AccessTools2.StructFieldRefAccess<T, F>(field)!(ref instance)),
            };
        }

        static Dictionary<string, IATestCase<T, F>> AvailableTestCases_StaticFieldRefAccess_ByName<T, F>(string fieldName) where T : class
        {
            return new()
            {
                ["StaticFieldRefAccess<F>(typeof(T), fieldName)()"] = ATestCase<T, F>(() => ref AccessTools2.StaticFieldRefAccess<F>(typeof(T), fieldName)!()),
            };
        }

        static Dictionary<string, IATestCase<T, F>> AvailableTestCases_StaticFieldRefAccess_ByFieldInfo<T, F>(FieldInfo field) where T : class
        {
            return new()
            {
                ["StaticFieldRefAccess<F>(field)()"] = ATestCase<T, F>(() => ref AccessTools2.StaticFieldRefAccess<F>(field)!()),
            };
        }


        [Test]
        public void Test_ClassInstance_ProtectedString()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2Class), "field1");
                var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
                TestSuite_Class<AccessTools2Class, AccessTools2Class, string>(
                    field, "field1test", expectedCaseToConstraint);
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, string>(
                    field, "field1test", expectedCaseToConstraint);
                TestSuite_Class<AccessTools2Class, AccessTools2SubClass, string>(
                    field, "field1test", expectedCaseToConstraint);
                TestSuite_Class<IAccessTools2Type, AccessTools2Class, string>(
                    field, "field1test", FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, AccessTools2Class, string>(
                    field, "field1test", FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, string, string>(
                    field, "field1test", IncompatibleInstanceType(expectedCaseToConstraint));
                TestSuite_Class<string, string, string>(
                    field, "field1test", IncompatibleTypeT(expectedCaseToConstraint));
                TestSuite_Struct<int, string>(
                    field, "field1test", expectedCaseToConstraint_ClassInstance_StructT);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, object>(
                    field, "field1test", expectedCaseToConstraint);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, IComparable>(
                    field, "field1test", expectedCaseToConstraint);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, string[]>(
                    field, new[] { "should always throw" }, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, int>(
                    field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_ClassInstance_PublicReadonlyFloat()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2Class), "field2");
                var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
                TestSuite_Class<AccessTools2Class, AccessTools2Class, float>(
                    field, 314f, expectedCaseToConstraint);
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, float>(
                    field, 314f, expectedCaseToConstraint);
                TestSuite_Class<AccessTools2Class, AccessTools2SubClass, float>(
                    field, 314f, expectedCaseToConstraint);
                TestSuite_Class<IAccessTools2Type, AccessTools2Class, float>(
                    field, 314f, FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, AccessTools2Class, float>(
                    field, 314f, FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, string, float>(
                    field, 314f, IncompatibleInstanceType(expectedCaseToConstraint));
                TestSuite_Class<string, string, float>(
                    field, 314f, IncompatibleTypeT(expectedCaseToConstraint));
                TestSuite_Struct<int, float>(
                    field, 314f, expectedCaseToConstraint_ClassInstance_StructT);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, object>(
                    field, 314f, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, ValueType>(
                    field, 314f, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, float?>(
                    field, 314f, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, IComparable>(
                    field, 314f, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, double>(
                    field, 314f, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_ClassStatic_PublicLong()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2Class), "field3");
                var expectedCaseToConstraint = expectedCaseToConstraint_ClassStatic;
                // Note: As this is as static field, instance type is ignored, so IncompatibleInstanceType is never needed.
                TestSuite_Class<AccessTools2Class, AccessTools2Class, long>(
                    field, 314L, expectedCaseToConstraint);
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, long>(
                    field, 314L, expectedCaseToConstraint);
                TestSuite_Class<IAccessTools2Type, AccessTools2Class, long>(
                    field, 314L, FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, AccessTools2Class, long>(
                    field, 314L, FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, string, long>(
                    field, 314L, FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<string, string, long>(
                    field, 314L, FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Struct<int, long>(
                    field, 314L, expectedCaseToConstraint_ClassStatic_StructT);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, object>(
                    field, 314L, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, ValueType>(
                    field, 314L, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, long?>(
                    field, 314L, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, IComparable>(
                    field, 314L, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, double>(
                    field, 314L, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_ClassStatic_PrivateReadonlyString()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2Class), "field4");
                var expectedCaseToConstraint = expectedCaseToConstraint_ClassStatic;
                // Note: As this is as static field, instance type is ignored, so IncompatibleInstanceType is never needed.
                TestSuite_Class<AccessTools2Class, AccessTools2Class, string>(
                    field, "field4test", expectedCaseToConstraint);
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, string>(
                    field, "field4test", expectedCaseToConstraint);
                TestSuite_Class<IAccessTools2Type, AccessTools2Class, string>(
                    field, "field4test", FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, AccessTools2Class, string>(
                    field, "field4test", FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, string, string>(
                    field, "field4test", FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<string, string, string>(
                    field, "field4test", FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Struct<int, string>(
                    field, "field4test", expectedCaseToConstraint_ClassStatic_StructT);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, object>(
                    field, "field4test", expectedCaseToConstraint);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, IComparable>(
                    field, "field4test", expectedCaseToConstraint);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, IEnumerable<string>>(
                    field, new[] { "should always throw" }, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, int>(
                    field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }


        [Test]
        public void Test_ClassInstance_PrivateClassFieldType()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2Class), "field5");
                var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
                // Type of field is AccessToolsClass.Inner, which is a private class.
                static IInner TestValue()
                {
                    return AccessTools2Class.NewInner(987);
                }
                TestSuite_Class<AccessTools2Class, AccessTools2Class, IInner>(
                    field, TestValue(), expectedCaseToConstraint);
                TestSuite_Class<IAccessTools2Type, AccessTools2Class, IInner>(
                    field, TestValue(), FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, AccessTools2Class, IInner>(
                    field, TestValue(), FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, string, IInner>(
                    field, TestValue(), IncompatibleInstanceType(expectedCaseToConstraint));
                TestSuite_Class<string, string, IInner>(
                    field, TestValue(), IncompatibleTypeT(expectedCaseToConstraint));
                TestSuite_Struct<int, IInner>(
                    field, TestValue(), expectedCaseToConstraint_ClassInstance_StructT);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, object>(
                    field, TestValue(), expectedCaseToConstraint);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, string[]>(
                    field, new[] { "should always throw" }, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, int>(
                    field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_ClassInstance_ArrayOfPrivateClassFieldType()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2Class), "field6");
                var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
                // Type of field is AccessToolsClass.Inner[], the element type of which is a private class.
                static IList TestValue()
                {
                    // IInner[] can't be cast to AccessTools.Inner[], so must create an actual AccessTools.Inner[].
                    var array = (IList)Array.CreateInstance(AccessTools.Inner(typeof(AccessTools2Class), "Inner"), 2);
                    array[0] = AccessTools2Class.NewInner(123);
                    array[1] = AccessTools2Class.NewInner(456);
                    return array;
                }
                TestSuite_Class<AccessTools2Class, AccessTools2Class, IList>(
                    field, TestValue(), expectedCaseToConstraint);
                TestSuite_Class<IAccessTools2Type, AccessTools2Class, IList>(
                    field, TestValue(), FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, AccessTools2Class, IList>(
                    field, TestValue(), FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, string, IList>(
                    field, TestValue(), IncompatibleInstanceType(expectedCaseToConstraint));
                TestSuite_Class<string, string, IList>(
                    field, TestValue(), IncompatibleTypeT(expectedCaseToConstraint));
                TestSuite_Struct<int, IList>(
                    field, TestValue(), expectedCaseToConstraint_ClassInstance_StructT);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, IInner[]>(
                    field, (IInner[])TestValue(), expectedCaseToConstraint); // AccessTools.Inner[] can be cast to IInner[]
                TestSuite_Class<AccessTools2Class, AccessTools2Class, object>(
                    field, TestValue(), expectedCaseToConstraint);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, IList>(
                    field, TestValue(), expectedCaseToConstraint);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, string[]>(
                    field, new[] { "should always throw" }, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, int>(
                    field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_ClassInstance_PrivateStructFieldType()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2Class), "field7");
                var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
                // Type of field is AccessToolsClass.InnerStruct, which is a private struct.
                // As it's a value type and references cannot be made to boxed value type instances, FieldRefValue will never work.
                static IInner TestValue()
                {
                    return AccessTools2Class.NewInnerStruct(-987);
                }
                TestSuite_Class<AccessTools2Class, AccessTools2Class, IInner>(
                    field, TestValue(), IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<int, IInner>(
                    field, TestValue(), expectedCaseToConstraint_ClassInstance_StructT);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, object>(
                    field, TestValue(), IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, ValueType>(
                    field, (ValueType)TestValue(), IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, int>(
                    field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_ClassInstance_ListOfPrivateStructFieldType()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2Class), "field8");
                var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
                // Type of field is List<AccessToolsClass.Inner>, the element type of which is a private struct.
                // Although AccessToolsClass.Inner is a value type, List is not, so FieldRefValue works normally.
                static IList TestValue()
                {
                    // List<IInner> can't be cast to List<AccessTools.Inner>, so must create an actual List<AccessTools.Inner>.
                    var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(AccessTools.Inner(typeof(AccessTools2Class), "InnerStruct")));
                    _ = list.Add(AccessTools2Class.NewInnerStruct(-123));
                    _ = list.Add(AccessTools2Class.NewInnerStruct(-456));
                    return list;
                }
                TestSuite_Class<AccessTools2Class, AccessTools2Class, IList>(
                    field, TestValue(), expectedCaseToConstraint);
                TestSuite_Class<IAccessTools2Type, AccessTools2Class, IList>(
                    field, TestValue(), FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, AccessTools2Class, IList>(
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
                TestSuite_Class<AccessTools2Class, AccessTools2Class, object>(
                    field, TestValue(), expectedCaseToConstraint);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, IList>(
                    field, TestValue(), expectedCaseToConstraint);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, string[]>(
                    field, new[] { "should always throw" }, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, int>(
                    field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_ClassInstance_InternalEnumFieldType()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2Class), "field9");
                var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
                TestSuite_Class<AccessTools2Class, AccessTools2Class, DayOfWeek>(
                    field, DayOfWeek.Thursday, expectedCaseToConstraint);
                TestSuite_Struct<int, DayOfWeek>(
                    field, DayOfWeek.Thursday, expectedCaseToConstraint_ClassInstance_StructT);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, object>(
                    field, DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, Enum>(
                    field, DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, IComparable>(
                    field, DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, byte>(
                    field, (byte)DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, int>(
                    field, (int)DayOfWeek.Thursday, expectedCaseToConstraint);
                TestSuite_Class<AccessTools2Class, AccessTools2Class, uint>(
                    field, (int)DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, int?>(
                    field, (int)DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, long>(
                    field, (long)DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2Class, AccessTools2Class, float>(
                    field, (float)DayOfWeek.Thursday, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_SubClassInstance_PrivateString()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2SubClass), "subclassField1");
                var expectedCaseToConstraint = expectedCaseToConstraint_ClassInstance;
                TestSuite_Class<AccessTools2Class, AccessTools2Class, string>(
                    field, "subclassField1test", IncompatibleInstanceType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, string>(
                    field, "subclassField1test", expectedCaseToConstraint);
                TestSuite_Class<AccessTools2Class, AccessTools2SubClass, string>(
                    field, "subclassField1test", FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<IAccessTools2Type, AccessTools2SubClass, string>(
                    field, "subclassField1test", FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, AccessTools2SubClass, string>(
                    field, "subclassField1test", FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, AccessTools2Class, string>(
                    field, "subclassField1test", IncompatibleInstanceType(expectedCaseToConstraint));
                TestSuite_Class<object, string, string>(
                    field, "subclassField1test", IncompatibleInstanceType(expectedCaseToConstraint));
                TestSuite_Class<string, string, string>(
                    field, "subclassField1test", IncompatibleTypeT(expectedCaseToConstraint));
                TestSuite_Struct<int, string>(
                    field, "subclassField1test", expectedCaseToConstraint_ClassInstance_StructT);
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, object>(
                    field, "subclassField1test", expectedCaseToConstraint);
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, IComparable>(
                    field, "subclassField1test", expectedCaseToConstraint);
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, Exception>(
                    field, new Exception("should always throw"), IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_SubClassStatic_InternalInt()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2SubClass), "subclassField2");
                var expectedCaseToConstraint = expectedCaseToConstraint_ClassStatic;
                // Note: As this is as static field, instance type is ignored, so IncompatibleInstanceType is never needed.
                TestSuite_Class<AccessTools2Class, AccessTools2Class, int>(
                    field, 123, FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, int>(
                    field, 123, expectedCaseToConstraint);
                TestSuite_Class<IAccessTools2Type, AccessTools2SubClass, int>(
                    field, 123, FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, AccessTools2SubClass, int>(
                    field, 123, FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, AccessTools2Class, int>(
                    field, 123, FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<object, string, int>(
                    field, 123, FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Class<string, string, int>(
                    field, 123, FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Struct<int, int>(
                    field, 123, expectedCaseToConstraint_ClassStatic_StructT);
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, object>(
                    field, 123, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, ValueType>(
                    field, 123, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, int?>(
                    field, 123, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, IComparable>(
                    field, 123, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Class<AccessTools2SubClass, AccessTools2SubClass, double>(
                    field, 123, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_StructInstance_PublicString()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2Struct), "structField1");
                var expectedCaseToConstraint = expectedCaseToConstraint_StructInstance;
                var expectedCaseToConstraintClassT = expectedCaseToConstraint_StructInstance_ClassT;
                TestSuite_Struct<AccessTools2Struct, string>(
                    field, "structField1test", expectedCaseToConstraint);
                TestSuite_Class<IAccessTools2Type, AccessTools2Struct, string>(
                    field, "structField1test", expectedCaseToConstraintClassT);
                TestSuite_Class<object, AccessTools2Struct, string>(
                    field, "structField1test", expectedCaseToConstraintClassT);
                TestSuite_Class<object, string, string>(
                    field, "structField1test", IncompatibleInstanceType(expectedCaseToConstraintClassT));
                TestSuite_Class<string, string, string>(
                    field, "structField1test", IncompatibleTypeT(expectedCaseToConstraintClassT));
                TestSuite_Struct<int, string>(
                    field, "structField1test", IncompatibleTypeT(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, object>(
                    field, "structField1test", expectedCaseToConstraint);
                TestSuite_Struct<AccessTools2Struct, IComparable>(
                    field, "structField1test", expectedCaseToConstraint);
                TestSuite_Struct<AccessTools2Struct, List<string>>(
                    field, new List<string> { "should always throw" }, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_StructInstance_PrivateReadonlyInt()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2Struct), "structField2");
                var expectedCaseToConstraint = expectedCaseToConstraint_StructInstance;
                var expectedCaseToConstraintClassT = expectedCaseToConstraint_StructInstance_ClassT;
                TestSuite_Struct<AccessTools2Struct, int>(
                    field, 1234, expectedCaseToConstraint);
                TestSuite_Class<IAccessTools2Type, AccessTools2Struct, int>(
                    field, 1234, expectedCaseToConstraintClassT);
                TestSuite_Class<object, AccessTools2Struct, int>(
                    field, 1234, expectedCaseToConstraintClassT);
                TestSuite_Class<object, string, int>(
                    field, 1234, IncompatibleInstanceType(expectedCaseToConstraintClassT));
                TestSuite_Class<string, string, int>(
                    field, 1234, IncompatibleTypeT(expectedCaseToConstraintClassT));
                TestSuite_Struct<int, int>(
                    field, 1234, IncompatibleTypeT(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, object>(
                    field, 1234, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, ValueType>(
                    field, 1234, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, int?>(
                    field, 1234, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, IComparable>(
                    field, 1234, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, long>(
                    field, 1234, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_StructStatic_PrivateInt()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2Struct), "structField3");
                var expectedCaseToConstraint = expectedCaseToConstraint_StructStatic;
                var expectedCaseToConstraintClassT = expectedCaseToConstraint_StructStatic_ClassT;
                // Note: As this is as static field, instance type is ignored, so IncompatibleInstanceType is never needed.
                TestSuite_Struct<AccessTools2Struct, int>(
                    field, 4321, expectedCaseToConstraint);
                TestSuite_Class<IAccessTools2Type, AccessTools2Struct, int>(
                    field, 4321, expectedCaseToConstraintClassT);
                TestSuite_Class<object, AccessTools2Struct, int>(
                    field, 4321, expectedCaseToConstraintClassT);
                TestSuite_Class<object, string, int>(
                    field, 4321, expectedCaseToConstraintClassT);
                TestSuite_Class<string, string, int>(
                    field, 4321, FieldMissingOnTypeT(expectedCaseToConstraintClassT));
                TestSuite_Struct<int, int>(
                    field, 4321, FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, object>(
                    field, 4321, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, ValueType>(
                    field, 4321, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, int?>(
                    field, 4321, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, IComparable>(
                    field, 4321, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, long>(
                    field, 4321, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_StructStatic_PublicReadonlyString()
        {
            Assert.Multiple(() =>
            {
                var field = AccessTools2.Field(typeof(AccessTools2Struct), "structField4");
                var expectedCaseToConstraint = expectedCaseToConstraint_StructStatic;
                var expectedCaseToConstraintClassT = expectedCaseToConstraint_StructStatic_ClassT;
                // Note: As this is as static field, instance type is ignored, so IncompatibleInstanceType is never needed.
                TestSuite_Struct<AccessTools2Struct, string>(
                    field, "structField4test", expectedCaseToConstraint);
                TestSuite_Class<IAccessTools2Type, AccessTools2Struct, string>(
                    field, "structField4test", expectedCaseToConstraintClassT);
                TestSuite_Class<object, AccessTools2Struct, string>(
                    field, "structField4test", expectedCaseToConstraintClassT);
                TestSuite_Class<object, string, string>(
                    field, "structField4test", expectedCaseToConstraintClassT);
                TestSuite_Class<string, string, string>(
                    field, "structField4test", FieldMissingOnTypeT(expectedCaseToConstraintClassT));
                TestSuite_Struct<int, string>(
                    field, "structField4test", FieldMissingOnTypeT(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, object>(
                    field, "structField4test", expectedCaseToConstraint);
                TestSuite_Struct<AccessTools2Struct, IComparable>(
                    field, "structField4test", expectedCaseToConstraint);
                TestSuite_Struct<AccessTools2Struct, int>(
                    field, 1337, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }

        [Test]
        public void Test_StructInstance_PrivateEnumFieldType()
        {
            Assert.Multiple(() =>
            {
                // Note: AccessToolsStruct.InnerEnum is private, so can't be specified as F here.
                var field = AccessTools2.Field(typeof(AccessTools2Struct), "structField5");
                var expectedCaseToConstraint = expectedCaseToConstraint_StructInstance;
                var expectedCaseToConstraintClassT = expectedCaseToConstraint_StructInstance_ClassT;
                TestSuite_Struct<AccessTools2Struct, byte>(
                    field, 3, expectedCaseToConstraint);
                TestSuite_Class<IAccessTools2Type, AccessTools2Struct, byte>(
                    field, 3, expectedCaseToConstraintClassT);
                TestSuite_Class<object, AccessTools2Struct, byte>(
                    field, 3, expectedCaseToConstraintClassT);
                TestSuite_Class<object, string, byte>(
                    field, 3, IncompatibleInstanceType(expectedCaseToConstraintClassT));
                TestSuite_Class<string, string, byte>(
                    field, 3, IncompatibleTypeT(expectedCaseToConstraintClassT));
                TestSuite_Struct<int, byte>(
                    field, 3, IncompatibleTypeT(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, object>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, object>(
                    field, AccessTools2Struct.NewInnerEnum(3), IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, Enum>(
                    field, AccessTools2Struct.NewInnerEnum(3), IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, IComparable>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, byte>(
                    field, 3, expectedCaseToConstraint);
                TestSuite_Struct<AccessTools2Struct, sbyte>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, byte?>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, int>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, long>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
                TestSuite_Struct<AccessTools2Struct, float>(
                    field, 3, IncompatibleFieldType(expectedCaseToConstraint));
            });
        }
    }
}