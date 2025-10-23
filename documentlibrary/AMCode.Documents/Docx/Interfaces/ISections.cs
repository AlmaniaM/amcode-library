using System.Collections.Generic;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Collection of document sections
    /// </summary>
    public interface ISections : IEnumerable<ISection>
    {
        /// <summary>
        /// Get section by index (0-based)
        /// </summary>
        ISection this[int index] { get; }
        
        /// <summary>
        /// Number of sections
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Parent document
        /// </summary>
        IDocument Document { get; }
        
        /// <summary>
        /// Create a new section
        /// </summary>
        ISection Create();
        
        /// <summary>
        /// Create a new section with page size
        /// </summary>
        ISection Create(PageSize pageSize);
        
        /// <summary>
        /// Remove section by index
        /// </summary>
        void Remove(int index);
        
        /// <summary>
        /// Remove section by reference
        /// </summary>
        void Remove(ISection section);
    }
}
