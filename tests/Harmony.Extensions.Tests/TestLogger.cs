using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

using System;
using System.Diagnostics;
using System.Linq;

namespace HarmonyLibTests
{
    public class TestLogger
    {
        private class NUnitShimListener : TraceListener
        {
            public override void Write(string message) => Console.Write(message);
            public override void WriteLine(string message) => Console.WriteLine(message);
        }

        private static readonly NUnitShimListener _listener = new();

        class ExplicitException : ResultStateException
        {
            public ExplicitException(string message) : base(message) { }

            public override ResultState ResultState => ResultState.Explicit;
        }

        [SetUp]
        public void BaseSetUp()
        {
            Trace.Listeners.Add(_listener);
            TestTools.Log($"### {TestExecutionContext.CurrentContext.CurrentResult.FullName}", indentLevel: 0);

            SkipExplicitTestIfVSTest();
        }

        // Workaround for [Explicit] attribute sometimes not working in the NUnit3 VS Test Adapter, which applies to both Visual Studio and
        // vstest.console (bug: https://github.com/nunit/nunit3-vs-adapter/issues/658). It does apparently work with `dotnet test` as long
        // as the test dll isn't specified (which delegates to vstest.console).
        // So always skip [Explicit] tests when NUnit3 VS Test Adapter is used - the [Explicit] attribute needs to be commented out to run the test.
        static void SkipExplicitTestIfVSTest()
        {
            var test = TestExecutionContext.CurrentContext.CurrentTest;
            if (test.Method?.IsDefined<ExplicitAttribute>(true) ?? test.TypeInfo?.IsDefined<ExplicitAttribute>(true) ?? false)
            {
                // Due to the way the NUnit3 VS Test Adapter creates separate AppDomains for tests and the difficulty with getting process
                // command line arguments in a cross-platform way, there's no direct way to determine whether the adapter is being used.
                // Indirect ways to determine whether the adapter is used in various ways:
                // 1) process name starts with "testhost" (e.g. testhost.x86)
                // 2) process name starts with "vstest" (e.g. vstest.console)
                var process = Process.GetCurrentProcess();
                if (process.ProcessName.StartsWith("testhost") || process.ProcessName.StartsWith("vstest"))
                    throw GetExplicitException(test);
                // 3) process modules include a *VisualStudio* dll
                // This case is needed when run under mono, since process name is just "mono(.exe)" then.
                if (process.Modules.Cast<ProcessModule>().Any(module => module.ModuleName.StartsWith("Microsoft.VisualStudio")))
                    throw GetExplicitException(test);
            }
        }

        static ExplicitException GetExplicitException(Test test)
        {
            // This is the least fragile way to get the explicit reason message.
            var explicitAttribute = test.GetCustomAttributes<ExplicitAttribute>(true).First();
            explicitAttribute.ApplyToTest(test);
            return new ExplicitException((string)test.Properties.Get(PropertyNames.SkipReason) ?? "");
        }

        [TearDown]
        public void BaseTearDown()
        {
            Trace.Listeners.Remove(_listener);
            var result = TestExecutionContext.CurrentContext.CurrentResult;
            TestTools.Log($"--- {result.FullName} => {result.ResultState}", indentLevel: 0);
        }
    }
}