// <copyright file="RepeatTestMethodAttributeTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MSTest.RepeatAttributes.Test.Unit
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;

    /// <summary>
    /// Test the RepeatAttribute in action.
    /// </summary>
    [TestClass]
    public class RepeatTestMethodAttributeTests
    {
        /// <summary>
        /// Finalizes an instance of the <see cref="RepeatTestMethodAttributeTests"/> class.
        /// </summary>
        ~RepeatTestMethodAttributeTests()
        {
            Environment.SetEnvironmentVariable("NumberofIterations", string.Empty);
        }

        /// <summary>
        /// Set up the environment variable before tests start.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            Environment.SetEnvironmentVariable("NumberofIterations", string.Empty, EnvironmentVariableTarget.User);
        }

        /// <summary>
        /// Test if the displayname for a test method that executes only once
        /// is not changed.
        /// </summary>
        [TestMethod]
        public void DisplayName_for_test_result_is_not_modified_if_run_count_is_1()
        {
            // Arrange
            TestResult testResult = new TestResult
            {
                Outcome = UnitTestOutcome.Passed,
            };

            RepeatTestMethodAttribute repeat = new RepeatTestMethodAttribute();
            var testMethodFake = Substitute.For<ITestMethod>();
            testMethodFake.Invoke(null).Returns(testResult);

            TestResult[] results = repeat.Execute(testMethodFake);

            results.Should().HaveCount(1);
            results[0].DisplayName.Should().BeNull();
        }

        [TestMethod]
        public void Read_environment_variable_and_run_iterations_specified()
        {
            // Arrange
            Environment.SetEnvironmentVariable("NumberofIterations", "10", EnvironmentVariableTarget.User);

            TestResult testResult = new TestResult
            {
                Outcome = UnitTestOutcome.Passed,
            };

            RepeatTestMethodAttribute repeat = new RepeatTestMethodAttribute();
            var testMethodFake = Substitute.For<ITestMethod>();
            testMethodFake.Invoke(null).Returns(testResult);

            // Act
            TestResult[] results = repeat.Execute(testMethodFake);

            // Verify
            results.Should().HaveCount(10);
            results[0].DisplayName.Should().Be("(iteration: 1)");
            results[9].DisplayName.Should().Be("(iteration: 10)");
        }

        [TestMethod]
        public void Use_default_iteration_when_negative_number_is_specified()
        {
            // Arrange
            Environment.SetEnvironmentVariable("NumberofIterations", "-1", EnvironmentVariableTarget.User);

            TestResult testResult = new TestResult
            {
                Outcome = UnitTestOutcome.Passed,
            };

            RepeatTestMethodAttribute repeat = new RepeatTestMethodAttribute();
            var testMethodFake = Substitute.For<ITestMethod>();
            testMethodFake.Invoke(null).Returns(testResult);

            // Act
            TestResult[] results = repeat.Execute(testMethodFake);

            // Verify
            results.Should().HaveCount(1);
        }
    }
}
