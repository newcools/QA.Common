namespace Automation.Common.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The linq extension.
    /// </summary>
    public static class LinqExtension
    {
        /// <summary>
        ///     Perform the action on each of the item from source.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="TSource">
        /// The source data type.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null)
                throw new ArgumentNullException("source", "Source collection cannot be null.");
            if (action == null)
                throw new ArgumentNullException("action", "Action cannot be null.");

            source.ToList().ForEach(action);
        }
    }
}
