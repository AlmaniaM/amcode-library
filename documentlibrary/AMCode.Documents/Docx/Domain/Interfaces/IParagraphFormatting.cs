using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.Common.Enums;

namespace AMCode.Docx
{
    /// <summary>
    /// Domain interface for paragraph formatting operations
    /// </summary>
    public interface IParagraphFormatting
    {
        /// <summary>
        /// Paragraph alignment
        /// </summary>
        HorizontalAlignment Alignment { get; set; }
        
        /// <summary>
        /// Paragraph spacing before (in points)
        /// </summary>
        double SpacingBefore { get; set; }
        
        /// <summary>
        /// Paragraph spacing after (in points)
        /// </summary>
        double SpacingAfter { get; set; }
        
        /// <summary>
        /// Line spacing
        /// </summary>
        double LineSpacing { get; set; }
    }
}
