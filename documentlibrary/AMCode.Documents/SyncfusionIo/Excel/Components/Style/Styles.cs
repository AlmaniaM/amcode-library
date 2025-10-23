using System;
using System.Collections;
using AMCode.SyncfusionIo.Xlsx.Extensions;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Internal
{
    /// <summary>
    /// A class designed to act as a collection of <see cref="IStyle"/>s.
    /// </summary>
    public class Styles : IStyles, IInternalStyles
    {
        /// <summary>
        /// Create an instance of the <see cref="Styles"/> class.
        /// </summary>
        /// <param name="libStyles">Provide an instance of the <see cref="Lib.IStyles"/> object.</param>
        internal Styles(Lib.IStyles libStyles)
        {
            InnerLibStyles = libStyles;
        }

        /// <inheritdoc/>
        public IStyle this[int index] => GetStyle(index);

        /// <inheritdoc/>
        public IStyle this[string name] => GetStyle(name);

        /// <inheritdoc/>
        public int Count => InnerLibStyles.Count;

        /// <inheritdoc/>
        public Lib.IStyles InnerLibStyles { get; }

        /// <inheritdoc/>
        public IStyle Add(string name) => new Style(InnerLibStyles.Add(name));

        /// <inheritdoc/>
        public bool Contains(string name) => InnerLibStyles.Contains(name);

        /// <inheritdoc/>
        public IEnumerator GetEnumerator() => InnerLibStyles.ToStyleEnumerator();

        /// <inheritdoc/>
        public IStyle GetStyle(int index)
        {
            try
            {
                return new Style(InnerLibStyles[index]);
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <inheritdoc/>
        public IStyle GetStyle(string name)
        {
            try
            {
                return new Style(InnerLibStyles[name]);
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <inheritdoc/>
        public void Remove(string styleName) => InnerLibStyles.Remove(styleName);
    }
}