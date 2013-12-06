namespace Automation.Common.Testing.Entity
{
    /// <summary>
    /// The message type.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// When the message type is not defined.
        /// </summary>
        None = 0,

        /// <summary>
        /// The validation error type.
        /// </summary>
        Error = 1,

        /// <summary>
        /// The exception type.
        /// </summary>
        Exception = 2,

        /// <summary>
        /// The warning type.
        /// </summary>
        Warning = 3,

        /// <summary>
        /// The information type.
        /// </summary>
        Information = 4
    }
}
