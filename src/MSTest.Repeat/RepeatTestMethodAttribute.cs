/// <summary>
/// Custom attribute defintion to help with running tests in a loop.
/// </summary>
/// <seealso cref="https://github.com/MarcolinoPT/Xunit.Repeat"/>
namespace MSTest.RepeatAttributes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Validation;

    /// <summary>
    /// Defines how MSTest tests can run in a loop.
    /// </summary>
    public class RepeatTestMethodAttribute : TestMethodAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatTestMethodAttribute"/> class.
        /// The class looks for the NumberofIterations environment variable and if it finds
        /// it uses the variable for the loop execution.
        /// The default variation count is 1.
        /// Use this method of initialization when wanting to control the execution iterations
        /// outside of the code.
        /// </summary>
        public RepeatTestMethodAttribute()
        {
            string numberofiterations = Environment.GetEnvironmentVariable("NumberofIterations", EnvironmentVariableTarget.User);

            uint iterations;
            if (string.IsNullOrEmpty(numberofiterations) || !uint.TryParse(numberofiterations, out iterations))
            {
                this.Iterations = 1;
            }
            else
            {
                this.Iterations = iterations;
            }

            if (this.Iterations == 0 || this.Iterations > 10000)
            {
                this.Iterations = 1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatTestMethodAttribute"/> class.
        /// Use this method of initialization when wanting to control the exact number of iterations.
        /// </summary>
        /// <param name="numberofiterations">Number of iterations the test should be run as.</param>
        public RepeatTestMethodAttribute(uint numberofiterations)
        {
            this.Iterations = numberofiterations;
        }

        /// <summary>
        /// Gets the current value indicating the iteration that's is executing
        /// in the repeat loop.
        /// </summary>
        public static int CurrentIteration { get; internal set; }

        /// <summary>
        /// Gets the number of Iterations the test should run.
        /// </summary>
        public uint Iterations { get; internal set; } = 1;

        /// <summary>
        /// The overriden execution method that does the execution in a loop.
        /// </summary>
        /// <param name="testMethod">Reference to the test method to execute.</param>
        /// <returns>An array of Test Results.</returns>
        public override TestResult[] Execute(ITestMethod testMethod)
        {
            Requires.NotNull(testMethod, nameof(testMethod));

#if NET472
            Trace.Listeners.Add(new ConsoleTraceListener());

            Trace.WriteLine($"Executing test {testMethod.TestMethodName} for {this.Iterations} iterations.");
#endif
            List<TestResult> testresults = new List<TestResult>();
            foreach (int iteration in Enumerable.Range(0, (int)this.Iterations))
            {
                CurrentIteration = iteration + 1;
                TestResult[] temptestresults = this.Invoke(testMethod);
                foreach (var temptestresult in temptestresults)
                {
                    if (temptestresult == null)
                    {
                        continue;
                    }

#if NET472
                    Trace.WriteLine($"Executing test {testMethod.TestMethodName} in iteration: {CurrentIteration}.");
#endif
                    if (this.Iterations > 1)
                    {
                        temptestresult.DisplayName = $"{testMethod.TestMethodName}(iteration: {CurrentIteration})";
                        temptestresult.ExecutionId = Guid.NewGuid();
                    }

                    testresults.Add(FastDeepCloner.DeepCloner.Clone(temptestresult));
                }
            }

            return testresults.ToArray();
        }

        private TestResult[] Invoke(ITestMethod testmethod)
        {
            return new[] { testmethod.Invoke(null) };
        }
    }
}
