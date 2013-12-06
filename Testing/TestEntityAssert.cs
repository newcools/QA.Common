namespace Automation.Common.Testing
{
    using System;

    using Automation.Common.Testing.Entity;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The test entity extensions.
    /// </summary>
    public static class TestEntityAssert
    {
        #region Public Methods and Operators

        /// <summary>
        ///     To assert and continue if the assertion fails. The assertion message is added into the current test entity.
        /// </summary>
        /// <param name="testEntity">
        ///     The test entity.
        /// </param>
        /// <param name="assert">
        ///     The assert action.
        /// </param>
        public static void AssertAndContinue(this TestEntity testEntity, Action assert)
        {
            try
            {
                assert();
            }
            catch (AssertFailedException failedException)
            {
                if (testEntity == null) throw;
                testEntity.LogError(failedException.Message);
            }
        }

        /// <summary>
        /// Verifies that two specified generic type data are equal by using the equality
        ///     operator. The assertion fails if they are not equal. Displays a message if
        ///     the assertion fails.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="expected">
        /// The first generic type data to compare. This is the generic type data the
        ///     unit test expects.
        /// </param>
        /// <param name="actual">
        /// The second generic type data to compare. This is the generic type data the
        ///     unit test produced.
        /// </param>
        /// <typeparam name="T">
        /// The object specific type.
        /// </typeparam>
        public static void AssertAreEqual<T>(this TestEntity testEntity, T expected, T actual)
        {
            string message = string.Format("Actual string [{0}] does not equal the expected string [{1}]", actual, expected);
            AssertAreEqual(testEntity, expected, actual, message);
        }
        
        /// <summary>
        /// Verifies that two specified generic type data are equal by using the equality operator. The assertion fails if they are not equal. Displays a message if the assertion fails, and applies the specified formatting to it.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="expected">
        /// The first generic type data to compare. This is the generic type data the unit test expects.
        /// </param>
        /// <param name="actual">
        /// The second generic type data to compare. This is the generic type data the unit test produced.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results.
        /// </param>
        /// <typeparam name="T">
        /// The object specific type.
        /// </typeparam>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// <paramref name="expected"/> is not equal to <paramref name="actual"/>.
        /// </exception>
        public static void AssertAreEqual<T>(this TestEntity testEntity, T expected, T actual, string message)
        {
            AssertAreEqual(testEntity, expected, actual, message, new object[] { });
        }

        /// <summary>
        /// Verifies that two specified generic type data are equal by using the equality operator. The assertion fails if they are not equal. Displays a message if the assertion fails, and applies the specified formatting to it.
        /// </summary>
        /// <param name="testEntity">
        /// The test Entity.
        /// </param>
        /// <param name="expected">
        /// The first generic type data to compare. This is the generic type data the unit test expects.
        /// </param>
        /// <param name="actual">
        /// The second generic type data to compare. This is the generic type data the unit test produced.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results.
        /// </param>
        /// <param name="parameters">
        /// An array of parameters to use when formatting <paramref name="message"/>.
        /// </param>
        /// <typeparam name="T">
        /// The object specific type.
        /// </typeparam>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// <paramref name="expected"/> is not equal to <paramref name="actual"/>.
        /// </exception>
        public static void AssertAreEqual<T>(this TestEntity testEntity, T expected, T actual, string message, params object[] parameters)
        {
            try
            {
                Assert.AreEqual(expected, actual, message, parameters);
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
        /// Verifies that the specified condition is false. The assertion fails if the condition is true,
        ///     a message is added to the test entity if assertion fails.
        /// </summary>
        /// <param name="testEntity">
        /// The test entity.
        /// </param>
        /// <param name="condition">
        /// The condition.
        /// </param>
        public static void AssertIsFalse(this TestEntity testEntity, bool condition)
        {
            AssertIsFalse(testEntity, condition, null);
        }

        /// <summary>
        /// Verifies that the specified condition is false. The assertion fails if the condition is true,
        ///     a message is added to the test entity if assertion fails.
        /// </summary>
        /// <param name="testEntity">
        /// The test entity.
        /// </param>
        /// <param name="condition">
        /// The condition.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results, if no test entity specified.
        ///     Or a message added to the current test entity.
        /// </param>
        public static void AssertIsFalse(this TestEntity testEntity, bool condition, string message)
        {
            AssertIsFalse(testEntity, condition, message, null);
        }

        /// <summary>
        /// Verifies that the specified condition is false. The assertion fails if the condition is true,
        ///     a message is added to the test entity if assertion fails.
        /// </summary>
        /// <param name="testEntity">
        /// The test entity.
        /// </param>
        /// <param name="condition">
        /// The condition.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results, if no test entity specified.
        ///     Or a message added to the current test entity.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public static void AssertIsFalse(this TestEntity testEntity, bool condition, string message, params object[] parameters)
        {
            if (!condition)
                return;
            if (testEntity == null)
            {
                Assert.IsFalse(condition, message, parameters);
            }
            testEntity.LogError(message, "Assert Is False Failed. The condition evaluated as [True].", parameters);
        }

        /// <summary>
        /// Verifies that the specified object is not null. The assertion fails if it is null. Displays a message if the assertion fails.
        /// </summary>
        /// <param name="testEntity">
        /// The test entity.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void AssertIsNotNull(this TestEntity testEntity, object value)
        {
            AssertIsNotNull(testEntity, value, null);
        }

        /// <summary>
        /// Verifies that the specified object is not null. The assertion fails if it is null. Displays a message if the assertion fails.
        /// </summary>
        /// <param name="testEntity">
        /// The test entity.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results when no test entity specified.
        ///     Or a message added to the current test entity.
        /// </param>
        public static void AssertIsNotNull(this TestEntity testEntity, object value, string message)
        {
            AssertIsNotNull(testEntity, value, message,  (object[]) null);
        }

        /// <summary>
        /// Verifies that the specified object is not null. The assertion fails if it is null. Displays a message if the assertion fails.
        /// </summary>
        /// <param name="testEntity">
        /// The test entity.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results when no test entity specified.
        ///     Or a message added to the current test entity.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public static void AssertIsNotNull(this TestEntity testEntity, object value, string message, params object[] parameters)
        {
            if (value != null)
            {
                return;
            }
            if (testEntity == null)
            {
                Assert.IsNotNull(value, message, parameters);
            }
            testEntity.LogError(message, "Assert Is not null Failed. The object is null.", parameters);
        }

        /// <summary>
        /// Verifies that the specified object is null. The assertion fails if it is null. Displays a message if the assertion fails.
        /// </summary>
        /// <param name="testEntity">
        /// The test entity.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void AssertIsNull(this TestEntity testEntity, object value)
        {
            AssertIsNull(testEntity, value, null);
        }

        /// <summary>
        /// Verifies that the specified object is null. The assertion fails if it is null. Displays a message if the assertion fails.
        /// </summary>
        /// <param name="testEntity">
        /// The test entity.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results when no test entity specified.
        ///     Or a message added to the current test entity.
        /// </param>
        public static void AssertIsNull(this TestEntity testEntity, object value, string message)
        {
            AssertIsNull(testEntity, value, message, null);
        }

        /// <summary>
        /// Verifies that the specified object is null. The assertion fails if it is null. Displays a message if the assertion fails.
        /// </summary>
        /// <param name="testEntity">
        /// The test entity.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results when no test entity specified.
        ///     Or a message added to the current test entity.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public static void AssertIsNull(this TestEntity testEntity, object value, string message, params object[] parameters)
        {
            if (value == null)
            {
                return;
            }
            if (testEntity == null)
            {
                Assert.IsNull(value, message, parameters);
            }

            testEntity.LogError(message, "Assert Is null Failed. The object is null.", parameters);
        }

        /// <summary>
        /// Verifies that the specified condition is true. The assertion fails if the condition is false,
        ///     a message is added to the test entity if assertion fails.
        /// </summary>
        /// <param name="testEntity">
        /// The test entity.
        /// </param>
        /// <param name="condition">
        /// The condition.
        /// </param>
        public static void AssertIsTrue(this TestEntity testEntity, bool condition)
        {
            AssertIsTrue(testEntity, condition, null);
        }

        /// <summary>
        /// Verifies that the specified condition is true. The assertion fails if the condition is false,
        ///     a message is added to the test entity if assertion fails.
        /// </summary>
        /// <param name="testEntity">
        /// The test entity.
        /// </param>
        /// <param name="condition">
        /// The condition.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results when no test entity specified.
        ///     Or a message added to the current test entity.
        /// </param>
        public static void AssertIsTrue(this TestEntity testEntity, bool condition, string message)
        {
            AssertIsTrue(testEntity, condition, message, null);
        }

        /// <summary>
        /// Verifies that the specified condition is true. The assertion fails if the condition is false,
        ///     a message is added to the test entity if assertion fails.
        /// </summary>
        /// <param name="testEntity">
        /// The test entity.
        /// </param>
        /// <param name="condition">
        /// The condition.
        /// </param>
        /// <param name="message">
        /// A message to display if the assertion fails. This message can be seen in the unit test results when no test entity specified.
        ///     Or a message added to the current test entity.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public static void AssertIsTrue(this TestEntity testEntity, bool condition, string message, params object[] parameters)
        {
            if (condition)
            {
                return;
            }
            if (testEntity == null)
            {
                Assert.IsTrue(condition, message, parameters);
            }
            if (string.IsNullOrWhiteSpace(message))
                testEntity.LogError("Assert Is True Failed. The condition evaluated as [False].");
            testEntity.LogError(message, "Assert Is True Failed. The condition evaluated as [False].", parameters);
        }
        #endregion
    }
}