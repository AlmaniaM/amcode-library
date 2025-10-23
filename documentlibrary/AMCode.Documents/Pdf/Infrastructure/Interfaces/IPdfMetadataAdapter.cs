using System;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF metadata adapter interface
    /// </summary>
    public interface IPdfMetadataAdapter
    {
        /// <summary>
        /// Adapt metadata for specific provider
        /// </summary>
        Result<object> AdaptMetadata(IPdfMetadata metadata);
        
        /// <summary>
        /// Adapt properties for specific provider
        /// </summary>
        Result<object> AdaptProperties(IPdfProperties properties);
        
        /// <summary>
        /// Extract metadata from provider-specific object
        /// </summary>
        Result<IPdfMetadata> ExtractMetadata(object providerMetadata);
        
        /// <summary>
        /// Extract properties from provider-specific object
        /// </summary>
        Result<IPdfProperties> ExtractProperties(object providerProperties);
    }
}
