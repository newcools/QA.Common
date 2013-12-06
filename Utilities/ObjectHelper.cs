namespace Automation.Common.Utilities
{
    using System;
    using System.Reflection;

    /// <summary>
    ///     The object helper.
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        ///     To copy public instance fields to a target object of the same type.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <typeparam name="T">
        ///     The object type.
        /// </typeparam>
        public static void CopyFieldsToTarget<T>(this T source, T target)
           where T : class, new()
        {
            Type type = typeof(T);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            if (target == null)
            {
                target = new T();
            }

            foreach (FieldInfo fieldInfo in fields)
            {
                fieldInfo.SetValue(target, fieldInfo.GetValue(source));
            }
        }
    }
}
