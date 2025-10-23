using System;
using System.Collections.Concurrent;
using System.Threading;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Pdf;

namespace AMCode.Documents.Pdf.Infrastructure.Performance
{
    /// <summary>
    /// Memory-efficient PDF document factory with object pooling
    /// </summary>
    public class PdfMemoryEfficientFactory : IDisposable
    {
        private readonly ConcurrentQueue<PdfDocument> _documentPool;
        private readonly ConcurrentQueue<PdfPages> _pagesPool;
        private readonly ConcurrentQueue<PdfPage> _pagePool;
        private readonly PdfPerformanceOptimizer _optimizer;
        private readonly IPdfLogger _logger;
        private readonly IPdfValidator _validator;
        private readonly IPdfProvider _provider;
        private bool _disposed = false;
        private int _maxPoolSize = 50;

        /// <summary>
        /// Create memory-efficient factory
        /// </summary>
        public PdfMemoryEfficientFactory(IPdfProvider provider, IPdfLogger logger, IPdfValidator validator)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            
            _documentPool = new ConcurrentQueue<PdfDocument>();
            _pagesPool = new ConcurrentQueue<PdfPages>();
            _pagePool = new ConcurrentQueue<PdfPage>();
            _optimizer = new PdfPerformanceOptimizer(logger);
        }

        /// <summary>
        /// Create or reuse a PDF document
        /// </summary>
        public Result<IPdfDocument> CreateDocument()
        {
            if (_disposed)
            {
                return Result<IPdfDocument>.Failure("Factory has been disposed");
            }

            try
            {
                // Try to reuse from pool
                if (_documentPool.TryDequeue(out var pooledDocument))
                {
                    // Reset document for reuse
                    ResetDocument(pooledDocument);
                    _logger.LogDebug("Reused document from pool");
                    return Result<IPdfDocument>.Success(pooledDocument);
                }

                // Create new document using provider
                var result = _provider.CreateDocument();
                if (result.IsSuccess)
                {
                    _logger.LogDebug("Created new document");
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create document: {ex.Message}", ex);
                return Result<IPdfDocument>.Failure(ex.Message);
            }
        }

        /// <summary>
        /// Create or reuse a pages collection
        /// </summary>
        public IPages CreatePages()
        {
            if (_disposed)
            {
                return new PdfPages(); // Fallback to new instance
            }

            if (_pagesPool.TryDequeue(out var pooledPages))
            {
                ResetPages(pooledPages);
                _logger.LogDebug("Reused pages from pool");
                return pooledPages;
            }

            _logger.LogDebug("Created new pages collection");
            return new PdfPages();
        }

        /// <summary>
        /// Create or reuse a page
        /// </summary>
        public IPage CreatePage(IPdfDocument document, PageSize size = PageSize.A4, PageOrientation orientation = PageOrientation.Portrait)
        {
            if (_disposed)
            {
                return new PdfPage(1); // Fallback to new instance
            }

            if (_pagePool.TryDequeue(out var pooledPage))
            {
                ResetPage(pooledPage, document, size, orientation);
                _logger.LogDebug("Reused page from pool");
                return pooledPage;
            }

            _logger.LogDebug("Created new page");
            return new PdfPage(1);
        }

        /// <summary>
        /// Return document to pool for reuse
        /// </summary>
        public void ReturnDocument(IPdfDocument document)
        {
            if (_disposed || document == null)
            {
                return;
            }

            if (document is PdfDocument pdfDocument && _documentPool.Count < _maxPoolSize)
            {
                _documentPool.Enqueue(pdfDocument);
                _logger.LogDebug("Returned document to pool");
            }
        }

        /// <summary>
        /// Return pages to pool for reuse
        /// </summary>
        public void ReturnPages(IPages pages)
        {
            if (_disposed || pages == null)
            {
                return;
            }

            if (pages is PdfPages pdfPages && _pagesPool.Count < _maxPoolSize)
            {
                _pagesPool.Enqueue(pdfPages);
                _logger.LogDebug("Returned pages to pool");
            }
        }

        /// <summary>
        /// Return page to pool for reuse
        /// </summary>
        public void ReturnPage(IPage page)
        {
            if (_disposed || page == null)
            {
                return;
            }

            if (page is PdfPage pdfPage && _pagePool.Count < _maxPoolSize)
            {
                _pagePool.Enqueue(pdfPage);
                _logger.LogDebug("Returned page to pool");
            }
        }

        /// <summary>
        /// Reset document for reuse
        /// </summary>
        private void ResetDocument(PdfDocument document)
        {
            // Note: PdfDocument properties are read-only, so we can't reset them
            // The document will be recreated with new properties when needed
            
            // Clear pages collection
            if (document.Pages is PdfPages pdfPages)
            {
                pdfPages.Clear();
            }
        }

        /// <summary>
        /// Reset pages for reuse
        /// </summary>
        private void ResetPages(PdfPages pages)
        {
            pages.Clear();
        }

        /// <summary>
        /// Reset page for reuse
        /// </summary>
        private void ResetPage(PdfPage page, IPdfDocument document, PageSize size, PageOrientation orientation)
        {
            page.Document = document;
            page.Size = size;
            page.Orientation = orientation;
            // Note: PageNumber is read-only, so we can't reset it
            // It will be set by the pages collection when the page is added
            page.Margins = new Margins(72); // Default margins
        }

        /// <summary>
        /// Get performance statistics
        /// </summary>
        public PdfMemoryStats GetMemoryStats()
        {
            return new PdfMemoryStats
            {
                DocumentPoolSize = _documentPool.Count,
                PagesPoolSize = _pagesPool.Count,
                PagePoolSize = _pagePool.Count,
                MaxPoolSize = _maxPoolSize,
                OptimizerStats = _optimizer.GetStats(),
                IsDisposed = _disposed
            };
        }

        /// <summary>
        /// Set maximum pool size
        /// </summary>
        public void SetMaxPoolSize(int maxSize)
        {
            if (maxSize < 0)
            {
                throw new ArgumentException("Max pool size cannot be negative", nameof(maxSize));
            }
            
            _maxPoolSize = maxSize;
            _logger.LogInformation($"Set max pool size to {maxSize}");
        }

        /// <summary>
        /// Clear all pools
        /// </summary>
        public void ClearPools()
        {
            while (_documentPool.TryDequeue(out _)) { }
            while (_pagesPool.TryDequeue(out _)) { }
            while (_pagePool.TryDequeue(out _)) { }
            
            _logger.LogInformation("Cleared all object pools");
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                ClearPools();
                _optimizer?.Dispose();
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// Memory statistics
    /// </summary>
    public class PdfMemoryStats
    {
        public int DocumentPoolSize { get; set; }
        public int PagesPoolSize { get; set; }
        public int PagePoolSize { get; set; }
        public int MaxPoolSize { get; set; }
        public PdfPerformanceStats OptimizerStats { get; set; }
        public bool IsDisposed { get; set; }
    }
}
