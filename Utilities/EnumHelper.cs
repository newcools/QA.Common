namespace Automation.Common.Utilities
{
    using System;

    /// <summary>
    /// The enum helper.
    /// </summary>
    public static class EnumHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum">
        /// The enumeration type to which to convert <paramref name="value"/>.
        /// </typeparam>
        /// <param name="value">
        /// An object that implements the <see cref="T:System.IConvertible"/> interface, or null. 
        /// </param>
        /// <param name="flags">
        /// Indicates if the <see cref="TEnum"/> has the Flags attributes
        /// </param>
        /// <returns>
        /// An object of type <see cref="TEnum"/> whose value is represented by <paramref name="value"/>.
        /// </returns>
        public static TEnum AsEnum<TEnum>(this object value, bool flags = false) where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            TEnum result;
            bool parsed = Enum.TryParse(value.ToString(), true, out result);
            if (!parsed)
            {
                return default(TEnum);
            }

            if (flags)
            {
                return result;
            }

            return Enum.IsDefined(typeof(TEnum), result) ? result : default(TEnum);
        }

        #endregion
    }
}