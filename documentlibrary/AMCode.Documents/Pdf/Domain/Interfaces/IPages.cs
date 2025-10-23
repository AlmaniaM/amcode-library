using System;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// Collection of PDF pages interface
    /// </summary>
    public interface IPages
    {
        /// <summary>
        /// Number of pages
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Get page by index
        /// </summary>
        IPage this[int index] { get; }
        
        /// <summary>
        /// Create a new page
        /// </summary>
        IPage Create(IPdfDocument document = null);
        
        /// <summary>
        /// Create a new page with specific size
        /// </summary>
        IPage Create(PageSize pageSize, IPdfDocument document = null);
        
        /// <summary>
        /// Remove a page
        /// </summary>
        void Remove(IPage page);
        
        /// <summary>
        /// Remove page by index
        /// </summary>
        void RemoveAt(int index);
        
        /// <summary>
        /// Clear all pages
        /// </summary>
        void Clear();
    }
}
