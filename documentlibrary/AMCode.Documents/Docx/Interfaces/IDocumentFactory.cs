using AMCode.Documents.Common.Models;
using System.Threading.Tasks;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Interface for document factory
    /// </summary>
    public interface IDocumentFactory
    {
        /// <summary>
        /// Creates a new document
        /// </summary>
        /// <returns>Result containing the document</returns>
        Result<IDocument> CreateDocument();
        
        /// <summary>
        /// Creates a new document with specified format
        /// </summary>
        /// <param name="format">Document format</param>
        /// <returns>Result containing the document</returns>
        Result<IDocument> CreateDocument(string format);
    }
}
