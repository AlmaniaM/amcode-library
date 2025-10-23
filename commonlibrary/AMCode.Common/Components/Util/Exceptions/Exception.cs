using System;
using AMCode.Common.Extensions.ReflectionMethodInfo;
using static AMCode.Common.Util.MethodInfoUtil;

namespace AMCode.Common.Util
{
    /// <summary>
    /// A class designed to hold helper functions for <see cref="Exception"/>s.
    /// </summary>
    public class ExceptionUtil
    {
        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a constructor.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateConstructorExceptionHeader<TConstructor>()
            => typeof(TConstructor).GetConstructor(new Type[] { }).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="TConstructor">Provide the class <see cref="Type"/> to generate the constructor header from.</typeparam>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateConstructorExceptionHeader<TConstructor, T>()
            => typeof(TConstructor).GetConstructor(new Type[] { typeof(T) }).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="TConstructor">Provide the class <see cref="Type"/> to generate the constructor header from.</typeparam>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateConstructorExceptionHeader<TConstructor, T1, T2>()
            => typeof(TConstructor).GetConstructor(new Type[] { typeof(T1), typeof(T2) }).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="TConstructor">Provide the class <see cref="Type"/> to generate the constructor header from.</typeparam>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateConstructorExceptionHeader<TConstructor, T1, T2, T3>()
            => typeof(TConstructor).GetConstructor(new Type[] { typeof(T1), typeof(T2), typeof(T3) }).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="TConstructor">Provide the class <see cref="Type"/> to generate the constructor header from.</typeparam>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateConstructorExceptionHeader<TConstructor, T1, T2, T3, T4>()
            => typeof(TConstructor).GetConstructor(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="TConstructor">Provide the class <see cref="Type"/> to generate the constructor header from.</typeparam>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateConstructorExceptionHeader<TConstructor, T1, T2, T3, T4, T5>()
            => typeof(TConstructor).GetConstructor(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="TConstructor">Provide the class <see cref="Type"/> to generate the constructor header from.</typeparam>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="T6">Parameter six type.</typeparam>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateConstructorExceptionHeader<TConstructor, T1, T2, T3, T4, T5, T6>()
            => typeof(TConstructor).GetConstructor(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) }).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="TConstructor">Provide the class <see cref="Type"/> to generate the constructor header from.</typeparam>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="T6">Parameter six type.</typeparam>
        /// <typeparam name="T7">Parameter seven type.</typeparam>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateConstructorExceptionHeader<TConstructor, T1, T2, T3, T4, T5, T6, T7>()
            => typeof(TConstructor).GetConstructor(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) }).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <param name="action">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader(Action action)
            => GetMethodInfo(action).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T>(Action<T> action)
            => GetMethodInfo(action).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2>(Action<T1, T2> action)
            => GetMethodInfo(action).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2, T3>(Action<T1, T2, T3> action)
            => GetMethodInfo(action).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
            => GetMethodInfo(action).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
            => GetMethodInfo(action).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="T6">Parameter six type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action)
            => GetMethodInfo(action).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="T6">Parameter six type.</typeparam>
        /// <typeparam name="T7">Parameter seven type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action)
            => GetMethodInfo(action).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="T6">Parameter six type.</typeparam>
        /// <typeparam name="T7">Parameter seven type.</typeparam>
        /// <typeparam name="T8">Parameter eight type.</typeparam>
        /// <param name="action">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
            => GetMethodInfo(action).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<TResult>(Func<TResult> func)
            => GetMethodInfo(func).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, TResult>(Func<T1, TResult> func)
            => GetMethodInfo(func).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2, TResult>(Func<T1, T2, TResult> func)
            => GetMethodInfo(func).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
            => GetMethodInfo(func).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func)
            => GetMethodInfo(func).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func)
            => GetMethodInfo(func).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="T6">Parameter six type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func)
            => GetMethodInfo(func).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
        /// </summary>
        /// <typeparam name="T1">Parameter one type.</typeparam>
        /// <typeparam name="T2">Parameter two type.</typeparam>
        /// <typeparam name="T3">Parameter three type.</typeparam>
        /// <typeparam name="T4">Parameter four type.</typeparam>
        /// <typeparam name="T5">Parameter five type.</typeparam>
        /// <typeparam name="T6">Parameter six type.</typeparam>
        /// <typeparam name="T7">Parameter seven type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
            => GetMethodInfo(func).CreateExceptionHeader();

        /// <summary>
        /// Create an <see cref="Exception"/> header information string about a method.
        /// Don't add parentheses when calling the method name.
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
        /// <param name="func">Provide the method to get build the <see cref="string"/> for.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
            => GetMethodInfo(func).CreateExceptionHeader();

        /// <summary>
        /// Get the method info from a delegate method call.
        /// </summary>
        /// <param name="del">A <see cref="Delegate"/> that calls a method name. Don't add parentheses.</param>
        /// <returns>A <see cref="string"/> function information header..</returns>
        public static string CreateExceptionHeader(Delegate del)
            => GetMethodInfo(del).CreateExceptionHeader();
    }
}