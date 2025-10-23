using System;

namespace DemandLink.Storage
{
    /// <summary>
    /// A class designed to represent an exception when a storage location that you do not have
    /// access to was attempted to be accessed.
    /// </summary>
    public class CannotAccessStorageException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="CannotAccessStorageException"/> class.
        /// </summary>
        public CannotAccessStorageException()
            : base()
        { }

        /// <summary>
        /// Create an instance of the <see cref="CannotAccessStorageException"/> class.
        /// </summary>
        /// <param name="message">Add a string message.</param>
        public CannotAccessStorageException(string message)
            : base(message)
        { }

        /// <summary>
        /// Create an instance of the <see cref="CannotAccessStorageException"/> class.
        /// </summary>
        /// <param name="header">A string header representing the location of where the exception happened.</param>
        /// <param name="message">Add a string message.</param>
        public CannotAccessStorageException(string header, string message)
            : base($"{header} Error: Cannot access the storage location. {message}")
        { }
    }
}