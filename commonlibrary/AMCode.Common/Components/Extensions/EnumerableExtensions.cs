using System;
using System.Collections.Generic;
using System.Linq;

namespace AMCode.Common.Extensions.Enumerables
{
    /// <summary>
    /// A static class designed to only hold Extension Methods for the <see cref="IEnumerable{T}"/> <see cref="Type"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Loop through the <see cref="IEnumerable{T}"/> and execute a provided action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action">An <see cref="Action"/> predicate to execute on each element.</param>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action) => list.ForEach((T t, int i) => action(t));

        /// <summary>
        /// Loop through the <see cref="IEnumerable{T}"/> and execute a provided action. This version
        /// provides you with an <see cref="int"/> index in the <see cref="Action"/> predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action">An <see cref="Action"/> predicate to execute on each element.</param>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T, int> action)
        {
            if (list != null && list.Any())
            {
                var count = list.Count();

                for (var i = 0; i < count; i++)
                {
                    action(list.ElementAt(i), i);
                }
            }
        }

        /// <summary>
        /// Loop through the <see cref="Array"/> and execute a provided action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action">An <see cref="Action"/> predicate to execute on each element.</param>
        public static void ForEach<T>(this T[] list, Action<T> action) => list.ForEach((T t, int i) => action(t));

        /// <summary>
        /// Loop through the <see cref="Array"/> and execute a provided action. This version
        /// provides you with an <see cref="int"/> index in the <see cref="Action"/> predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action">An <see cref="Action"/> predicate to execute on each element.</param>
        public static void ForEach<T>(this T[] list, Action<T, int> action)
        {
            if (list != null && list.Any())
            {
                for (var i = 0; i < list.Length; i++)
                {
                    action(list[i], i);
                }
            }
        }

        /// <summary>
        /// Loop through the <see cref="IList{T}"/> and execute a provided action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action">An <see cref="Action"/> predicate to execute on each element.</param>
        public static void ForEach<T>(this IList<T> list, Action<T> action) => list.ForEach((T t, int i) => action(t));

        /// <summary>
        /// Loop through the <see cref="IList{T}"/> and execute a provided action. This version
        /// provides you with an <see cref="int"/> index in the <see cref="Action"/> predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action">An <see cref="Action"/> predicate to execute on each element.</param>
        public static void ForEach<T>(this IList<T> list, Action<T, int> action)
        {
            if (list != null && list.Any())
            {
                for (var i = 0; i < list.Count; i++)
                {
                    action(list[i], i);
                }
            }
        }
    }
}