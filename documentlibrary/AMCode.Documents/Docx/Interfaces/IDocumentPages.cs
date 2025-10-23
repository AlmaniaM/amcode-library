namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Interface for document pages collection
    /// </summary>
    public interface IDocumentPages
    {
        /// <summary>
        /// Creates a new page
        /// </summary>
        /// <returns>New document page</returns>
        IDocumentPage Create();
    }
}
