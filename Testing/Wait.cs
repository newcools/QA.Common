namespace Automation.Common.Testing
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// The waiting helpers.
    /// </summary>
    public static class Wait
    {
        #region Public Methods and Operators

        /// <summary>
        /// Blocks the current thread until the specified condition is met, or until the specified time-out expires.
        /// </summary>
        /// <returns>
        /// True if the condition is met before the time-out; otherwise, false.
        /// </returns>
        /// <param name="conditionContext">
        /// The context to evaluate the condition.
        /// </param>
        /// <param name="conditionEvaluator">
        /// The delegate to evaluate the condition.
        /// </param>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds before time-out.
        /// </param>
        /// <typeparam name="T">
        /// The <see cref="T:System.Type"/> that specifies the Type for the condition and predicate.
        /// </typeparam>
        public static bool WaitForCondition<T>(T conditionContext, Predicate<T> conditionEvaluator, int millisecondsTimeout)
        {
            if (Equals(conditionContext, default(T)))
            {
                throw new ArgumentNullException("conditionContext");
            }

            if (conditionEvaluator == null)
            {
                throw new ArgumentNullException("conditionEvaluator");
            }

            if (millisecondsTimeout == -1)
            {
                millisecondsTimeout = int.MaxValue;
            }

            CheckForMinimumPermissibleValue(0, millisecondsTimeout, "millisecondsTimeout");

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (!conditionEvaluator(conditionContext))
            {
                Thread.Sleep(Math.Min(Math.Max((int)(millisecondsTimeout - stopwatch.ElapsedMilliseconds), 0), 100));
                if (stopwatch.ElapsedMilliseconds >= millisecondsTimeout)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Blocks the current thread until the specified condition is met, or until the specified time-out expires.
        /// </summary>
        /// <returns>
        /// True if the condition is met before the time-out; otherwise, false.
        /// </returns>
        /// <param name="conditionEvaluator">
        /// The delegate to evaluate the condition.
        /// </param>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds before time-out.
        /// </param>
        public static bool WaitForCondition(Predicate conditionEvaluator, int millisecondsTimeout)
        {
            if (conditionEvaluator == null)
            {
                throw new ArgumentNullException("conditionEvaluator");
            }

            if (millisecondsTimeout == -1)
            {
                millisecondsTimeout = int.MaxValue;
            }

            CheckForMinimumPermissibleValue(0, millisecondsTimeout, "millisecondsTimeout");

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (!conditionEvaluator())
            {
                Thread.Sleep(Math.Min(Math.Max((int)(millisecondsTimeout - stopwatch.ElapsedMilliseconds), 0), 100));
                if (stopwatch.ElapsedMilliseconds >= millisecondsTimeout)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks for minimum permissible value.
        /// </summary>
        /// <param name="minimumPermissibleValue">
        /// The minimum permissible value.
        /// </param>
        /// <param name="value">
        /// The value to be validated.
        /// </param>
        /// <param name="parameterName">
        /// The parameter name.
        /// </param>
        internal static void CheckForMinimumPermissibleValue(int minimumPermissibleValue, int value, string parameterName)
        {
            if (value >= minimumPermissibleValue)
            {
                return;
            }

            throw new ArgumentException(
                string.Format(
                    CultureInfo.CurrentCulture, 
                    "Invalid value {0} for parameter {1}.", 
                    new[]
                        {
                            value, 
                            (object)parameterName
                        }), 
                parameterName);
        }

        #endregion
    }
}