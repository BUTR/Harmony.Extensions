using HarmonyLib.BUTR.Extensions;
using HarmonyLibTests.Tools.Assets;

using NUnit.Framework;
using NUnit.Framework.Constraints;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace HarmonyLibTests.Tools
{
    // This is a comprehensive set of tests for AccessTools2.*FieldRefAccess methods.
    // Fields of test asset types are each subjected to suites of compatible test cases, where each test case follows the form of:
    // - Assert that `AccessTools2.*FieldRefAccess...` equals original value for field (or throws expected exception)
    // - Assert that `AccessTools2.*FieldRefAccess... = testValue` correctly sets value for field (or throws expected exception)
    // A particular field is subject to multiple suites of test cases, varying on the exact T and F type parameters and the instance type used.
    // The "compatibility" of a test case to a field depends on:
    // - the type that declares the field
    // - type parameter T (which may not match previous)
    // - the type of the field
    // - type parameter F (which again may not match previous)
    // particularly around differences between references types (classes and interfaces) and value types (structs, primitives, etc.).
    [TestFixture]
    public partial class TestRefAccess : TestLogger
    {
        // The "A" here is to distinguish from NUnit's own TestCase, though the "ATestCase" naming is a neat side effect.
        interface IATestCase<T, F>
        {
            F Get(ref T instance);
            void Set(ref T instance, F value);
        }

        // Marker constraint that ATestSuite uses to skip tests that can crash.
        static SkipTestConstraint SkipTest(string reason) => new(reason);

        class SkipTestConstraint : Constraint
        {
            public SkipTestConstraint(string reason) : base(reason) { }

            public override ConstraintResult ApplyTo<TActual>(TActual actual) => throw new InvalidOperationException(ToString());
        }

        // As a final check during a test case, ATestSuite checks that field.FieldType.IsInstanceOfType(field.GetValue(instance)),
        // and throws this specific exception if that check fails.
#pragma warning disable CA1032 // Implement standard exception constructors
        class IncompatibleFieldTypeException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
        {
            public IncompatibleFieldTypeException(string message) : base(message) { }
        }

        static readonly Dictionary<Type, object> instancePrototypes = new()
        {
            [typeof(AccessToolsClass)] = new AccessToolsClass(),
            [typeof(AccessToolsSubClass)] = new AccessToolsSubClass(),
            [typeof(AccessToolsStruct)] = new AccessToolsStruct(null),
            [typeof(string)] = "a string instance", // sample "invalid" class instance
            [typeof(int)] = -123, // sample "invalid" struct instance
        };

        static T CloneInstancePrototype<T>(Type instanceType)
        {
            var instance = instancePrototypes[instanceType];
            if (instance is ICloneable cloneable)
                return (T) cloneable.Clone();
            return (T) AccessTools2.Method(instance.GetType(), "MemberwiseClone")?.Invoke(instance, new object[0]);
        }

        // Like ATestCase naming above, the "A" here is to distinguish from NUnit's own TestSuite.
        class ATestSuite<T, F>
        {
            readonly Type instanceType; // must be T or subclass/implementation of T
            readonly FieldInfo field;
            readonly F testValue;
            readonly Dictionary<string, ReusableConstraint> expectedCaseToConstraint;
            readonly Dictionary<string, IATestCase<T, F>> availableTestCases;

            public ATestSuite(Type instanceType, FieldInfo field, F testValue,
                Dictionary<string, ReusableConstraint> expectedCaseToConstraint,
                Dictionary<string, IATestCase<T, F>> availableTestCases)
            {
                TestTools.AssertImmediate(() =>
                {
                    Assert.That(expectedCaseToConstraint.Keys, Is.EquivalentTo(availableTestCases.Keys),
                        "expectedCaseToConstraint and availableTestCases must have same test cases");
                    Assert.That(instancePrototypes, Contains.Key(instanceType));
                    Assert.IsTrue(typeof(T).IsAssignableFrom(instanceType), "{0} must be assignable from {1}", typeof(T), instanceType);
                });
                this.instanceType = instanceType;
                this.field = field;
                this.testValue = testValue;
                this.expectedCaseToConstraint = expectedCaseToConstraint;
                this.availableTestCases = availableTestCases;
            }

            public void Run()
            {
                var testSuiteLabel = $"field={field.Name}, T={typeof(T).Name}, I={instanceType.Name}, F={typeof(F).Name}";
                TestTools.Log(testSuiteLabel + ":", indentLevel: 0);
                Assert.Multiple(() =>
                {
                    foreach (var pair in availableTestCases)
                        Run(testSuiteLabel, pair.Key, pair.Value, expectedCaseToConstraint[pair.Key]);
                });
            }

            static object GetOrigValue(FieldInfo field)
            {
                // Not using cloned instance of given instance type since it may be (intentionally) incompatible with the field's declaring type.
                // Also not casting to F to avoid potential invalid cast exceptions (and to see how test cases handle incompatible types).
                return field.GetValue(instancePrototypes[field.DeclaringType]);
            }

            void Run(string testSuiteLabel, string testCaseName, IATestCase<T, F> testCase, ReusableConstraint expectedConstraint)
            {
                TestTools.Log(testCaseName + ":", writeLine: false);
                var testCaseLabel = $"{testSuiteLabel}, testCase={testCaseName}";

                var resolvedConstraint = expectedConstraint.Resolve();
                if (resolvedConstraint is SkipTestConstraint)
                {
                    TestTools.Log(resolvedConstraint);
                    return;
                }

                var instance = field.IsStatic ? default : CloneInstancePrototype<T>(instanceType);
                var origValue = GetOrigValue(field);
                var expectedExceptionType = TestTools.ThrowsConstraintExceptionType(resolvedConstraint);

                ConstraintResult constraintResult;
                if (expectedExceptionType is null || expectedExceptionType == typeof(IncompatibleFieldTypeException))
                {
                    constraintResult = TestTools.AssertThat(() =>
                    {
                        Assert.AreNotEqual(origValue, testValue, "{0}: expected !Equals(origValue, testValue) (indicates static field didn't get reset properly)", testCaseLabel);
                        var value = testCase.Get(ref instance);
                        // The ?.ToString() is a trick to ensure that value is fully evaluated from the ref value.
                        _ = value?.ToString();
                        Assert.AreEqual(TryConvert(origValue), value, "{0}: expected Equals(origValue, value)", testCaseLabel);
                        testCase.Set(ref instance, testValue);
                        var newValue = field.GetValue(instance);
                        Assert.AreEqual(testValue, TryConvert(newValue), "{0}: expected Equals(testValue, field.GetValue(instance))", testCaseLabel);
                        TestTools.Log($"{field.Name}: {origValue} => {testCase.Get(ref instance)}");
                        testCase.Set(ref instance, value); // reset field value
                        if (field.FieldType.IsInstanceOfType(newValue) is false)
                            throw new IncompatibleFieldTypeException($"expected field.GetValue(instance) is {field.FieldType.Name} " +
                                "(runtime sometimes allows setting fields to values of incompatible types without any above checks failing/throwing)");
                    }, expectedConstraint, testCaseLabel);
                }
                else
                {
                    constraintResult = TestTools.AssertThat(() =>
                    {
                        var value = testCase.Get(ref instance);
                        // The ?.ToString() is a trick to ensure that value is fully evaluated from the ref value.
                        _ = value?.ToString();
                        testCase.Set(ref instance, value);
                    }, expectedConstraint, testCaseLabel);
                }

                if (expectedExceptionType is null)
                {
                    if (constraintResult.ActualValue is Exception ex)
                        TestTools.Log($"UNEXPECTED {ExceptionToString(ex)} (expected no exception)\n{ex.StackTrace}");
                }
                else
                {
                    if (constraintResult.ActualValue is Exception ex)
                    {
                        if (constraintResult.IsSuccess)
                            TestTools.Log($"expected {ExceptionToString(ex)} (expected {resolvedConstraint})");
                        else
                            TestTools.Log($"UNEXPECTED {ExceptionToString(ex)} (expected {resolvedConstraint})\n{ex.StackTrace}");
                    }
                    else
                        TestTools.Log($"UNEXPECTED no exception (expected {resolvedConstraint})");
                }
            }

            static string ExceptionToString(Exception ex)
            {
                var message = $"{ex.GetType()}: {ex.Message}";
                if (ex.InnerException is { } innerException)
                    message += $" [{ExceptionToString(innerException)}]";
                return message;
            }

            // Try "casting" to F, but don't throw exception if it fails.
            // We can't use the `as` operator here since that does not work for numeric/enum conversions.
            static object TryConvert(object x)
            {
                try
                {
                    return (F)x;
                }
                catch
                {
                    return x;
                }
            }
        }

        // This helps avoid ambiguous reference between 'HarmonyLib.CollectionExtensions' and 'System.Collections.Generic.CollectionExtensions'.
        static Dictionary<K, V> Merge<K, V>(Dictionary<K, V> firstDict, params Dictionary<K, V>[] otherDicts) => firstDict.Merge(otherDicts);
    }
}