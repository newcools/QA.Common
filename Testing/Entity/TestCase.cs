namespace Automation.Common.Testing.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     The test case. Represents a standard test case.
    /// </summary>
    public class TestCase : TestEntity
    {
        #region Fields

        /// <summary>
        ///     The _test class.
        /// </summary>
        private TestClass testClass;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="TestCase" /> class from being created.
        /// </summary>
        internal TestCase()
        {
            this.Scenarios = new List<Scenario>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the scenarios.
        /// </summary>
        internal ICollection<Scenario> Scenarios { get; private set; }

        /// <summary>
        ///     Gets or sets the test class.
        /// </summary>
        /// <value>
        ///     The test class.
        /// </value>
        internal TestClass TestClass
        {
            get
            {
                return this.testClass;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "TestClass is null.");
                }

                this.testClass = value;
                this.testClass.TestCases.Add(this);
                this.testClass.Saved = false;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Create a child <see cref="Scenario"/> from the given scenario name.
        /// </summary>
        /// <param name="name">
        /// The name for the new <see cref="Scenario"/>.
        /// </param>
        /// <returns>
        /// A new instance of <see cref="Scenario"/>.
        /// </returns>
        public Scenario CreateScenario(string name)
        {
            string scenarioName;
            switch (name)
            {
                case "":
                    {
                        scenarioName = "string.Empty";
                        break;
                    }
                case null:
                    {
                        scenarioName = "null scenario name";
                        break;
                    }
                default:
                    {
                        scenarioName = name;
                        break;
                    }
            }

            Scenario scenario = new Scenario(this, string.Format("[{0}]: ", scenarioName));
            return scenario;
        }

        /// <summary>
        ///     Sets the end time to current time.
        /// </summary>
        public override void SetEndTime()
        {
            base.SetEndTime();
            this.TestClass.SetEndTime();
        }

        /// <summary>
        /// Sets the status recursively.
        /// </summary>
        /// <param name="executionStatus">
        /// The status.
        /// </param>
        public override void SetStatus(Status executionStatus)
        {
            if (this.Scenarios.Any())
            {
                base.SetStatus(executionStatus);
            }
            else if (this.Status != Status.Fail)
            {
                this.Status = executionStatus;
            }

            this.TestClass.SetStatus(this.Status);
        }

        /// <summary>
        ///     Get basic information according to the current status.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" /> value represents the <see cref="TestCase"/>.
        /// </returns>
        public override string ToString()
        {
            switch (this.Status)
            {
                case Status.Pass:
                    return string.Format("Test case [{0}] passed.", this.Name);
                case Status.Fail:
                    return
                        string.Format(
                            "Test case [{0}] failed, there were [{1}] validation errors. Refer to the test result files for more information.",
                            this.Name,
                            this.Messages.Count);
                case Status.PartiallyPass:
                    return
                        string.Format(
                            "Test case [{0}] failed, there were [{1}] scenarios failed. Refer to the test result files for more information.",
                            this.Name,
                            this.Scenarios.Count);
                default:
                    return string.Format("Test case [{0}] has unexpected status [{1}].", this.Name, this.Status);
            }
        }

        #endregion
    }
}