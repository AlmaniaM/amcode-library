using System;

namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Contains information about a workbook without opening it
    /// </summary>
    public class WorkbookInfo
    {
        /// <summary>
        /// Gets or sets the workbook title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the number of worksheets
        /// </summary>
        public int WorksheetCount { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Gets or sets the last modified date
        /// </summary>
        public DateTime? Modified { get; set; }

        /// <summary>
        /// Gets or sets the file size in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the workbook is password protected
        /// </summary>
        public bool IsProtected { get; set; }
    }
}
