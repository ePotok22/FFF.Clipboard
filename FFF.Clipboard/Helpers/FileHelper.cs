using System.IO;
using FFF.Interfaces;

namespace FFF.Helpers
{
    internal class FileHelper : IFileDirectory
    {
        // Constant for the maximum path length
        private const int MaxPathLength = 259;

        /// <summary>
        /// Checks if the specified file exists.
        /// </summary>
        /// <param name="filePath">The file path to check.</param>
        /// <returns>True if the file exists; otherwise, false.</returns>
        public static bool IsExists(string filePath)
        {
            try
            {
                return File.Exists(ResolveLongPath(filePath));
            }
            catch
            {
                // Handle or log the exception as needed
                return false;
            }
        }

        bool IFileDirectory.IsExists(string filePath) =>
            IsExists(filePath);

        /// <summary>
        /// Resolves the file path to handle long paths.
        /// </summary>
        /// <param name="path">The original file path.</param>
        /// <returns>The resolved file path.</returns>
        private static string ResolveLongPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return path;
            if (path.Length > MaxPathLength && !path.StartsWith(@"\\"))
                return $@"\\?\{path}";
            else return path;
        }
    }
}
