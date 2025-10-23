using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.Docx.Interfaces;
using System;

namespace AMCode.Docx
{
    /// <summary>
    /// Main domain interface for document operations
    /// Combines content, metadata, and lifecycle concerns
    /// </summary>
    public interface IDocumentDomain : IDocumentContent, IDocumentMetadata, IDisposable
    {
        /// <summary>
        /// Document identifier
        /// </summary>
        Guid Id { get; }
        
        /// <summary>
        /// Document creation timestamp
        /// </summary>
        DateTime CreatedAt { get; }
        
        /// <summary>
        /// Document last modified timestamp
        /// </summary>
        DateTime LastModified { get; }
    }
}
