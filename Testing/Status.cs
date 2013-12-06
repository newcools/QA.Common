namespace Automation.Common.Testing
{
    /// <summary>
    /// The status for test cases, test scenarios or test classes.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// The not started status.
        /// </summary>
        NotStarted = 0,

        /// <summary>
        /// The pass.
        /// </summary>
        Pass = 1,

        /// <summary>
        /// The partially pass.
        /// </summary>
        PartiallyPass = 2,

        /// <summary>
        /// The fail.
        /// </summary>
        Fail = 3
    }
}
