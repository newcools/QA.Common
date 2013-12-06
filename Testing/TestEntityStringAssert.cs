namespace Automation.Common.Testing
{
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Automation.Common.Testing.Entity;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The test entity assert extensions for string assertion.
    /// </summary>
    public static class TestEntityStringAssert
    {
        #region Public Methods and Operators

        /// <summary>
        /// Verifies that the first string contains the second string. Displays a message if the assertion fails, and applies the specified formatting to it. This method is case sensitive.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="value">
        /// The string that is expected to contain <paramref name="substring"/>.
        /// </param>
        /// <param name="substring">
        /// The string expected to occur within <paramref name="value"/>.
        /// </param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// <paramref name="substring"/> is not found in <paramref name="value"/>.
        /// </exception>
        public static void AssertContains(this TestEntity testEntity, string value, string substring)
        {
            string message = string.Format("Source string [{0}] does not contain the substring [{1}]", value, substring);
            AssertContains(testEntity, value, substring, message);
        }

        /// <summary>
        /// Verifies that the first string contains the second string. Displays a message if the assertion fails, and applies the specified formatting to it. This method is case sensitive.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="value">
        /// The string that is expected to contain <paramref name="substring"/>.
        /// </param>
        /// <param name="substring">
        /// The string expected to occur within <paramref name="value"/>.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results.
        /// </param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// <paramref name="substring"/> is not found in <paramref name="value"/>.
        /// </exception>
        public static void AssertContains(this TestEntity testEntity, string value, string substring, string message)
        {
            AssertContains(testEntity, value, substring, message, new object[] { });
        }

        /// <summary>
        /// Verifies that the first string contains the second string. Displays a message if the assertion fails, and applies the specified formatting to it. This method is case sensitive.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="value">
        /// The string that is expected to contain <paramref name="substring"/>.
        /// </param>
        /// <param name="substring">
        /// The string expected to occur within <paramref name="value"/>.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results.
        /// </param>
        /// <param name="parameters">
        /// An array of parameters to use when formatting <paramref name="message"/>.
        /// </param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// <paramref name="substring"/> is not found in <paramref name="value"/>.
        /// </exception>
        public static void AssertContains(this TestEntity testEntity, string value, string substring, string message, params object[] parameters)
        {
            try
            {
                StringAssert.Contains(value, substring, message, parameters);
            }
            catch (AssertFailedException)
            {
                if (testEntity != null)
                    testEntity.LogError(message, parameters);
                else 
                    throw;
            }
        }

        /// <summary>
        /// Verifies that the specified string matches the regular expression. Displays a message if the assertion fails, and applies the specified formatting to it.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="value">
        /// The string that is expected to contain <paramref name="pattern"/>.
        /// </param>
        /// <param name="pattern">
        /// The regular expression that <paramref name="value"/> is expected to match.
        /// </param>
        public static void AssertMatches(this TestEntity testEntity, string value, Regex pattern)
        {
            string message = string.Format("Source string [{0}] does not match the pattern [{1}]", value, pattern);
            AssertMatches(testEntity, value, pattern, message);
        }

        /// <summary>
        /// Verifies that the specified string matches the regular expression. Displays a message if the assertion fails, and applies the specified formatting to it.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="value">
        /// The string that is expected to contain <paramref name="pattern"/>.
        /// </param>
        /// <param name="pattern">
        /// The regular expression that <paramref name="value"/> is expected to match.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results.
        /// </param>
        public static void AssertMatches(this TestEntity testEntity, string value, Regex pattern, string message)
        {
            AssertMatches(testEntity, value, pattern, message, new object[] { });
        }

        /// <summary>
        /// Verifies that the specified string matches the regular expression. Displays a message if the assertion fails, and applies the specified formatting to it.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="value">
        /// The string that is expected to contain <paramref name="pattern"/>.
        /// </param>
        /// <param name="pattern">
        /// The regular expression that <paramref name="value"/> is expected to match.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results.
        /// </param>
        /// <param name="parameters">
        /// An array of parameters to use when formatting <paramref name="message"/>.
        /// </param>
        public static void AssertMatches(this TestEntity testEntity, string value, Regex pattern, string message, params object[] parameters)
        {
            try
            {
                StringAssert.Matches(value, pattern, message, parameters);
            }
            catch (AssertFailedException)
            {
                if (testEntity != null)
                    testEntity.LogError(message, parameters);
                else
                    throw;
            }
        }

        /// <summary>
        /// Verifies that two specified strings are equal, ignoring case or not as specified, and using the culture info specified. The assertion fails if they are not equal. Displays a message if the assertion fails, and applies the specified formatting to it.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="expected">
        /// The first string to compare. This is the string the unit test expects.
        /// </param>
        /// <param name="actual">
        /// The second string to compare. This is the string the unit test produced.
        /// </param>
        /// <param name="ignoreCase">
        /// A Boolean value that indicates a case-sensitive or insensitive comparison. true indicates a case-insensitive comparison.
        /// </param>
        /// <param name="culture">
        /// A <see cref="T:System.Globalization.CultureInfo"/> object that supplies culture-specific comparison information.
        /// </param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// <paramref name="expected"/> is not equal to <paramref name="actual"/>.
        /// </exception>
        public static void AssertAreEqual(this TestEntity testEntity, string expected, string actual, bool ignoreCase, CultureInfo culture)
        {
            string message = string.Format("Actual string [{0}] does not equal the expected string [{1}]", actual, expected);
            AssertAreEqual(testEntity, expected, actual, ignoreCase, culture, message);
        }

        /// <summary>
        /// Verifies that two specified strings are equal, ignoring case or not as specified, and using the culture info specified. The assertion fails if they are not equal. Displays a message if the assertion fails, and applies the specified formatting to it.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="expected">
        /// The first string to compare. This is the string the unit test expects.
        /// </param>
        /// <param name="actual">
        /// The second string to compare. This is the string the unit test produced.
        /// </param>
        /// <param name="ignoreCase">
        /// A Boolean value that indicates a case-sensitive or insensitive comparison. true indicates a case-insensitive comparison.
        /// </param>
        /// <param name="culture">
        /// A <see cref="T:System.Globalization.CultureInfo"/> object that supplies culture-specific comparison information.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results.
        /// </param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// <paramref name="expected"/> is not equal to <paramref name="actual"/>.
        /// </exception>
        public static void AssertAreEqual(this TestEntity testEntity, string expected, string actual, bool ignoreCase, CultureInfo culture, string message)
        {
            AssertAreEqual(testEntity, expected, actual, ignoreCase, culture, message, new object[] { });
        }

        /// <summary>
        /// Verifies that two specified strings are equal, ignoring case or not as specified, and using the culture info specified. The assertion fails if they are not equal. Displays a message if the assertion fails, and applies the specified formatting to it.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="expected">
        /// The first string to compare. This is the string the unit test expects.
        /// </param>
        /// <param name="actual">
        /// The second string to compare. This is the string the unit test produced.
        /// </param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// <paramref name="expected"/> is not equal to <paramref name="actual"/>.
        /// </exception>
        public static void AssertAreEqual(this TestEntity testEntity, string expected, string actual)
        {
            AssertAreEqual(testEntity, expected, actual, false);
        }

        /// <summary>
        /// Verifies that two specified strings are equal, ignoring case or not as specified, and using the culture info specified. The assertion fails if they are not equal. Displays a message if the assertion fails, and applies the specified formatting to it.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="expected">
        /// The first string to compare. This is the string the unit test expects.
        /// </param>
        /// <param name="actual">
        /// The second string to compare. This is the string the unit test produced.
        /// </param>
        /// <param name="ignoreCase">
        /// A Boolean value that indicates a case-sensitive or insensitive comparison. true indicates a case-insensitive comparison.
        /// </param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// <paramref name="expected"/> is not equal to <paramref name="actual"/>.
        /// </exception>
        public static void AssertAreEqual(this TestEntity testEntity, string expected, string actual, bool ignoreCase)
        {
            string message = string.Format("Actual string [{0}] does not equal the expected string [{1}]", actual, expected);
            AssertAreEqual(testEntity, expected, actual, ignoreCase, message);
        }

        /// <summary>
        /// Verifies that two specified strings are equal, ignoring case or not as specified, and using the culture info specified. The assertion fails if they are not equal. Displays a message if the assertion fails, and applies the specified formatting to it.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="expected">
        /// The first string to compare. This is the string the unit test expects.
        /// </param>
        /// <param name="actual">
        /// The second string to compare. This is the string the unit test produced.
        /// </param>
        /// <param name="ignoreCase">
        /// A Boolean value that indicates a case-sensitive or insensitive comparison. true indicates a case-insensitive comparison.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results.
        /// </param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// <paramref name="expected"/> is not equal to <paramref name="actual"/>.
        /// </exception>
        public static void AssertAreEqual(this TestEntity testEntity, string expected, string actual, bool ignoreCase, string message)
        {
            AssertAreEqual(testEntity, expected, actual, ignoreCase, message, new object[] { });
        }

        /// <summary>
        /// Verifies that two specified strings are equal, ignoring case or not as specified, and using the culture info specified. The assertion fails if they are not equal. Displays a message if the assertion fails, and applies the specified formatting to it.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="expected">
        /// The first string to compare. This is the string the unit test expects.
        /// </param>
        /// <param name="actual">
        /// The second string to compare. This is the string the unit test produced.
        /// </param>
        /// <param name="ignoreCase">
        /// A Boolean value that indicates a case-sensitive or insensitive comparison. true indicates a case-insensitive comparison.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results.
        /// </param>
        /// <param name="parameters">
        /// An array of parameters to use when formatting <paramref name="message"/>.
        /// </param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// <paramref name="expected"/> is not equal to <paramref name="actual"/>.
        /// </exception>
        public static void AssertAreEqual(this TestEntity testEntity, string expected, string actual, bool ignoreCase, string message, params object[] parameters)
        {
            AssertAreEqual(testEntity, expected, actual, ignoreCase, CultureInfo.InvariantCulture, message, parameters);
        }

        /// <summary>
        /// Verifies that two specified strings are equal, ignoring case or not as specified, and using the culture info specified. The assertion fails if they are not equal. Displays a message if the assertion fails, and applies the specified formatting to it.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="expected">
        /// The first string to compare. This is the string the unit test expects.
        /// </param>
        /// <param name="actual">
        /// The second string to compare. This is the string the unit test produced.
        /// </param>
        /// <param name="ignoreCase">
        /// A Boolean value that indicates a case-sensitive or insensitive comparison. true indicates a case-insensitive comparison.
        /// </param>
        /// <param name="culture">
        /// A <see cref="T:System.Globalization.CultureInfo"/> object that supplies culture-specific comparison information.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results.
        /// </param>
        /// <param name="parameters">
        /// An array of parameters to use when formatting <paramref name="message"/>.
        /// </param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// <paramref name="expected"/> is not equal to <paramref name="actual"/>.
        /// </exception>
        public static void AssertAreEqual(this TestEntity testEntity, string expected, string actual, bool ignoreCase, CultureInfo culture, string message, params object[] parameters)
        {
            try
            {
                Assert.AreEqual(expected, actual, ignoreCase, culture, message, parameters);
            }
            catch (AssertFailedException)
            {
                if (testEntity != null)
                    testEntity.LogError(message, parameters);
                else
                    throw;
            }
        }
        #endregion
    }
}