// <copyright file="MSTestRepeatTestMethodAttributeIntegrationTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MSTest.RepeatAttributes.Test.Integration
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines the test to test the RepeatTestMethod attribute
    /// works as defined.
    /// Please note, the test methods are named ending with 1 & 2
    /// to be able to run the tests in the specific order I want
    /// to as Test 2 is monitoring the results of Test 1.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MSTestRepeatTestMethodAttributeIntegrationTest
    {
        private static int counter = 0;
        private static int targetrepeatcount = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSTestRepeatTestMethodAttributeIntegrationTest"/> class.
        /// </summary>
        /// <param name="context">The test context of the current unit test execution.</param>
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _ = context;

            string numiterations = Environment.GetEnvironmentVariable("NumberofIterations", EnvironmentVariableTarget.User);
            if (int.TryParse(numiterations, out int repeatcount))
            {
                targetrepeatcount = repeatcount;
            }

            if (File.Exists("runcounter.log"))
            {
                File.Delete("runcounter.log");
            }
        }

        /// <summary>
        /// Ensure the environment variable is reset and the log file deleted for the next execution instance.
        /// </summary>
        [ClassCleanup]
        public static void UnInitialize()
        {
            Environment.SetEnvironmentVariable("NumberofIterations", string.Empty, EnvironmentVariableTarget.User);

            if (File.Exists("runcounter.log"))
            {
                File.Delete("runcounter.log");
            }
        }

        /// <summary>
        /// The test method that has the Repeat Data Attribute set.
        /// </summary>
        /// <param name="iteration">Dummy variable needed to start the Repeat.</param>
        [RepeatTestMethod]
        [TestCategory("RepeatIntegrationTest")]
        public void TestMethod_with_repeatdata_attribute_1()
        {
            // Act and Arrange
            ++counter;
            using (StreamWriter sw = File.AppendText("runcounter.log"))
            {
                sw.Write(counter);
            }

            RepeatTestMethodAttribute.CurrentIteration.Should().Be(counter);
        }

        /// <summary>
        /// Check if the first test executed n times.
        /// </summary>
        [TestMethod]
        [TestCategory("RepeatIntegrationTest")]
        public void TestMethod_with_repeatdata_attribute_2()
        {
            // Verify
            var val = File.ReadAllText("runcounter.log");
            string expectedvalue = string.Empty;
            for (int i = 0; i < targetrepeatcount; i++)
            {
                expectedvalue += (i + 1).ToString();
            }

            val.Should().Be(expectedvalue);
            RepeatTestMethodAttribute.CurrentIteration.Should().Be(targetrepeatcount);
        }
    }
}
