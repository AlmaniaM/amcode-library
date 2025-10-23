using System.Collections.Generic;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Collection of paragraphs
    /// </summary>
    public interface IParagraphs : IEnumerable<IParagraph>
    {
        /// <summary>
        /// Get paragraph by index (0-based)
        /// </summary>
        IParagraph this[int index] { get; }
        
        /// <summary>
        /// Number of paragraphs
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Parent document
        /// </summary>
        IDocument Document { get; }
        
        /// <summary>
        /// Create a new paragraph
        /// </summary>
        IParagraph Create();
        
        /// <summary>
        /// Create a new paragraph with text
        /// </summary>
        IParagraph Create(string text);
        
        /// <summary>
        /// Remove paragraph by index
        /// </summary>
        void Remove(int index);
        
        /// <summary>
        /// Remove paragraph by reference
        /// </summary>
        void Remove(IParagraph paragraph);
    }
}
