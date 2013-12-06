namespace Automation.Common.Testing
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using Automation.Common.Testing.Entity;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The test base.
    /// </summary>
    [DeploymentItem("View", "View")]
    [DeploymentItem("Scripts", "Scripts")]
    public abstract class TestBase
    {
        #region Constants

        /// <summary>
        /// The result file name.
        /// </summary>
        private const string DefaultResultsFileName = "TestResults.xml";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialises static members of the <see cref="TestBase"/> class.
        /// </summary>
        static TestBase()
        {
            PrepareDropRoot();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the results directory.
        /// </summary>
        public static string ResultsDirectory { get; private set; }
        
        /// <summary>
        /// Gets or sets the test context which provides information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        #endregion //Public Properties

        #region Properties

        /// <summary>
        /// Gets the test settings.
        /// </summary>
        protected static TestSettings TestSettings
        {
            get { return TestSettings.Instance; }
        }

        /// <summary>
        /// Gets or sets the current test case.
        /// </summary>
        protected static TestCase CurrentTestCase { get; set; }

        #endregion // Properties

        #region Public Methods and Operators

        /// <summary>
        /// Setup test class entity.
        /// </summary>
        /// <param name="testContext">
        /// The test context.
        /// </param>
        /// <param name="useDefaultResultFile">
        /// A flag indicates whether uses the default result file.
        /// </param>
        /// <returns>
        /// The <see cref="TestClass"/>.
        /// </returns>
        protected static TestClass SetTestClass(TestContext testContext, bool useDefaultResultFile = false)
        {
            string projectName = testContext.GetNameSpace();
            string resultFile = useDefaultResultFile ? DefaultResultsFileName : string.Format("{0}-{1}", projectName, DefaultResultsFileName);

            TestResult result = TestResult.InitTestResult(resultFile);
            TestProject testProject = result.GetTestProject(projectName);
            TestClass testClass = testProject.GetTestClass(testContext.FullyQualifiedTestClassName);
            testClass.Name = testContext.GetTestClassName();

            return testClass;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize the test case.
        /// </summary>
        /// <param name="useDefaultResultFile">
        /// A flag indicating whether to use the common result file.
        /// </param>
        protected virtual void SetTestCase(bool useDefaultResultFile = false)
        {
            TestClass testClass = SetTestClass(this.TestContext, useDefaultResultFile);

            this.SetTestCase(testClass, useDefaultResultFile);
        }

        /// <summary>
        /// Sets test case entity.
        /// </summary>
        /// <param name="testClass">
        /// The test class.
        /// </param>
        /// <param name="useDefaultResultFile">
        /// A flag indicating whether to use the default result file.
        /// </param>
        protected virtual void SetTestCase(TestClass testClass, bool useDefaultResultFile = false)
        {
            if (testClass == null)
            {
                throw new ArgumentNullException("testClass", "Test class cannot be null.");
            }

            TestCase testCase = testClass.GetTestCase(this.TestContext.FullyQualifiedTestCaseName());
            testCase.Name = this.TestContext.TestName;
            CurrentTestCase = testCase;
        }

        /// <summary>
        /// Prepares the drop root for test results.
        /// </summary>
        private static void PrepareDropRoot()
        {
            try
            {
                DirectoryInfo dropRootDirectory = new DirectoryInfo(TestSettings.DropRoot);

                // To ensure can create the directory configured in the application configuration file.
                if (!dropRootDirectory.Exists)
                {
                    dropRootDirectory.Create();
                }
            }
            catch (Exception)
            {
                TestSettings.DropRoot = Path.GetTempPath();
                Debug.WriteLine("The specified path was invalid, using default drop root.");
            }

            DirectoryInfo resultsDirectory = new DirectoryInfo(Path.Combine(TestSettings.DropRoot, "TestResults", DateTimeAdapter.FileTimeStamp));
            resultsDirectory.Create();
            ResultsDirectory = resultsDirectory.FullName;

            const string ContentPath = @"View";
            const string ScriptsPath = @"Scripts";
            DirectoryInfo contentSource = new DirectoryInfo(ContentPath);
            DirectoryInfo scriptsSource = new DirectoryInfo(ScriptsPath);
            if (!contentSource.Exists || !scriptsSource.Exists)
            {
                return;
            }

            DirectoryInfo contentTarget = resultsDirectory.CreateSubdirectory(ContentPath);

            foreach (FileInfo file in contentSource.EnumerateFiles())
            {
                file.CopyTo(Path.Combine(contentTarget.FullName, file.Name), true);
            }

            DirectoryInfo scriptsTarget = resultsDirectory.CreateSubdirectory(ScriptsPath);

            foreach (FileInfo file in scriptsSource.EnumerateFiles())
            {
                file.CopyTo(Path.Combine(scriptsTarget.FullName, file.Name), true);
            }
        }

        /// <summary>
        /// Blocks the current thread until the specified condition is met, or until the specified time-out expires.
        /// </summary>
        /// <returns>
        /// True if the condition is met before the time-out; otherwise, false.
        /// </returns>
        /// <param name="conditionEvaluator">
        /// The delegate to evaluate the condition.
        /// </param>
        protected static bool WaitForCondition(Predicate conditionEvaluator)
        {
            return Wait.WaitForCondition(conditionEvaluator, TestSettings.WaitForReadyTimeout);
        }

        /// <summary>
        /// Saves test results.
        /// </summary>
        /// <param name="testClass">
        /// The test class.
        /// </param>
        protected static void SaveTestResults(TestClass testClass)
        {
            if (testClass == null)
                return;
            
            TestResult result = testClass.TestProject.TestResult;
            result.Save();
            result.CopyTo(ResultsDirectory);
        }
        #endregion
    }
}