using System;
using System.Reflection;

namespace AMCode.Common.Util
{
    /// <summary>
    /// Utility class for retrieving <see cref="MethodInfo"/> objects.
    /// </summary>
    public static class MethodInfoUtil
    {
        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <param name="action">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo(Action action) => action.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T>(Action<T> action) => action.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2>(Action<T1, T2> action) => action.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2, T3>(Action<T1, T2, T3> action) => action.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action) => action.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action) => action.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="T6">Parameter six type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action) => action.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="T6">Parameter six type.</typeparam>
        /// <typeparam name="T7">Parameter seven type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action) => action.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="T6">Parameter six type.</typeparam>
        /// <typeparam name="T7">Parameter seven type.</typeparam>
        /// <typeparam name="T8">Parameter eight type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action) => action.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<TResult>(Func<TResult> func) => func.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, TResult>(Func<T1, TResult> func) => func.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2, TResult>(Func<T1, T2, TResult> func) => func.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func) => func.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func) => func.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func) => func.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="T6">Parameter six type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func) => func.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="T6">Parameter six type.</typeparam>
        /// <typeparam name="T7">Parameter seven type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func) => func.Method;

        /// <summary>
        /// Get the method info of a method name call. Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="T6">Parameter six type.</typeparam>
        /// <typeparam name="T7">Parameter seven type.</typeparam>
        /// <typeparam name="T8">Parameter eight type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="MethodInfo"/> for.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func) => func.Method;

        /// <summary>
        /// Get the method info from a delegate method call.
        /// </summary>
        /// <param name="del">A <see cref="Delegate"/> that calls a method name. Don't add parentheses.</param>
        /// <returns>A <see cref="MethodInfo"/> object.</returns>
        public static MethodInfo GetMethodInfo(Delegate del) => del.Method;
    }
}