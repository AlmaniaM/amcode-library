using System.Collections;
using System.Collections.Generic;
using AMCode.SyncfusionIo.Xlsx.Internal;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Extensions
{
    internal static class StylesExtensions
    {
        /// <summary>
        /// Convert a <see cref="Lib.IStyles"/> <see cref="IEnumerator"/> into an <see cref="IStyles"/> <see cref="IEnumerator{T}"/>.
        /// </summary>
        /// <param name="styles">A <see cref="Lib.IStyles"/> object.</param>
        /// <returns>An <see cref="IStyle"/> <see cref="IEnumerator{T}"/>.</returns>
        public static IEnumerator<IStyle> ToStyleEnumerator(this Lib.IStyles styles)
        {
            if (styles is null)
            {
                return default;
            }

            var stylesEnumerator = styles.GetEnumerator();
            var iStylesList = new List<IStyle>();

            while (stylesEnumerator.MoveNext())
            {
                iStylesList.Add(new Style((Lib.IStyle)stylesEnumerator.Current));
            }

            return iStylesList.GetEnumerator();
        }
    }
}