using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Pdf;

namespace AMCode.Documents.Pdf.Infrastructure.Performance
{
    /// <summary>
    /// Performance optimizer for PDF operations
    /// </summary>
    public class PdfPerformanceOptimizer : IDisposable
    {
        private readonly ConcurrentQueue<PdfProperties> _propertiesPool;
        private readonly ConcurrentQueue<PdfMetadataAdapter> _metadataPool;
        private readonly ConcurrentDictionary<string, object> _cache;
        private readonly Timer _cleanupTimer;
        private readonly IPdfLogger _logger;
        private bool _disposed = false;

        /// <summary>
        /// Create performance optimizer
        /// </summary>
        public PdfPerformanceOptimizer(IPdfLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _propertiesPool = new ConcurrentQueue<PdfProperties>();
            _metadataPool = new ConcurrentQueue<PdfMetadataAdapter>();
            _cache = new ConcurrentDictionary<string, object>();
            
            // Setup cleanup timer to run every 5 minutes
            _cleanupTimer = new Timer(CleanupExpiredCache, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        }

        /// <summary>
        /// Get or create properties object
        /// </summary>
        public PdfProperties GetProperties(string title = "New Document", string author = "AMCode.Pdf")
        {
            if (_propertiesPool.TryDequeue(out var properties))
            {
                // Reset properties for reuse
                properties.Title = title;
                properties.Author = author;
                properties.CreationDate = DateTime.UtcNow;
                properties.ModificationDate = DateTime.UtcNow;
                return properties;
            }

            return new PdfProperties(title, author);
        }

        /// <summary>
        /// Return properties object to pool
        /// </summary>
        public void ReturnProperties(PdfProperties properties)
        {
            if (properties != null && _propertiesPool.Count < 100) // Limit pool size
            {
                _propertiesPool.Enqueue(properties);
            }
        }

        /// <summary>
        /// Get or create metadata adapter
        /// </summary>
        public PdfMetadataAdapter GetMetadataAdapter(PdfProperties properties)
        {
            // Since Properties is read-only, we need to create a new instance
            // TODO: Consider making PdfMetadataAdapter mutable for better performance
            return new PdfMetadataAdapter(properties);
        }

        /// <summary>
        /// Return metadata adapter to pool
        /// </summary>
        public void ReturnMetadataAdapter(PdfMetadataAdapter metadata)
        {
            if (metadata != null && _metadataPool.Count < 100) // Limit pool size
            {
                _metadataPool.Enqueue(metadata);
            }
        }

        /// <summary>
        /// Get cached object
        /// </summary>
        public T GetCached<T>(string key, Func<T> factory) where T : class
        {
            if (_cache.TryGetValue(key, out var cached) && cached is T result)
            {
                return result;
            }

            var newItem = factory();
            _cache.TryAdd(key, newItem);
            return newItem;
        }

        /// <summary>
        /// Cache an object
        /// </summary>
        public void Cache<T>(string key, T value) where T : class
        {
            _cache.AddOrUpdate(key, value, (k, v) => value);
        }

        /// <summary>
        /// Remove cached object
        /// </summary>
        public bool RemoveCached(string key)
        {
            return _cache.TryRemove(key, out _);
        }

        /// <summary>
        /// Cleanup expired cache entries
        /// </summary>
        private void CleanupExpiredCache(object state)
        {
            try
            {
                // Remove old cache entries (simple cleanup for now)
                if (_cache.Count > 1000)
                {
                    var keysToRemove = new List<string>();
                    var count = 0;
                    
                    foreach (var kvp in _cache)
                    {
                        if (count++ > 500) // Keep half of the cache
                        {
                            keysToRemove.Add(kvp.Key);
                        }
                    }

                    foreach (var key in keysToRemove)
                    {
                        _cache.TryRemove(key, out _);
                    }

                    _logger.LogDebug($"Cleaned up {keysToRemove.Count} cache entries");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during cache cleanup: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get performance statistics
        /// </summary>
        public PdfPerformanceStats GetStats()
        {
            return new PdfPerformanceStats
            {
                PropertiesPoolSize = _propertiesPool.Count,
                MetadataPoolSize = _metadataPool.Count,
                CacheSize = _cache.Count,
                IsDisposed = _disposed
            };
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _cleanupTimer?.Dispose();
                _cache.Clear();
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// Performance statistics
    /// </summary>
    public class PdfPerformanceStats
    {
        public int PropertiesPoolSize { get; set; }
        public int MetadataPoolSize { get; set; }
        public int CacheSize { get; set; }
        public bool IsDisposed { get; set; }
    }
}
