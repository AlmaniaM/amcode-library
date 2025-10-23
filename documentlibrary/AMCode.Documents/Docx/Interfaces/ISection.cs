using System;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Represents a document section
    /// </summary>
    public interface ISection
    {
        /// <summary>
        /// Section page size
        /// </summary>
        PageSize PageSize { get; set; }
        
        /// <summary>
        /// Section margins
        /// </summary>
        Margins Margins { get; set; }
        
        /// <summary>
        /// Section orientation
        /// </summary>
        PageOrientation Orientation { get; set; }
        
        /// <summary>
        /// Section index (0-based)
        /// </summary>
        int Index { get; }
        
        /// <summary>
        /// Parent sections collection
        /// </summary>
        ISections Sections { get; }
        
        /// <summary>
        /// Set page size
        /// </summary>
        void SetPageSize(PageSize pageSize);
        
        /// <summary>
        /// Set margins
        /// </summary>
        void SetMargins(Margins margins);
        
        /// <summary>
        /// Set orientation
        /// </summary>
        void SetOrientation(PageOrientation orientation);
    }
    
    /// <summary>
    /// Page orientation options
    /// </summary>
    public enum PageOrientation
    {
        Portrait = 0,
        Landscape = 1
    }
}
