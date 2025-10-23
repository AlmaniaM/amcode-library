using System.Threading.Tasks;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Interface for document page
    /// </summary>
    public interface IDocumentPage
    {
        /// <summary>
        /// Adds text to the page
        /// </summary>
        /// <param name="text">Text to add</param>
        /// <param name="font">Font name</param>
        /// <param name="size">Font size</param>
        /// <param name="color">Text color</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        void AddText(string text, string font, int size, string color, int x, int y);
        
        /// <summary>
        /// Saves the page as a stream
        /// </summary>
        /// <returns>Task containing the stream</returns>
        Task<System.IO.Stream> SaveAsStreamAsync();
    }
}
