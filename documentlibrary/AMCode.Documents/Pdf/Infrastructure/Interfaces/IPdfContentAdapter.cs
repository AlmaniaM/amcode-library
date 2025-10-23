using System;
using System.IO;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF content adapter interface
    /// </summary>
    public interface IPdfContentAdapter
    {
        /// <summary>
        /// Adapt content for specific provider
        /// </summary>
        Result<object> AdaptContent(IPdfContent content);
        
        /// <summary>
        /// Adapt page for specific provider
        /// </summary>
        Result<object> AdaptPage(IPage page);
        
        /// <summary>
        /// Adapt table for specific provider
        /// </summary>
        Result<object> AdaptTable(ITable table);
        
        /// <summary>
        /// Save adapted content to stream
        /// </summary>
        Result SaveAs(object adaptedContent, Stream stream);
        
        /// <summary>
        /// Save adapted content to file
        /// </summary>
        Result SaveAs(object adaptedContent, string filePath);
    }
}
