using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.Docx.Interfaces;
using System;
using AMCode.Documents.Common.Enums;
using AMCode.Documents.Common.Models;

namespace AMCode.Docx
{
    /// <summary>
    /// Builder interface for creating paragraphs with fluent API
    /// </summary>
    public interface IParagraphBuilder
    {
        /// <summary>
        /// Set the paragraph text
        /// </summary>
        /// <param name="text">The text content</param>
        /// <returns>The builder instance for chaining</returns>
        IParagraphBuilder WithText(string text);

        /// <summary>
        /// Set the paragraph alignment
        /// </summary>
        /// <param name="alignment">The alignment to set</param>
        /// <returns>The builder instance for chaining</returns>
        IParagraphBuilder WithAlignment(HorizontalAlignment alignment);

        /// <summary>
        /// Set paragraph spacing
        /// </summary>
        /// <param name="before">Spacing before in points</param>
        /// <param name="after">Spacing after in points</param>
        /// <param name="line">Line spacing</param>
        /// <returns>The builder instance for chaining</returns>
        IParagraphBuilder WithSpacing(double before, double after, double line);

        /// <summary>
        /// Set the default font style for the paragraph
        /// </summary>
        /// <param name="fontStyle">The font style to apply</param>
        /// <returns>The builder instance for chaining</returns>
        IParagraphBuilder WithFont(FontStyle fontStyle);

        /// <summary>
        /// Configure a run within the paragraph
        /// </summary>
        /// <param name="configureRun">Action to configure the run</param>
        /// <returns>The builder instance for chaining</returns>
        IParagraphBuilder WithRun(Action<IRunBuilder> configureRun);

        /// <summary>
        /// Add a line break to the paragraph
        /// </summary>
        /// <returns>The builder instance for chaining</returns>
        IParagraphBuilder WithLineBreak();

        /// <summary>
        /// Build the paragraph
        /// </summary>
        /// <returns>Result containing the created paragraph or error</returns>
        Result<IParagraph> Build();
    }
}
