using System;

namespace AMCode.Common.Domain
{
    /// <summary>
    /// Base class for all domain-specific exceptions
    /// </summary>
    public abstract class DomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DomainException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        protected DomainException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DomainException class with a specified error message and a reference to the inner exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        protected DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when a business rule is violated
    /// </summary>
    public class BusinessRuleViolationException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the BusinessRuleViolationException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public BusinessRuleViolationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BusinessRuleViolationException class with a specified error message and a reference to the inner exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        public BusinessRuleViolationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when an entity is not found
    /// </summary>
    public class EntityNotFoundException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the EntityNotFoundException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public EntityNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the EntityNotFoundException class with a specified error message and a reference to the inner exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when an operation is attempted on an invalid entity state
    /// </summary>
    public class InvalidEntityStateException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the InvalidEntityStateException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public InvalidEntityStateException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidEntityStateException class with a specified error message and a reference to the inner exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        public InvalidEntityStateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when validation fails
    /// </summary>
    public class ValidationException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the ValidationException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public ValidationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ValidationException class with a specified error message and a reference to the inner exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

