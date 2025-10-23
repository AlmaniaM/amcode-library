using System;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Exception thrown when MongoDB document transformation operations fail.
    /// </summary>
    public class MongoTransformationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the MongoTransformationException class.
        /// </summary>
        public MongoTransformationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MongoTransformationException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MongoTransformationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MongoTransformationException class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public MongoTransformationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
