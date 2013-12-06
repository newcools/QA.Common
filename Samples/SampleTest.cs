using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Samples
{
    using System.Diagnostics;

    using Automation.Common.Testing;
    using Automation.Common.Testing.Entity;

    /// <summary>
    /// Sample test class demonstrate how to use the testing libraries.
    /// </summary>
    [TestClass]
    public class SampleTest : TestBase
    {
        /// <summary>
        /// The expected string.
        /// </summary>
        private const string ExpectedString = "Expected";

        #region Initialize and Cleanup
        /// <summary>
        /// The test class entity.
        /// </summary>
        private static TestClass testClass;

        /// <summary>
        /// The test class initialization.
        /// </summary>
        /// <param name="testContext">
        /// The test context.
        /// </param>
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            testClass = SetTestClass(testContext);
        }

        /// <summary>
        ///     Saves the test result report.
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            SaveTestResults(testClass);
        }

        /// <summary>
        ///     Initialize test case for result logging and execution control.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.SetTestCase(testClass);
        }
        #endregion //Initialize and Cleanup

        #region Test Methods

        /// <summary>
        /// The positive test execution sample.
        /// </summary>
        [TestMethod]
        public void PositiveTestExecution()
        {
            CurrentTestCase.Execute(
                @testEntity =>
                    {
                        string actual = TestSomething();
                        @testEntity.AssertAreEqual(ExpectedString, actual);
                        @testEntity.AssertIsFalse(false);
                        @testEntity.AssertIsNull(null);
                    });
        }


        /// <summary>
        /// A fail test sample.
        /// </summary>
        [TestMethod]
        public void FailTestExecution()
        {
            CurrentTestCase.Execute(
                @testEntity =>
                {
                    string actual = TestSomething();
                    @testEntity.AssertAreEqual(ExpectedString, actual);
                    @testEntity.AssertIsFalse(true, "Expecting false.");
                    @testEntity.AssertIsNull(null);
                });
        }

        /// <summary>
        /// The expect exception.
        /// </summary>
        [TestMethod]
        public void ExpectException()
        {
            CurrentTestCase.ExpectException<ArgumentException>(
                @testEntity =>
                    {
                        RunAndThrowException();
                    },
                (exception, @testEntity) =>
                    {
                        @testEntity.AssertContains(exception.Message, "argument");
                    });
        }

        /// <summary>
        /// The run and throw exception dummy method.
        /// </summary>
        private static void RunAndThrowException()
        {
            throw new ArgumentException("The provided argument is invalid.");
        }

        /// <summary>
        /// The dummy testing method.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string TestSomething()
        {
            Debug.WriteLine("Test step 1");
            Debug.WriteLine("Test step 2");
            Debug.WriteLine("Test step 3");
            return ExpectedString;
        }

        #endregion Test Methods
    }
}
