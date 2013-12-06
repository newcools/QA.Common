namespace Automation.Common.Testing
{
    using System;

    /// <summary>
    ///     The test case id attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestCaseIdAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCaseIdAttribute"/> class.
        /// </summary>
        /// <param name="testCaseId">
        /// The test case id.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public TestCaseIdAttribute(string testCaseId)
        {
            if (string.IsNullOrWhiteSpace(testCaseId))
            {
                throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'testCaseId'.", "testCaseId");
            }

            this.TestCaseId = testCaseId;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the test case id.
        /// </summary>
        public string TestCaseId { get; private set; }

        #endregion
    }
}