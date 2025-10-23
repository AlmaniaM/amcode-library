using System.Collections;
using System.Collections.Generic;
using AMCode.SyncfusionIo.Xlsx.Internal;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Extensions
{
    internal static class BordersExtensions
    {
        /// <summary>
        /// Convert a <see cref="Lib.IBorders"/> <see cref="IEnumerator"/> into an <see cref="IBorder"/> <see cref="IEnumerator{T}"/>.
        /// </summary>
        /// <param name="borders">A <see cref="Lib.IBorders"/> object.</param>
        /// <returns>An <see cref="IBorder"/> <see cref="IEnumerator{T}"/>.</returns>
        public static IEnumerator<IBorder> ToBordersEnumerator(this Lib.IBorders borders)
        {
            if (borders is null)
            {
                return default;
            }

            var bordersEnumerator = borders.GetEnumerator();
            var ibordersList = new List<IBorder>();

            while (bordersEnumerator.MoveNext())
            {
                ibordersList.Add(new Border((Lib.IBorder)bordersEnumerator.Current));
            }

            return ibordersList.GetEnumerator();
        }
    }
}