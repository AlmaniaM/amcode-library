using System;
using System.Linq;
using System.Reflection;
using System.Text;
using static AMCode.Common.Util.MethodInfoUtil;

namespace AMCode.Common.Extensions.ReflectionMethodInfo
{
    /// <summary>
    /// A static class designed to hold all extension methods for the <see cref="MethodBase"/> class.
    /// </summary>
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Create an <see cref="Exception"/> header of the provided <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="constructorInfo">The <see cref="ConstructorInfo"/> to get the header info from.</param>
        /// <returns>A <see cref="string"/> which indicates what <c>class</c>
        /// the method belongs to and what parameter types it has</returns>
        public static string CreateExceptionHeader(this ConstructorInfo constructorInfo)
        {
            if (constructorInfo is null)
            {
                var mInfo = GetMethodInfo<ConstructorInfo, string>(CreateExceptionHeader);
                var header = createExceptionHeader(mInfo);
                throw new NullReferenceException($"{header} Error: {nameof(ConstructorInfo)} cannot be null.");
            }

            return createExceptionHeader(constructorInfo);
        }

        /// <summary>
        /// Create an <see cref="Exception"/> header of the provided <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> to get the header info from.</param>
        /// <returns>A <see cref="string"/> which indicates what <c>class</c>
        /// the method belongs to and what parameter types it has</returns>
        public static string CreateExceptionHeader(this MethodInfo methodInfo)
        {
            if (methodInfo is null)
            {
                var mInfo = GetMethodInfo<MethodInfo, string>(CreateExceptionHeader);
                var header = createExceptionHeader(mInfo);
                throw new NullReferenceException($"{header} Error: {nameof(MethodInfo)} cannot be null.");
            }

            return createExceptionHeader(methodInfo);
        }

        /// <summary>
        /// Create an <see cref="Exception"/> header of the provided <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="constructorInfo">The <see cref="ConstructorInfo"/> to get the header info from.</param>
        /// <returns>A <see cref="string"/> which indicates what <c>class</c>
        /// the method belongs to and what parameter types it has</returns>
        private static string createExceptionHeader(ConstructorInfo constructorInfo)
            => createExceptionHeader(constructorInfo, constructorInfo.DeclaringType.Name, constructorInfo.DeclaringType.Name);

        /// <summary>
        /// Create an <see cref="Exception"/> header of the provided <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> to get the header info from.</param>
        /// <returns>A <see cref="string"/> which indicates what <c>class</c>
        /// the method belongs to and what parameter types it has</returns>
        private static string createExceptionHeader(MethodInfo methodInfo)
            => createExceptionHeader(methodInfo, methodInfo.DeclaringType.Name, methodInfo.Name);

        /// <summary>
        /// Create an <see cref="Exception"/> header of the provided <see cref="MethodBase"/>.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodBase"/> to get the header info from.</param>
        /// <param name="className">The <see cref="string"/> name of the class.</param>
        /// <param name="methodName">The <see cref="string"/> name of the method.</param>
        /// <returns>A <see cref="string"/> which indicates what <c>class</c>
        /// the method belongs to and what parameter types it has</returns>
        private static string createExceptionHeader(MethodBase methodInfo, string className, string methodName)
        {
            var parameters = methodInfo.GetParameters()
                .Select(parameter => $"{parameter.ParameterType.Name} {parameter.Name}");

            return new StringBuilder()
                .Append($"[{className}][{methodName}]")
                .Append($"({string.Join(", ", parameters)})")
                .ToString();
        }
    }
}