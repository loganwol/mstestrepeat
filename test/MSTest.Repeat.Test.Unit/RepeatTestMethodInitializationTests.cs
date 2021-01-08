namespace MSTest.RepeatAttributes.Test.Unit
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;

    /// <summary>
    /// Defines the integration tests to test Repeat Test.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RepeatTestMethodInitializationTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatTestMethodInitializationTests"/> class.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            Environment.SetEnvironmentVariable("NumberofIterations", string.Empty, EnvironmentVariableTarget.User);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="RepeatTestMethodInitializationTests"/> class.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            Environment.SetEnvironmentVariable("NumberofIterations", string.Empty, EnvironmentVariableTarget.User);
        }

        /// <summary>
        /// Test that the iteration count can be read from Environment variable.
        /// </summary>
        [TestMethod]
        public void Iteration_count_can_be_Read_from_environment_variable_successfully()
        {
            // Arrange
            Environment.SetEnvironmentVariable("NumberofIterations", "200", EnvironmentVariableTarget.User);

            // Act
            RepeatTestMethodAttribute repeatTestMethod = new RepeatTestMethodAttribute();

            // Verify
            repeatTestMethod.Iterations.Should().Be(200);
        }

        /// <summary>
        /// Test the iteration count is default if there is no enviornment variable set.
        /// </summary>
        /// <param name="iterationvalue">The value of the iteration to test with.</param>
        [TestMethod]
        [DataRow("0", DisplayName = "Iteration count set to zero")]
        [DataRow("-1", DisplayName = "Iteration count set to negative value")]
        [DataRow("1000001", DisplayName = "Iteration count set to large value")]
        [DataRow("", DisplayName = "Iteration count set to empty value")]
        public void Iteration_count_is_default_if_enviornment_variable_is_not_set(string iterationvalue)
        {
            // Arrange
            Environment.SetEnvironmentVariable("NumberofIterations", iterationvalue, EnvironmentVariableTarget.User);

            RepeatTestMethodAttribute repeatTestMethod = new RepeatTestMethodAttribute();

            repeatTestMethod.Iterations.Should().Be(1);
        }
    }
}