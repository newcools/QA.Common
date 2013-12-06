namespace Automation.Common.Utilities
{
    using System;

    /// <summary>
    ///     The string helper.
    /// </summary>
    public static class StringHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// To randomize the provided string by appending a random <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">
        /// The original value.
        /// </param>
        /// <returns>
        /// The randomized <see cref="string"/>.
        /// </returns>
        public static string Random(this string value)
        {
            return string.Format("{0} - {1}", value, Guid.NewGuid());
        }

        /// <summary>
        /// Escapes for data filter.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string EscapeForDataFilter(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("The input data is null or a white space.", "data");
            }

            return data.Replace("'", "''");
        }

        /// <summary>
        /// Format a the key and value pair.
        /// </summary>
        /// <param name="key">
        /// The key.  
        /// </param>
        /// <param name="value">
        /// The value. 
        /// </param>
        /// <param name="dilimited">
        /// If true, append the separator string to the end. 
        /// </param>
        /// <param name="separator">
        /// The separator. 
        /// </param>
        /// <returns>
        /// The formatted string with the form of [Key:Value]. 
        /// </returns>
        public static string FormatAsKeyValuePair(string key, string value, bool dilimited = true, string separator = "|")
        {
            return string.Format("[{0}:{1}]{2}", key, value, dilimited ? separator : string.Empty);
        }

        /// <summary>
        /// Format the existing string to be encapsulated by double quote mark.
        /// </summary>
        /// <param name="input">
        /// The input string. 
        /// </param>
        /// <param name="quoted">
        /// To specify whether to double quote the original string. 
        /// </param>
        /// <param name="dilimited">
        /// If true, append the separator string to the end. 
        /// </param>
        /// <param name="separator">
        /// The separator. 
        /// </param>
        /// <returns>
        /// The System.String. 
        /// </returns>
        public static string QuoteAndDelimite(string input, bool quoted = false, bool dilimited = true, string separator = "|")
        {
            string result = input;
            if (quoted)
            {
                result = string.Format("\"{0}\"", result);
            }

            if (dilimited)
            {
                result = string.Format("{0}{1}", result, separator);
            }

            return result;
        }

        #endregion
    }
}