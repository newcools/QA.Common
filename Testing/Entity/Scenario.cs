namespace Automation.Common.Testing.Entity
{
    using System;

    /// <summary>
    ///     Test scenario, child type of test case. Used for data-driven test cases.
    /// </summary>
    public class Scenario : TestEntity
    {
        #region Fields

        /// <summary>
        ///     The _test case.
        /// </summary>
        private TestCase _testCase;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenario"/> class.
        /// </summary>
        /// <param name="testCase">
        /// The test case.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        internal Scenario(TestCase testCase, string name)
        {
            // TODO: Complete member initialization
            this.TestCase = testCase;
            this.Name = name;
        }

        /// <summary>
        ///     Prevents a default instance of the <see cref="Scenario" /> class from being created.
        /// </summary>
        private Scenario()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the test case.
        /// </summary>
        internal TestCase TestCase
        {
            get
            {
                return this._testCase;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "TestCace is null.");
                }

                this._testCase = value;
                this._testCase.Scenarios.Add(this);
                this._testCase.Saved = false;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Factory method to get a test <see cref="Scenario"/> instance.
        /// </summary>
        /// <param name="parent">
        /// The parent, which is of type <see cref="TestCase"/>.
        /// </param>
        /// <param name="fullyQualifiedName">
        /// The fully qualified name.
        /// </param>
        /// <returns>
        /// The <see cref="Scenario"/> instance.
        /// </returns>
        public static Scenario GetTestScenario(TestCase parent, string fullyQualifiedName)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent", "The parent test case cannot be null");
            }

            if (string.IsNullOrWhiteSpace(fullyQualifiedName))
            {
                throw new ArgumentException("Test scenario FQDN cannot be null or empty", "fullyQualifiedName");
            }

            var scenario = new Scenario { TestCase = parent, FullyQualifiedName = fullyQualifiedName };

            return scenario;
        }

        /// <summary>
        ///     Sets the end time to current time.
        /// </summary>
        public override void SetEndTime()
        {
            base.SetEndTime();
            this.TestCase.SetEndTime();
        }

        /// <summary>
        /// Sets the status recursively.
        /// </summary>
        /// <param name="executionStatus">
        /// The execution status.
        /// </param>
        public override void SetStatus(Status executionStatus)
        {
            if (this.Status != Status.Fail)
            {
                this.Status = executionStatus;
            }

            this.TestCase.SetStatus(this.Status);
        }

        /// <summary>
        ///     Get basic information according to the current status.
        /// </summary>
        /// <returns>
        ///     The <see cref="Scenario"/> instance as <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            switch (this.Status)
            {
                case Status.NotStarted:
                    return string.Format("Test scenario [{0}] not started yet.", this.Name);
                case Status.Pass:
                    return string.Format("Test scenario [{0}] passed.", this.Name);
                case Status.Fail:
                    return
                        string.Format(
                            "Test scenario [{0}] failed, there were [{1}] validation errors. Refer to the test result files for more information.",
                            this.Name,
                            this.Messages.Count);
                default:
                    return string.Format("Test scenario [{0}] has unexpected status [{1}].", this.Name, this.Status);
            }
        }

        #endregion
    }
}