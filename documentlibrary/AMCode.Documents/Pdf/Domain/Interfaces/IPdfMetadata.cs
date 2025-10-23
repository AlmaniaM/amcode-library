using System;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// Document metadata interface
    /// </summary>
    public interface IPdfMetadata
    {
        /// <summary>
        /// Document properties
        /// </summary>
        IPdfProperties Properties { get; }
        
        /// <summary>
        /// Close metadata and cleanup resources
        /// </summary>
        void Close();
    }
}
