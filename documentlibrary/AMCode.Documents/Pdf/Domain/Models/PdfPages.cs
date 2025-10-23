using System;
using System.Collections.Generic;
using System.Linq;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// Collection of PDF pages implementation
    /// </summary>
    public class PdfPages : IPages
    {
        private readonly List<IPage> _pages = new List<IPage>();

        /// <summary>
        /// Number of pages
        /// </summary>
        public int Count => _pages.Count;

        /// <summary>
        /// Get page by index
        /// </summary>
        public IPage this[int index] => _pages[index];

        /// <summary>
        /// Create PDF pages collection
        /// </summary>
        public PdfPages()
        {
        }

        /// <summary>
        /// Create a new page
        /// </summary>
        public IPage Create(IPdfDocument document = null)
        {
            var page = new PdfPage(_pages.Count + 1)
            {
                Document = document
            };
            _pages.Add(page);
            return page;
        }

        /// <summary>
        /// Create a new page with specific size
        /// </summary>
        public IPage Create(PageSize pageSize, IPdfDocument document = null)
        {
            var page = new PdfPage(_pages.Count + 1)
            {
                Size = pageSize,
                Document = document
            };
            _pages.Add(page);
            return page;
        }

        /// <summary>
        /// Set document reference for all pages
        /// </summary>
        public void SetDocumentReference(IPdfDocument document)
        {
            foreach (var page in _pages)
            {
                if (page is PdfPage pdfPage)
                {
                    pdfPage.Document = document;
                }
            }
        }

        /// <summary>
        /// Remove a page
        /// </summary>
        public void Remove(IPage page)
        {
            if (page == null)
                return;

            _pages.Remove(page);
            UpdatePageNumbers();
        }

        /// <summary>
        /// Remove page by index
        /// </summary>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _pages.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            _pages.RemoveAt(index);
            UpdatePageNumbers();
        }

        /// <summary>
        /// Clear all pages
        /// </summary>
        public void Clear()
        {
            _pages.Clear();
        }

        private void UpdatePageNumbers()
        {
            for (int i = 0; i < _pages.Count; i++)
            {
                if (_pages[i] is PdfPage pdfPage)
                {
                    // Update page number - this would need to be implemented in PdfPage
                    // For now, we'll just ensure the collection is consistent
                }
            }
        }
    }
}
