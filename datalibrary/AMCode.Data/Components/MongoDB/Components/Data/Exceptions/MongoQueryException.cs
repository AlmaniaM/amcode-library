using System;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Exception thrown when MongoDB query operations fail.
    /// </summary>
    public class MongoQueryException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the MongoQueryException class.
        /// </summary>
        public MongoQueryException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MongoQueryException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MongoQueryException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MongoQueryException class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public MongoQueryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
