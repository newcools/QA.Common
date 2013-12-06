namespace Automation.Common.Testing.Entity
{
	/// <summary>
	/// The test case or test scenario validation message.
	/// </summary>
    public class Message
    {
		/// <summary>
		/// Message constructor. When creating a new message, set the message time as the current time.
		/// </summary>
        public Message()
        {
            this.Time = DateTimeAdapter.DateNow;
        }

		/// <summary>
		/// The message body of the current message.
		/// </summary>
        public string Description { get; set; }

		/// <summary>
		/// The time when the message created.
		/// </summary>
        public string Time { get; internal set; }

		/// <summary>
		/// The current message type.
		/// </summary>
		public MessageType MessageType { get; set; }
    }
}
