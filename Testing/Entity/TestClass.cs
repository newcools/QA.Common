namespace Automation.Common.Testing.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     The test class that contains the test cases.
    /// </summary>
    public class TestClass : EntityBase
    {
        #region Fields

        /// <summary>
        ///     The test project that the current test class belongs to.
        /// </summary>
        private TestProject testProject;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="TestClass"/> class from being created. 
        ///     Initializes a new instance of the <see cref="TestClass"/> class.
        /// </summary>
        internal TestClass()
        {
            this.TestCases = new List<TestCase>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the test project.
        /// </summary>
        public TestProject TestProject
        {
            get
            {
                return this.testProject;
            }

            internal set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "TestProject is null.");
                }

                this.testProject = value;
                this.testProject.TestClasses.Add(this);
                this.testProject.Saved = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the test cases.
        /// </summary>
        internal ICollection<TestCase> TestCases { get; private set; }

        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// To either get an existing test case, or create a new test case if does not exist.
        /// </summary>
        /// <param name="fullName">
        /// The test case full name.
        /// </param>
        /// <returns>
        /// The <see cref="TestCase"/> matches the specified test case full name.
        /// </returns>
        public TestCase GetTestCase(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Test case name cannot be null or empty.", "fullName");

            TestCase testClass = this.TestCases.FirstOrDefault(item => item.FullyQualifiedName.Equals(fullName));
            return testClass
                ?? new TestCase { TestClass = this, FullyQualifiedName = fullName };
        }

        /// <summary>
        ///     Sets the end time to current time.
        /// </summary>
        public override void SetEndTime()
        {
            base.SetEndTime();
            this.TestProject.SetEndTime();
        }

        /// <summary>
        /// Sets the status recursively.
        /// </summary>
        /// <param name="executionStatus">
        /// The status.
        /// </param>
        public override void SetStatus(Status executionStatus)
        {
            base.SetStatus(executionStatus);
            this.TestProject.SetStatus(this.Status);
        }

        #endregion
    }
}