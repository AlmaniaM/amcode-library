using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AMCode.Common.IO
{
    /// <summary>
    /// A static class designed to container file path helper functions.
    /// </summary>
    public static class PathUtils
    {
        /// <summary>
        /// Combine a variable number of path fragments into a complete path.
        /// </summary>
        /// <param name="paths">Any number of file path fragments.</param>
        /// <returns>A <see cref="string"/> representing a file path.</returns>
        public static string CombinePaths(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
            {
                return string.Empty;
            }

            var wholePath = new List<string>();
            paths.Select(path => path.Replace(@"\", "/").Split('/').ToList())
                .ToList()
                .ForEach(pathList => wholePath.AddRange(pathList));
            return Path.Combine(wholePath.ToArray());
        }
    }
}