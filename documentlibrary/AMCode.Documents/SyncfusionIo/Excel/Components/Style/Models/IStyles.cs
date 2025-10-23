using System.Collections;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// An interface that represents a collection of <see cref="IStyle"/> objects.
    /// </summary>
    public interface IStyles : IEnumerable
    {
        /// <summary>
        /// Get a single <see cref="IStyle"/> object from the <see cref="IStyles"/> collection.
        /// </summary>
        /// <param name="index">The index of the <see cref="IStyle"/> object.</param>
        /// <returns>An <see cref="IStyle"/> object.</returns>
        IStyle this[int index] { get; }

        /// <summary>
        /// Returns a single <see cref="IStyle"/> object from the <see cref="IStyles"/> collection.
        /// </summary>
        /// <param name="name">The name of the <see cref="IStyle"/> object.</param>
        /// <returns>An <see cref="IStyle"/> object.</returns>
        IStyle this[string name] { get; }

        /// <summary>
        /// Get the number of <see cref="IStyle"/> objects in the <see cref="IStyles"/> collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Creates a new <see cref="IStyle"/> and adds it to the list of <see cref="IStyles"/>
        /// that are available for the current workbook.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A newly created <see cref="IStyle"/> object.</returns>
        IStyle Add(string name);

        /// <summary>
        /// Check if the given style name exists in the collection.
        /// </summary>
        /// <param name="name">The name of the <see cref="IStyle"/>.</param>
        /// <returns>A <c>true</c> if the style exists and <c>false</c> if it doesn't.</returns>
        bool Contains(string name);

        /// <summary>
        /// Get a single <see cref="IStyle"/> object from the <see cref="IStyles"/> collection.
        /// </summary>
        /// <param name="index">The index of the <see cref="IStyle"/> object.</param>
        /// <returns>An <see cref="IStyle"/> object.</returns>
        IStyle GetStyle(int index);

        /// <summary>
        /// Returns a single <see cref="IStyle"/> object from the <see cref="IStyles"/> collection.
        /// </summary>
        /// <param name="name">The name of the <see cref="IStyle"/> object.</param>
        /// <returns>An <see cref="IStyle"/> object.</returns>
        IStyle GetStyle(string name);

        /// <summary>
        /// Remove an <see cref="IStyle"/>.
        /// </summary>
        /// <param name="styleName">The name of the <see cref="IStyle"/>.</param>
        void Remove(string styleName);
    }

    internal interface IInternalStyles : IStyles
    {
        /// <summary>
        /// Get the underlying <see cref="Lib.IStyles"/> object.
        /// </summary>
        Lib.IStyles InnerLibStyles { get; }
    }
}