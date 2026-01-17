using System;

namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Metadata for creating a new workbook
    /// </summary>
    public class WorkbookCreationMetadata
    {
        /// <summary>
        /// Gets or sets the workbook title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the author name
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the company name
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the keywords
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the comments
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the manager name
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// Gets or sets the application name
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets the application version
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        /// Gets or sets the template path
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        public DateTime? Created { get; set; }
    }
}
