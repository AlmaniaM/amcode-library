using System.Collections.Generic;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Collection of runs (text segments) in a paragraph
    /// </summary>
    public interface IRuns : IEnumerable<IRun>
    {
        /// <summary>
        /// Get run by index (0-based)
        /// </summary>
        IRun this[int index] { get; }
        
        /// <summary>
        /// Number of runs
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Parent paragraph
        /// </summary>
        IParagraph Paragraph { get; }
        
        /// <summary>
        /// Create a new run with text
        /// </summary>
        IRun Create(string text);
        
        /// <summary>
        /// Create a new run with text and formatting
        /// </summary>
        IRun Create(string text, FontStyle fontStyle);
        
        /// <summary>
        /// Remove run by index
        /// </summary>
        void Remove(int index);
        
        /// <summary>
        /// Remove run by reference
        /// </summary>
        void Remove(IRun run);
    }
}
