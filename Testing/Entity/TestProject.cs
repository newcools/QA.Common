namespace Automation.Common.Testing.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     The test project.
    /// </summary>
    public class TestProject : EntityBase
    {
        #region Fields

        /// <summary>
        ///     The _test result.
        /// </summary>
        private TestResult testResult;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="TestProject"/> class from being created. 
        ///     Initializes a new instance of the <see cref="TestProject"/> class.
        /// </summary>
        internal TestProject()
        {
            this.StartTime = DateTimeAdapter.DateNow;
            this.TestClasses = new List<TestClass>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the test result.
        /// </summary>
        public TestResult TestResult
        {
            get
            {
                return this.testResult;
            }

            internal set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "TestResult is null.");
                }

                this.testResult = value;
                this.testResult.Projects.Add(this);
                this.testResult.Saved = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the test classes.
        /// </summary>
        internal ICollection<TestClass> TestClasses { get; private set; }

        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// To either get an existing test class, or create a new test class if does not exist.
        /// </summary>
        /// <param name="fullName">
        /// The test class full name.
        /// </param>
        /// <returns>
        /// The <see cref="GetTestClass"/> matches the specified test class full name.
        /// </returns>
        public TestClass GetTestClass(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Test class name cannot be null or empty.", "fullName");

            TestClass testClass = this.TestClasses.FirstOrDefault(item => item.FullyQualifiedName.Equals(fullName));
            return testClass
                ?? new TestClass { TestProject = this, FullyQualifiedName = fullName };
        }
        #endregion // Public Methods and Operators
    }
}