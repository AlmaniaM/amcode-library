using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// Fluent PDF document builder
    /// </summary>
    public class PdfBuilder : IPdfBuilder
    {
        private readonly IPdfProvider _provider;
        private IPdfProperties _properties;
        private readonly List<Action<IPdfDocument>> _pageActions = new();

        /// <summary>
        /// Create PDF builder
        /// </summary>
        public PdfBuilder(IPdfProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <summary>
        /// Set document title
        /// </summary>
        public IPdfBuilder WithTitle(string title)
        {
            if (_properties == null)
                _properties = new PdfProperties();
            
            _properties.Title = title ?? string.Empty;
            return this;
        }

        /// <summary>
        /// Set document author
        /// </summary>
        public IPdfBuilder WithAuthor(string author)
        {
            if (_properties == null)
                _properties = new PdfProperties();
            
            _properties.Author = author ?? string.Empty;
            return this;
        }

        /// <summary>
        /// Set document subject
        /// </summary>
        public IPdfBuilder WithSubject(string subject)
        {
            if (_properties == null)
                _properties = new PdfProperties();
            
            _properties.Subject = subject ?? string.Empty;
            return this;
        }

        /// <summary>
        /// Set document keywords
        /// </summary>
        public IPdfBuilder WithKeywords(string keywords)
        {
            if (_properties == null)
                _properties = new PdfProperties();
            
            _properties.Keywords = keywords ?? string.Empty;
            return this;
        }

        /// <summary>
        /// Set document creator
        /// </summary>
        public IPdfBuilder WithCreator(string creator)
        {
            if (_properties == null)
                _properties = new PdfProperties();
            
            _properties.Creator = creator ?? string.Empty;
            return this;
        }

        /// <summary>
        /// Set document producer
        /// </summary>
        public IPdfBuilder WithProducer(string producer)
        {
            if (_properties == null)
                _properties = new PdfProperties();
            
            _properties.Producer = producer ?? string.Empty;
            return this;
        }

        /// <summary>
        /// Add a page with default configuration
        /// </summary>
        public IPdfBuilder WithPage(Action<IPage> configurePage)
        {
            if (configurePage == null)
                throw new ArgumentNullException(nameof(configurePage));

            _pageActions.Add(document => 
            {
                var page = document.Pages.Create();
                configurePage(page);
            });
            return this;
        }

        /// <summary>
        /// Add a page with specific size
        /// </summary>
        public IPdfBuilder WithPage(PageSize pageSize, Action<IPage> configurePage)
        {
            if (configurePage == null)
                throw new ArgumentNullException(nameof(configurePage));

            _pageActions.Add(document => 
            {
                var page = document.Pages.Create(pageSize);
                configurePage(page);
            });
            return this;
        }

        /// <summary>
        /// Add multiple pages
        /// </summary>
        public IPdfBuilder WithPages(int count, Action<IPage> configurePage)
        {
            if (count <= 0)
                throw new ArgumentException("Page count must be greater than 0", nameof(count));
            if (configurePage == null)
                throw new ArgumentNullException(nameof(configurePage));

            for (int i = 0; i < count; i++)
            {
                WithPage(configurePage);
            }
            return this;
        }

        /// <summary>
        /// Add multiple pages with specific size
        /// </summary>
        public IPdfBuilder WithPages(int count, PageSize pageSize, Action<IPage> configurePage)
        {
            if (count <= 0)
                throw new ArgumentException("Page count must be greater than 0", nameof(count));
            if (configurePage == null)
                throw new ArgumentNullException(nameof(configurePage));

            for (int i = 0; i < count; i++)
            {
                WithPage(pageSize, configurePage);
            }
            return this;
        }

        /// <summary>
        /// Build the PDF document
        /// </summary>
        public Result<IPdfDocument> Build()
        {
            try
            {
                var result = _properties != null 
                    ? _provider.CreateDocument(_properties)
                    : _provider.CreateDocument();

                if (!result.IsSuccess)
                    return result;

                var document = result.Value;
                
                foreach (var action in _pageActions)
                {
                    action(document);
                }

                return Result<IPdfDocument>.Success(document);
            }
            catch (Exception ex)
            {
                return Result<IPdfDocument>.Failure(ex.Message, ex);
            }
        }
    }
}
