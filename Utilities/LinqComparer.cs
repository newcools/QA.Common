// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinqComparer.cs" company="">
//   
// </copyright>
// <summary>
//   LINQ extensions with lambda comparers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Automation.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// LINQ extensions with lambda comparers. 
    /// </summary>
    public static class LinqComparer
    {
        #region Public Methods and Operators

        /// <summary>
        /// Determines whether a sequence contains a specified element by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/>.
        /// </summary>
        /// <returns>
        /// True if the source sequence contains an element that has the specified value; otherwise, false.
        /// </returns>
        /// <param name="source">
        /// A sequence in which to locate a value.
        /// </param>
        /// <param name="value">
        /// The value to locate in the sequence.
        /// </param>
        /// <param name="selectKey">
        /// The select Key.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <typeparam name="TCompareKey">
        /// The type of the key returned by <paramref name="selectKey"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        public static bool Contains<TSource, TCompareKey>(
            this IEnumerable<TSource> source, 
            TSource value, 
            Func<TSource, TCompareKey> selectKey)
        {
            return source.Contains(value, Create(selectKey));
        }

        /// <summary>
        /// Get an instance of <see cref="IComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The object type under comparison.
        /// </typeparam>
        /// <param name="compare">
        /// The compare.
        /// </param>
        /// <returns>
        /// The <see cref="IComparer{T}"/>.
        /// </returns>
        public static IComparer<T> Create<T>(Func<T, T, int> compare)
        {
            if (compare == null)
            {
                throw new ArgumentNullException("compare");
            }

            return new KeyComparer<T>(compare);
        }

        /// <summary>
        /// Create an instance of <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">
        /// The source type.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The key type.
        /// </typeparam>
        /// <param name="selectKey">
        /// The compare Key Selector.
        /// </param>
        /// <returns>
        /// The <see cref="IEqualityComparer{T}"/>.
        /// </returns>
        public static IEqualityComparer<TSource> Create<TSource, TKey>(Func<TSource, TKey> selectKey)
        {
            if (selectKey == null)
            {
                throw new ArgumentNullException("selectKey");
            }

            return Create<TSource>(
                (x, y) =>
                    {
                        if (ReferenceEquals(x, y))
                        {
                            return true;
                        }
                        
                        if (x == null || y == null)
                        {
                            return false;
                        }

                        return selectKey(x).Equals(selectKey(y));
                    }, 
                obj => obj == null ? 0 : selectKey(obj).GetHashCode());
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="equals">
        /// The equals.
        /// </param>
        /// <param name="getHashCode">
        /// The get hash code.
        /// </param>
        /// <typeparam name="T">
        /// The source type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEqualityComparer{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static IEqualityComparer<T> Create<T>(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            if (equals == null)
            {
                throw new ArgumentNullException("equals");
            }

            if (getHashCode == null)
            {
                throw new ArgumentNullException("getHashCode");
            }

            return new KeyEqualityComparer<T>(equals, getHashCode);
        }

        /// <summary>
        /// Returns distinct elements from a sequence by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare values.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> that contains distinct elements from the source sequence.
        /// </returns>
        /// <param name="source">
        /// The sequence to remove duplicate elements from.
        /// </param>
        /// <param name="selectKey">
        /// The select Key.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <typeparam name="TCompareKey">
        /// The type of the key returned by <paramref name="selectKey"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        public static IEnumerable<TSource> Distinct<TSource, TCompareKey>(
            this IEnumerable<TSource> source, 
            Func<TSource, TCompareKey> selectKey)
        {
            return source.Distinct(Create(selectKey));
        }

        /// <summary>
        /// Produces the set difference of two sequences by using the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare values.
        /// </summary>
        /// <returns>
        /// A sequence that contains the set difference of the elements of two sequences.
        /// </returns>
        /// <param name="first">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose elements that are not also in <paramref name="second"/> will be returned.
        /// </param>
        /// <param name="second">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose elements that also occur in the first sequence will cause those elements to be removed from the returned sequence.
        /// </param>
        /// <param name="selectKey">
        /// The select Key.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of the input sequences.
        /// </typeparam>
        /// <typeparam name="TCompareKey">
        /// The type of the key returned by <paramref name="selectKey"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="first"/> or <paramref name="second"/> is null.
        /// </exception>
        public static IEnumerable<TSource> Except<TSource, TCompareKey>(
            this IEnumerable<TSource> first, 
            IEnumerable<TSource> second, 
            Func<TSource, TCompareKey> selectKey)
        {
            return first.Except(second, Create(selectKey));
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. The keys are compared by using a specified comparer.
        /// </summary>
        /// <returns>
        /// A collection of elements of type <see cref="TResult"/> where each element represents a projection over a group and its key.
        /// </returns>
        /// <param name="source">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose elements to group.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract the key for each element.
        /// </param>
        /// <param name="resultSelector">
        /// A function to create a result value from each group.
        /// </param>
        /// <param name="selectKey">
        /// The select Key.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result value returned by <paramref name="resultSelector"/>.
        /// </typeparam>
        /// <typeparam name="TCompareKey">
        /// The type of the key returned by <paramref name="selectKey"/>.
        /// </typeparam>
        public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult, TCompareKey>(
            this IEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, 
            Func<TKey, IEnumerable<TSource>, TResult> resultSelector, 
            Func<TKey, TCompareKey> selectKey)
        {
            return source.GroupBy(keySelector, resultSelector, Create(selectKey));
        }

        /// <summary>
        /// Groups the elements of a sequence according to a key selector function. The keys are compared by using a comparer and each group's elements are projected by using a specified function.
        /// </summary>
        /// <returns>
        /// An IEnumerable&lt;IGrouping&lt;TKey, TElement&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TElement)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2"/> object contains a collection of objects of type <see cref="TElement"/> and a key.
        /// </returns>
        /// <param name="source">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose elements to group.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract the key for each element.
        /// </param>
        /// <param name="elementSelector">
        /// A function to map each source element to an element in an <see cref="T:System.Linq.IGrouping`2"/>.
        /// </param>
        /// <param name="selectKey">
        /// The select Key.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <typeparam name="TElement">
        /// The type of the elements in the <see cref="T:System.Linq.IGrouping`2"/>.
        /// </typeparam>
        /// <typeparam name="TCompareKey">
        /// The type of comparer returned by <paramref name="selectKey"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.
        /// </exception>
        public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement, TCompareKey>(
            this IEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, 
            Func<TSource, TElement> elementSelector, 
            Func<TKey, TCompareKey> selectKey)
        {
            return source.GroupBy(keySelector, elementSelector, Create(selectKey));
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. Key values are compared by using a specified comparer, and the elements of each group are projected by using a specified function.
        /// </summary>
        /// <returns>
        /// A collection of elements of type <see cref="TResult"/> name="TResult"/&gt; where each element represents a projection over a group and its key.
        /// </returns>
        /// <param name="source">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose elements to group.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract the key for each element.
        /// </param>
        /// <param name="elementSelector">
        /// A function to map each source element to an element in an <see cref="T:System.Linq.IGrouping`2"/>.
        /// </param>
        /// <param name="resultSelector">
        /// A function to create a result value from each group.
        /// </param>
        /// <param name="selectKey">
        /// The select Key.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <typeparam name="TElement">
        /// The type of the elements in each <see cref="T:System.Linq.IGrouping`2"/>.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result value returned by <paramref name="resultSelector"/>.
        /// </typeparam>
        /// <typeparam name="TCompareKey">
        /// The type of comparer returned by <paramref name="selectKey"/>.
        /// </typeparam>
        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult, TCompareKey>(
            this IEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, 
            Func<TSource, TElement> elementSelector, 
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector, 
            Func<TKey, TCompareKey> selectKey)
        {
            return source.GroupBy(keySelector, elementSelector, resultSelector, Create(selectKey));
        }

        /// <summary>
        /// Correlates the elements of two sequences based on key equality and groups the results. A specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> is used to compare keys.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> that contains elements of type <see cref="TResult"/> that are obtained by performing a grouped join on two sequences.
        /// </returns>
        /// <param name="outer">
        /// The first sequence to join.
        /// </param>
        /// <param name="inner">
        /// The sequence to join to the first sequence.
        /// </param>
        /// <param name="outerKeySelector">
        /// A function to extract the join key from each element of the first sequence.
        /// </param>
        /// <param name="innerKeySelector">
        /// A function to extract the join key from each element of the second sequence.
        /// </param>
        /// <param name="resultSelector">
        /// A function to create a result element from an element from the first sequence and a collection of matching elements from the second sequence.
        /// </param>
        /// <param name="selectKey">
        /// The select Key.
        /// </param>
        /// <typeparam name="TOuter">
        /// The type of the elements of the first sequence.
        /// </typeparam>
        /// <typeparam name="TInner">
        /// The type of the elements of the second sequence.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the keys returned by the key selector functions.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result elements.
        /// </typeparam>
        /// <typeparam name="TCompareKey">
        /// The type of comparer returned by <paramref name="selectKey"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="outer"/> or <paramref name="inner"/> or <paramref name="outerKeySelector"/> or <paramref name="innerKeySelector"/> or <paramref name="resultSelector"/> is null.
        /// </exception>
        public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult, TCompareKey>(
            this IEnumerable<TOuter> outer, 
            IEnumerable<TInner> inner, 
            Func<TOuter, TKey> outerKeySelector, 
            Func<TInner, TKey> innerKeySelector, 
            Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, 
            Func<TKey, TCompareKey> selectKey)
        {
            return outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, Create(selectKey));
        }

        /// <summary>
        /// Produces the set intersection of two sequences by using the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare values.
        /// </summary>
        /// <returns>
        /// A sequence that contains the elements that form the set intersection of two sequences.
        /// </returns>
        /// <param name="first">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose distinct elements that also appear in <paramref name="second"/> will be returned.
        /// </param>
        /// <param name="second">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose distinct elements that also appear in the first sequence will be returned.
        /// </param>
        /// <param name="selectKey">
        /// The select Key.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of the input sequences.
        /// </typeparam>
        /// <typeparam name="TCompareKey">
        /// The type of comparer returned by <paramref name="selectKey"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="first"/> or <paramref name="second"/> is null.
        /// </exception>
        public static IEnumerable<TSource> Intersect<TSource, TCompareKey>(
            this IEnumerable<TSource> first, 
            IEnumerable<TSource> second, 
            Func<TSource, TCompareKey> selectKey)
        {
            return first.Intersect(second, Create(selectKey));
        }

        /// <summary>
        /// Correlates the elements of two sequences based on matching keys. A specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> is used to compare keys.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> that has elements of type <see cref="TResult"/> that are obtained by performing an inner join on two sequences.
        /// </returns>
        /// <param name="outer">
        /// The first sequence to join.
        /// </param>
        /// <param name="inner">
        /// The sequence to join to the first sequence.
        /// </param>
        /// <param name="outerKeySelector">
        /// A function to extract the join key from each element of the first sequence.
        /// </param>
        /// <param name="innerKeySelector">
        /// A function to extract the join key from each element of the second sequence.
        /// </param>
        /// <param name="resultSelector">
        /// A function to create a result element from two matching elements.
        /// </param>
        /// <param name="selectKey">
        /// The select Key.
        /// </param>
        /// <typeparam name="TOuter">
        /// The type of the elements of the first sequence.
        /// </typeparam>
        /// <typeparam name="TInner">
        /// The type of the elements of the second sequence.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the keys returned by the key selector functions.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result elements.
        /// </typeparam>
        /// <typeparam name="TCompareKey">
        /// The type of comparer returned by <paramref name="selectKey"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="outer"/> or <paramref name="inner"/> or <paramref name="outerKeySelector"/> or <paramref name="innerKeySelector"/> or <paramref name="resultSelector"/> is null.
        /// </exception>
        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult, TCompareKey>(
            this IEnumerable<TOuter> outer, 
            IEnumerable<TInner> inner, 
            Func<TOuter, TKey> outerKeySelector, 
            Func<TInner, TKey> innerKeySelector, 
            Func<TOuter, TInner, TResult> resultSelector, 
            Func<TKey, TCompareKey> selectKey)
        {
            return outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector, Create(selectKey));
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <param name="source">
        /// A sequence of values.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract a key from an element.
        /// </param>
        /// <param name="compare">
        /// A function to create a Comparer.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IOrderedEnumerable{TSource}"/> whose elements are sorted according to a key.
        /// </returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
            this IEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, 
            Func<TKey, TKey, int> compare)
        {
            return source.OrderBy(keySelector, Create(compare));
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order by using a specified comparer.
        /// </summary>
        /// <param name="source">
        /// A sequence of values.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract a key from an element.
        /// </param>
        /// <param name="compare">
        /// A function to create a Comparer.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IOrderedEnumerable{TSource}"/> whose elements are sorted according to a key.
        /// </returns>
        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(
            this IEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, 
            Func<TKey, TKey, int> compare)
        {
            return source.OrderByDescending(keySelector, Create(compare));
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing their elements by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/>.
        /// </summary>
        /// <returns>
        /// True if the two source sequences are of equal length and their corresponding elements compare equal according to <see cref="TCompareKey"/>; otherwise, false.
        /// </returns>
        /// <param name="first">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> to compare to <paramref name="second"/>.
        /// </param>
        /// <param name="second">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> to compare to the first sequence.
        /// </param>
        /// <param name="selectKey">
        /// The select Key.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of the input sequences.
        /// </typeparam>
        /// <typeparam name="TCompareKey">
        /// The type of comparer returned by <paramref name="selectKey"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="first"/> or <paramref name="second"/> is null.
        /// </exception>
        public static bool SequenceEqual<TSource, TCompareKey>(
            this IEnumerable<TSource> first, 
            IEnumerable<TSource> second, 
            Func<TSource, TCompareKey> selectKey)
        {
            return first.SequenceEqual(second, Create(selectKey));
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to a key.
        /// </returns>
        /// <param name="source">
        /// An <see cref="T:System.Linq.IOrderedEnumerable`1"/> that contains elements to sort.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract a key from each element.
        /// </param>
        /// <param name="compare">
        /// The compare.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="keySelector"/> is null.
        /// </exception>
        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(
            this IOrderedEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, 
            Func<TKey, TKey, int> compare)
        {
            return source.ThenBy(keySelector, Create(compare));
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted in descending order according to a key.
        /// </returns>
        /// <param name="source">
        /// An <see cref="T:System.Linq.IOrderedEnumerable`1"/> that contains elements to sort.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract a key from each element.
        /// </param>
        /// <param name="compare">
        /// The compare.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="keySelector"/> is null.
        /// </exception>
        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(
            this IOrderedEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, 
            Func<TKey, TKey, int> compare)
        {
            return source.ThenByDescending(keySelector, Create(compare));
        }

        /// <summary>
        /// Creates a <see cref="T:System.Collections.Generic.Dictionary`2"/> from an <see cref="T:System.Collections.Generic.IEnumerable`1"/> according to a specified key selector function, a comparer, and an element selector function.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.Dictionary`2"/> that contains values of type <see cref="TElement"/> selected from the input sequence.
        /// </returns>
        /// <param name="source">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> to create a <see cref="T:System.Collections.Generic.Dictionary`2"/> from.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract a key from each element.
        /// </param>
        /// <param name="elementSelector">
        /// A transform function to produce a result element value from each element.
        /// </param>
        /// <param name="selectKey">
        /// The select Key.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <typeparam name="TElement">
        /// The type of the value returned by <paramref name="elementSelector"/>.
        /// </typeparam>
        /// <typeparam name="TCompareKey">
        /// The type of comparer returned by <paramref name="selectKey"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.-or-<paramref name="keySelector"/> produces a key that is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="keySelector"/> produces duplicate keys for two elements.
        /// </exception>
        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement, TCompareKey>(
            this IEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, 
            Func<TSource, TElement> elementSelector, 
            Func<TKey, TCompareKey> selectKey)
        {
            return source.ToDictionary(keySelector, elementSelector, Create(selectKey));
        }

        /// <summary>
        /// Creates a <see cref="T:System.Linq.Lookup`2"/> from an <see cref="T:System.Collections.Generic.IEnumerable`1"/> according to a specified key selector function, a comparer and an element selector function.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Linq.Lookup`2"/> that contains values of type <see cref="TElement"/> selected from the input sequence.
        /// </returns>
        /// <param name="source">
        /// The <see cref="T:System.Collections.Generic.IEnumerable`1"/> to create a <see cref="T:System.Linq.Lookup`2"/> from.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract a key from each element.
        /// </param>
        /// <param name="elementSelector">
        /// A transform function to produce a result element value from each element.
        /// </param>
        /// <param name="selectKey">
        /// The select Key.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <typeparam name="TElement">
        /// The type of the value returned by <paramref name="elementSelector"/>.
        /// </typeparam>
        /// <typeparam name="TCompareKey">
        /// The type of comparer returned by <paramref name="selectKey"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.
        /// </exception>
        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement, TCompareKey>(
            this IEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, 
            Func<TSource, TElement> elementSelector, 
            Func<TKey, TCompareKey> selectKey)
        {
            return source.ToLookup(keySelector, elementSelector, Create(selectKey));
        }

        /// <summary>
        /// Produces the set union of two sequences by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> that contains the elements from both input sequences, excluding duplicates.
        /// </returns>
        /// <param name="first">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose distinct elements form the first set for the union.
        /// </param>
        /// <param name="second">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose distinct elements form the second set for the union.
        /// </param>
        /// <param name="selectKey">
        /// The select Key.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of the input sequences.
        /// </typeparam>
        /// <typeparam name="TCompareKey">
        /// The type of comparer returned by <paramref name="selectKey"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="first"/> or <paramref name="second"/> is null.
        /// </exception>
        public static IEnumerable<TSource> Union<TSource, TCompareKey>(
            this IEnumerable<TSource> first, 
            IEnumerable<TSource> second, 
            Func<TSource, TCompareKey> selectKey)
        {
            return first.Union(second, Create(selectKey));
        }

        #endregion

        /// <summary>
        /// The comparer.
        /// </summary>
        /// <typeparam name="T">
        /// The source object type.
        /// </typeparam>
        private class KeyComparer<T> : IComparer<T>
        {
            #region Fields

            /// <summary>
            /// The compare.
            /// </summary>
            private readonly Func<T, T, int> compare;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initialises a new instance of the <see cref="KeyComparer{T}"/> class. 
            /// Initializes a new instance of the <see cref="KeyComparer{T}"/> class.
            /// </summary>
            /// <param name="compare">
            /// The compare.
            /// </param>
            public KeyComparer(Func<T, T, int> compare)
            {
                this.compare = compare;
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// The compare.
            /// </summary>
            /// <param name="x">
            /// The x.
            /// </param>
            /// <param name="y">
            /// The y.
            /// </param>
            /// <returns>
            /// The <see cref="int"/>.
            /// </returns>
            public int Compare(T x, T y)
            {
                return this.compare(x, y);
            }

            #endregion
        }

        /// <summary>
        /// The equality comparer.
        /// </summary>
        /// <typeparam name="T">
        /// The object type.
        /// </typeparam>
        private class KeyEqualityComparer<T> : IEqualityComparer<T>
        {
            #region Fields

            /// <summary>
            /// The equals.
            /// </summary>
            private readonly Func<T, T, bool> equals;

            /// <summary>
            /// The get hash code.
            /// </summary>
            private readonly Func<T, int> getHashCode;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initialises a new instance of the <see cref="KeyEqualityComparer{T}"/> class. 
            /// Initializes a new instance of the <see cref="KeyEqualityComparer{T}"/> class.
            /// </summary>
            /// <param name="equals">
            /// The equals.
            /// </param>
            /// <param name="getHashCode">
            /// The get hash code.
            /// </param>
            public KeyEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
            {
                this.equals = equals;
                this.getHashCode = getHashCode;
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// The equals.
            /// </summary>
            /// <param name="x">
            /// The x.
            /// </param>
            /// <param name="y">
            /// The y.
            /// </param>
            /// <returns>
            /// The <see cref="bool"/>.
            /// </returns>
            public bool Equals(T x, T y)
            {
                return this.@equals(x, y);
            }

            /// <summary>
            /// The get hash code.
            /// </summary>
            /// <param name="obj">
            /// The object.
            /// </param>
            /// <returns>
            /// The <see cref="int"/>.
            /// </returns>
            public int GetHashCode(T obj)
            {
                return this.getHashCode(obj);
            }

            #endregion
        }
    }
}