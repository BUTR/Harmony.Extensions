using NUnit.Framework;
using NUnit.Framework.Constraints;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLibTests.Tools
{
    public partial class TestRefAccess
    {
        static void TestSuite_Class<T, I, F>(FieldInfo field, F testValue, Dictionary<string, ReusableConstraint> expectedCaseToConstraint) where T : class
        {
            TestTools.AssertImmediate(() => Assert.NotNull(field));
            var availableTestCases = Merge(
                AvailableTestCases_FieldRefAccess_ByName<T, F>(field.Name)
                , AvailableTestCases_FieldRefAccess_ByFieldInfo<T, F>(field)
                , AvailableTestCases_StaticFieldRefAccess_ByName<T, F>(field.Name)
                , AvailableTestCases_StaticFieldRefAccess_ByFieldInfo<T, F>(field)
            );
            new ATestSuite<T, F>(typeof(I), field, testValue, expectedCaseToConstraint, availableTestCases).Run();
        }

        static void TestSuite_Struct<T, F>(FieldInfo field, F testValue, Dictionary<string, ReusableConstraint> expectedCaseToConstraint) where T : struct
        {
            TestTools.AssertImmediate(() => Assert.NotNull(field));
            var availableTestCases = Merge(
                AvailableTestCases_StructFieldRefAccess<T, F>(field, field.Name)
            );
            new ATestSuite<T, F>(typeof(T), field, testValue, expectedCaseToConstraint, availableTestCases).Run();
        }

        // NUnit limitation: the same constraint can't be used multiple times.
        // Workaround is to wrap each constraint in a ReusableConstraint as needed.
        static Dictionary<string, ReusableConstraint> ReusableConstraints(Dictionary<string, IResolveConstraint> expectedCaseToConstraint)
        {
            var newExpectedCaseToConstraint = new Dictionary<string, ReusableConstraint>();
            foreach (var pair in expectedCaseToConstraint)
            {
                var testCaseName = pair.Key;
                var expectedConstraint = pair.Value;
                newExpectedCaseToConstraint.Add(testCaseName,
                    expectedConstraint as ReusableConstraint ?? new ReusableConstraint(expectedConstraint));
            }
            return newExpectedCaseToConstraint;
        }

        static Dictionary<string, ReusableConstraint> FieldMissingOnTypeT(Dictionary<string, ReusableConstraint> expectedCaseToConstraint)
        {
            return expectedCaseToConstraint.Merge(ReusableConstraints(new Dictionary<string, IResolveConstraint>
            {
                ["StaticFieldRefAccess<F>(typeof(T), fieldName)()"] = Throws.TypeOf<NullReferenceException>(),
                ["StructFieldRefAccess<T, F>(fieldName)(ref instance)"] = Throws.TypeOf<NullReferenceException>(),
                ["FieldRefAccess<F>(typeof(T), fieldName)(instance)"] = Throws.TypeOf<NullReferenceException>(),
                ["FieldRefAccess<F>(typeof(T), fieldName)()"] = Throws.TypeOf<NullReferenceException>(),
                ["FieldRefAccess<T, F>(fieldName)(instance)"] = Throws.TypeOf<NullReferenceException>(),
            }).Where(pair => expectedCaseToConstraint.ContainsKey(pair.Key)));
        }

        static Dictionary<string, ReusableConstraint> IncompatibleInstanceType(Dictionary<string, ReusableConstraint> expectedCaseToConstraint)
        {
            // Given that type T must be assignable from instance type, and that instance type is incompatible with field's declaring type,
            // assume that the field cannot be found on type T.
            var newExpectedCaseToConstraint = FieldMissingOnTypeT(expectedCaseToConstraint).Merge(ReusableConstraints(new Dictionary<string, IResolveConstraint>
            {
                ["StructFieldRefAccess<T, F>(field)(ref instance)"] = Throws.TypeOf<NullReferenceException>(),
            }).Where(pair => expectedCaseToConstraint.ContainsKey(pair.Key)));
            // Only override Throws.Nothing constraint with InvalidCastException for these test cases,
            // since other Throws constraints should have precedence over InvalidCastException:
            // - Null is thrown from FieldRefAccess
            // - Null is thrown when invoking FieldRefAccess-returned delegate with null instance
            // - InvalidCastException is only thrown when invoking FieldRefAccess-returned delegate with an instance of incompatible type
            return newExpectedCaseToConstraint.Merge(ReusableConstraints(new Dictionary<string, IResolveConstraint>
            {
                ["FieldRefAccess<T, F>(field)(instance)"] = Throws.TypeOf<InvalidCastException>(),
            }).Where(pair => expectedCaseToConstraint.TryGetValue(pair.Key, out var constraint) && constraint.Resolve() is ThrowsNothingConstraint));
        }

        static Dictionary<string, ReusableConstraint> IncompatibleTypeT(Dictionary<string, ReusableConstraint> expectedCaseToConstraint)
        {
            // Given that type T is incompatible with field's declaring type, and instance type must be assignable to type T,
            // instance type must also be incompatible with field's declaring type.
            // Also assume that the field cannot be found on type T (already assumed in IncompatibleInstanceType).
            return IncompatibleInstanceType(expectedCaseToConstraint).Merge(ReusableConstraints(new Dictionary<string, IResolveConstraint>
            {
                ["FieldRefAccess<T, F>(field)(instance)"] = Throws.TypeOf<NullReferenceException>(),
                ["FieldRefAccess<T, F>(field)()"] = Throws.TypeOf<NullReferenceException>(),
            }).Where(pair => expectedCaseToConstraint.ContainsKey(pair.Key)));
        }

        static Dictionary<string, ReusableConstraint> IncompatibleFieldType(Dictionary<string, ReusableConstraint> expectedCaseToConstraint)
        {
            var newExpectedCaseToConstraint = new Dictionary<string, ReusableConstraint>(expectedCaseToConstraint);
            foreach (var pair in expectedCaseToConstraint)
            {
                var testCaseName = pair.Key;
                var expectedConstraint = pair.Value.Resolve();
                if (expectedConstraint is SkipTestConstraint)
                    continue;
                var expectedExceptionType = TestTools.ThrowsConstraintExceptionType(expectedConstraint);
                if (expectedExceptionType is null || expectedExceptionType == typeof(NullReferenceException))
                    newExpectedCaseToConstraint[testCaseName] = new ReusableConstraint(Throws.TypeOf<NullReferenceException>());
            }
            return newExpectedCaseToConstraint;
        }


        static readonly Dictionary<string, ReusableConstraint> expectedCaseToConstraint_ClassInstance =
            ReusableConstraints(new Dictionary<string, IResolveConstraint>
            {
                ["FieldRefAccess<T, F>(fieldName)(instance)"] = Throws.Nothing,
                ["FieldRefAccess<F>(typeof(T), fieldName)(instance)"] = Throws.Nothing,
                ["FieldRefAccess<F>(typeof(T), fieldName)()"] = Throws.TypeOf<NullReferenceException>(),
                ["FieldRefAccess<T, F>(field)(instance)"] = Throws.Nothing,
                ["FieldRefAccess<T, F>(field)()"] = Throws.TypeOf<NullReferenceException>(),
                ["StaticFieldRefAccess<F>(typeof(T), fieldName)()"] = Throws.TypeOf<NullReferenceException>(),
                ["StaticFieldRefAccess<F>(field)()"] = Throws.TypeOf<NullReferenceException>(),
            });

        static readonly Dictionary<string, ReusableConstraint> expectedCaseToConstraint_ClassStatic =
            ReusableConstraints(new Dictionary<string, IResolveConstraint>
            {
                ["FieldRefAccess<F>(typeof(T), fieldName)(instance)"] = Throws.TypeOf<NullReferenceException>(),
                ["FieldRefAccess<F>(typeof(T), fieldName)()"] = Throws.TypeOf<NullReferenceException>(),
                ["FieldRefAccess<T, F>(fieldName)(instance)"] = Throws.TypeOf<NullReferenceException>(),
                ["FieldRefAccess<T, F>(field)(instance)"] = Throws.TypeOf<NullReferenceException>(),
                ["FieldRefAccess<T, F>(field)()"] = Throws.TypeOf<NullReferenceException>(),
                ["StaticFieldRefAccess<F>(field)()"] = Throws.Nothing,
                ["StaticFieldRefAccess<F>(typeof(T), fieldName)()"] = Throws.Nothing,
            });

        static readonly Dictionary<string, ReusableConstraint> expectedCaseToConstraint_StructInstance =
            ReusableConstraints(new Dictionary<string, IResolveConstraint>
            {
                ["StructFieldRefAccess<T, F>(fieldName)(ref instance)"] = Throws.Nothing,
                ["StructFieldRefAccess<T, F>(field)(ref instance)"] = Throws.Nothing,
            });

        static readonly Dictionary<string, ReusableConstraint> expectedCaseToConstraint_StructStatic =
            ReusableConstraints(new Dictionary<string, IResolveConstraint>
            {
                ["StructFieldRefAccess<T, F>(fieldName)(ref instance)"] = Throws.TypeOf<NullReferenceException>(),
                ["StructFieldRefAccess<T, F>(field)(ref instance)"] = Throws.TypeOf<NullReferenceException>(),
            });

        // AccessToolsClass/AccessToolsSubClass are incompatible with all value types (including structs), so using IncompatibleTypeT here.
        static readonly Dictionary<string, ReusableConstraint> expectedCaseToConstraint_ClassInstance_StructT =
            IncompatibleTypeT(expectedCaseToConstraint_StructInstance);

        static readonly Dictionary<string, ReusableConstraint> expectedCaseToConstraint_ClassStatic_StructT =
            FieldMissingOnTypeT(expectedCaseToConstraint_StructStatic);

        // AccessToolsStruct is compatible with object/ValueType/IAccessToolsType reference types (classes/interfaces), so NOT using IncompatibleTypeT here.
        static readonly Dictionary<string, ReusableConstraint> expectedCaseToConstraint_StructInstance_ClassT =
            FieldMissingOnTypeT(expectedCaseToConstraint_ClassInstance).Merge(ReusableConstraints(new Dictionary<string, IResolveConstraint>
            {
                ["FieldRefAccess<T, F>(field)(instance)"] = Throws.TypeOf<NullReferenceException>(),
                ["FieldRefAccess<T, F>(field)()"] = Throws.TypeOf<NullReferenceException>(),
            }));

        static readonly Dictionary<string, ReusableConstraint> expectedCaseToConstraint_StructStatic_ClassT =
            FieldMissingOnTypeT(expectedCaseToConstraint_ClassStatic);
    }
}