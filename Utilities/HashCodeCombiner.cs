namespace Automation.Common.Utilities
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Combines hash codes.
    /// </summary>
    public static class HashCodeCombiner
    {
        #region Constants

        /// <summary>
        ///     Another magic number for multiplication before adding each field.
        /// </summary>
        /// <remarks>
        ///     See the remarks for <see cref="HashCodeStarter" />.
        /// </remarks>
        private const int HashCodeMultiplier = 31;

        /// <summary>
        ///     Magic number for starting hash code.
        /// </summary>
        /// <remarks>
        ///     Don't ask me why this works; it just does.
        /// </remarks>
        private const int HashCodeStarter = 17;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Combine the hash codes of the specified objects.
        /// </summary>
        /// <param name="values">
        /// The objects to combine.
        /// </param>
        /// <returns>
        /// The combined hash code (generated using addition combined with prime multiplication).
        /// </returns>
        public static int CombineHashCodes(params object[] values)
        {
            return values == null ? 0 : CombineHashCodes(values.AsEnumerable());
        }

        /// <summary>
        /// Combine the hash codes of the specified objects.
        /// </summary>
        /// <param name="objects">
        /// The objects to combine.
        /// </param>
        /// <returns>
        /// The combined hash code (generated using addition combined with prime multiplication).
        /// </returns>
        public static int CombineHashCodes(IEnumerable<object> objects)
        {
            if (objects == null)
            {
                return 0;
            }

            int hashCode = HashCodeStarter;
            foreach (object currentObject in objects)
            {
                unchecked
                {
                    // Wrap around on overflow.
                    hashCode *= HashCodeMultiplier;
                    if (currentObject == null)
                    {
                        continue;
                    }

                    hashCode += currentObject.GetHashCode();
                }
            }

            return hashCode;
        }

        #endregion
    }
}