using System;

namespace DemandLink.Storage
{
    /// <summary>
    /// A class designed to represent an exception when a file that you tried to 
    /// retrieve does not exist.
    /// </summary>
    public class FileDoesNotExistException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="FileDoesNotExistException"/> class.
        /// </summary>
        public FileDoesNotExistException()
            : base()
        { }

        /// <summary>
        /// Create an instance of the <see cref="FileDoesNotExistException"/> class.
        /// </summary>
        /// <param name="message">Add a string message.</param>
        public FileDoesNotExistException(string message)
            : base(message)
        { }

        /// <summary>
        /// Create an instance of the <see cref="FileDoesNotExistException"/> class.
        /// </summary>
        /// <param name="header">A string header representing the location of where the exception happened.</param>
        /// <param name="fileName">The file name that was attempted to be retrieved.</param>
        public FileDoesNotExistException(string header, string fileName)
            : base($"{header} Error: The specified file '{fileName}' does not exist.")
        { }
    }
}