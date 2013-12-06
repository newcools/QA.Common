namespace Automation.Common.Testing
{
    using System;

    /// <summary>
    /// Defines pre-defined date time formats.
    /// </summary>
    public static class DateTimeAdapter
    {
        /// <summary>
        /// Gets current time with format: "Hour:Minute:Second".
        /// </summary>
        public static string Now
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss");
            }
        }

        /// <summary>
        /// Gets current time with format: "day Month Hour:Minute:Second".
        /// </summary>
        public static string DateNow
        {
            get
            {
                return DateTime.Now.ToString("dd MMM HH:mm:ss");
            }
        }

        /// <summary>
        /// Gets the file time stamp.
        /// </summary>
        public static string FileTimeStamp
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss");
            }
        }
    }
}
