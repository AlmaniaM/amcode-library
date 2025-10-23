using AMCode.Documents.Docx.Interfaces;
using System;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of IDocumentProperties
    /// </summary>
    public class OpenXmlDocumentProperties : IDocumentProperties
    {
        private readonly WordprocessingDocument _document;

        public string Title
        {
            get => GetCoreProperty("title");
            set => SetCoreProperty("title", value);
        }

        public string Subject
        {
            get => GetCoreProperty("subject");
            set => SetCoreProperty("subject", value);
        }

        public string Author
        {
            get => GetCoreProperty("creator");
            set => SetCoreProperty("creator", value);
        }

        public string Keywords
        {
            get => GetCoreProperty("keywords");
            set => SetCoreProperty("keywords", value);
        }

        public string Comments
        {
            get => GetCoreProperty("description");
            set => SetCoreProperty("description", value);
        }

        public string Category
        {
            get => GetCoreProperty("category");
            set => SetCoreProperty("category", value);
        }

        public string Company
        {
            get => GetCoreProperty("company");
            set => SetCoreProperty("company", value);
        }

        public string Manager
        {
            get => GetCoreProperty("manager");
            set => SetCoreProperty("manager", value);
        }

        public string Description
        {
            get => GetCoreProperty("description");
            set => SetCoreProperty("description", value);
        }

        public string Version
        {
            get => GetCoreProperty("version");
            set => SetCoreProperty("version", value);
        }

        public DateTime Created
        {
            get => GetCorePropertyDateTime("created");
            set => SetCorePropertyDateTime("created", value);
        }

        public DateTime Modified
        {
            get => GetCorePropertyDateTime("modified");
            set => SetCorePropertyDateTime("modified", value);
        }

        public DateTime LastPrinted
        {
            get => GetCorePropertyDateTime("lastPrinted");
            set => SetCorePropertyDateTime("lastPrinted", value);
        }

        public int Revision
        {
            get => int.TryParse(GetCoreProperty("revision"), out int revision) ? revision : 0;
            set => SetCoreProperty("revision", value.ToString());
        }

        public int PageCount => GetPageCount();
        public int WordCount => GetWordCount();
        public int CharacterCount => GetCharacterCount();

        public OpenXmlDocumentProperties(WordprocessingDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
        }

        private string GetCoreProperty(string propertyName)
        {
            try
            {
                var coreProperties = _document.PackageProperties;
                if (coreProperties == null) return string.Empty;

                return propertyName switch
                {
                    "title" => coreProperties.Title ?? string.Empty,
                    "subject" => coreProperties.Subject ?? string.Empty,
                    "creator" => coreProperties.Creator ?? string.Empty,
                    "keywords" => coreProperties.Keywords ?? string.Empty,
                    "description" => coreProperties.Description ?? string.Empty,
                    "category" => coreProperties.Category ?? string.Empty,
                    "company" => string.Empty, // Company not available in IPackageProperties
                    "manager" => string.Empty, // Manager not available in IPackageProperties
                    "version" => coreProperties.Version ?? string.Empty,
                    "revision" => coreProperties.Revision ?? string.Empty,
                    _ => string.Empty
                };
            }
            catch
            {
                return string.Empty;
            }
        }

        private void SetCoreProperty(string propertyName, string value)
        {
            try
            {
                var coreProperties = _document.PackageProperties;
                if (coreProperties == null) return;

                switch (propertyName)
                {
                    case "title":
                        coreProperties.Title = value;
                        break;
                    case "subject":
                        coreProperties.Subject = value;
                        break;
                    case "creator":
                        coreProperties.Creator = value;
                        break;
                    case "keywords":
                        coreProperties.Keywords = value;
                        break;
                    case "description":
                        coreProperties.Description = value;
                        break;
                    case "category":
                        coreProperties.Category = value;
                        break;
                    case "company":
                        // Company not available in IPackageProperties - ignore
                        break;
                    case "manager":
                        // Manager not available in IPackageProperties - ignore
                        break;
                    case "version":
                        coreProperties.Version = value;
                        break;
                    case "revision":
                        coreProperties.Revision = value;
                        break;
                }
            }
            catch
            {
                // Ignore errors when setting properties
            }
        }

        private DateTime GetCorePropertyDateTime(string propertyName)
        {
            try
            {
                var coreProperties = _document.PackageProperties;
                if (coreProperties == null) return DateTime.MinValue;

                return propertyName switch
                {
                    "created" => coreProperties.Created ?? DateTime.MinValue,
                    "modified" => coreProperties.Modified ?? DateTime.MinValue,
                    "lastPrinted" => coreProperties.LastPrinted ?? DateTime.MinValue,
                    _ => DateTime.MinValue
                };
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        private void SetCorePropertyDateTime(string propertyName, DateTime value)
        {
            try
            {
                var coreProperties = _document.PackageProperties;
                if (coreProperties == null) return;

                switch (propertyName)
                {
                    case "created":
                        coreProperties.Created = value;
                        break;
                    case "modified":
                        coreProperties.Modified = value;
                        break;
                    case "lastPrinted":
                        coreProperties.LastPrinted = value;
                        break;
                }
            }
            catch
            {
                // Ignore errors when setting properties
            }
        }

        private int GetPageCount()
        {
            try
            {
                // This is a simplified implementation
                // In a real implementation, you would need to calculate based on content
                var paragraphs = _document.MainDocumentPart?.Document?.Body?.Elements<Paragraph>();
                return paragraphs?.Count() ?? 0;
            }
            catch
            {
                return 0;
            }
        }

        private int GetWordCount()
        {
            try
            {
                var text = _document.MainDocumentPart?.Document?.Body?.InnerText ?? string.Empty;
                return text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            }
            catch
            {
                return 0;
            }
        }

        private int GetCharacterCount()
        {
            try
            {
                var text = _document.MainDocumentPart?.Document?.Body?.InnerText ?? string.Empty;
                return text.Length;
            }
            catch
            {
                return 0;
            }
        }
    }
}
