﻿namespace Automation.Common.Testing
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Defines the test case execution flow controls.
    /// </summary>
    public static class TestExecution
    {
        #region Public Methods and Operators

        /// <summary>
        /// Executes an exception, expecting an exception to be thrown.
        ///     Like Assert.Throws in NUnit.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The exception thrown by the executionAction
        /// </returns>
        public static Exception Throws(Action action)
        {
            return Throws(action, null);
        }

        /// <summary>
        /// Executes an exception, expecting an exception to be thrown.
        ///     Like Assert.Throws in NUnit.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <param name="message">
        /// The error message if the expected exception is not thrown
        /// </param>
        /// <returns>
        /// The exception thrown by the executionAction
        /// </returns>
        public static Exception Throws(Action action, string message)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                // The executionAction method has thrown the expected exception.
                // Return the exception, in case the unit test wants to perform further assertions on it.
                return ex;
            }

            // If we end up here, the expected exception was not thrown. Fail!
            throw new AssertFailedException(message ?? "Expected exception was not thrown.");
        }

        /// <summary>
        /// Executes an exception, expecting an exception of a specific type to be thrown.
        ///     Like Assert.Throws in NUnit.
        /// </summary>
        /// <typeparam name="T">
        /// The exception type.
        /// </typeparam>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The exception thrown by the executionAction
        /// </returns>
        public static T Throws<T>(Action action) where T : Exception
        {
            return Throws<T>(action, null);
        }

        /// <summary>
        /// Executes an exception, expecting an exception of a specific type to be thrown.
        ///     Like Assert.Throws in NUnit.
        /// </summary>
        /// <typeparam name="T">
        /// The exception type.
        /// </typeparam>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <param name="message">
        /// The error message if the expected exception is not thrown
        /// </param>
        /// <returns>
        /// The exception thrown by the executionAction
        /// </returns>
        public static T Throws<T>(Action action, string message) where T : Exception
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                T actual = ex as T;
                if (actual == null)
                {
                    throw new AssertFailedException(message ?? string.Format("Expected exception of type {0} not thrown. Actual exception type was {1}.", typeof(T), ex.GetType()));
                }

                // The executionAction method has thrown the expected exception of type 'T'.
                // Return the exception, in case the unit test wants to perform further assertions on it.
                return actual;
            }

            // If we end up here, the expected exception of type 'T' was not thrown. Fail!
            throw new AssertFailedException(message ?? string.Format("Expected exception of type {0} not thrown.", typeof(T)));
        }

        #endregion
    }
}