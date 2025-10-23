using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Docx
{
    /// <summary>
    /// Builder interface for creating runs with fluent API
    /// </summary>
    public interface IRunBuilder
    {
        /// <summary>
        /// Set the run text
        /// </summary>
        /// <param name="text">The text content</param>
        /// <returns>The builder instance for chaining</returns>
        IRunBuilder WithText(string text);

        /// <summary>
        /// Set the run font style
        /// </summary>
        /// <param name="fontStyle">The font style to apply</param>
        /// <returns>The builder instance for chaining</returns>
        IRunBuilder WithFont(FontStyle fontStyle);

        /// <summary>
        /// Set the run to bold
        /// </summary>
        /// <param name="bold">Whether the text should be bold</param>
        /// <returns>The builder instance for chaining</returns>
        IRunBuilder WithBold(bool bold = true);

        /// <summary>
        /// Set the run to italic
        /// </summary>
        /// <param name="italic">Whether the text should be italic</param>
        /// <returns>The builder instance for chaining</returns>
        IRunBuilder WithItalic(bool italic = true);

        /// <summary>
        /// Set the run to underlined
        /// </summary>
        /// <param name="underline">Whether the text should be underlined</param>
        /// <returns>The builder instance for chaining</returns>
        IRunBuilder WithUnderline(bool underline = true);

        /// <summary>
        /// Set the run font size
        /// </summary>
        /// <param name="fontSize">The font size in points</param>
        /// <returns>The builder instance for chaining</returns>
        IRunBuilder WithFontSize(double fontSize);

        /// <summary>
        /// Set the run font color
        /// </summary>
        /// <param name="color">The font color</param>
        /// <returns>The builder instance for chaining</returns>
        IRunBuilder WithColor(Color color);

        /// <summary>
        /// Build the run
        /// </summary>
        /// <returns>Result containing the created run or error</returns>
        Result<IRun> Build();
    }
}
