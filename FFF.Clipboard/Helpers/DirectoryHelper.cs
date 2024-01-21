using FFF.Interfaces;
using System.IO;

namespace FFF.Helpers
{
    internal class DirectoryHelper : IFileDirectory
    {
        // Constants for path length limits
        private const int MaxPathLength = 247;

        /// <summary>
        /// Checks if the specified directory exists.
        /// </summary>
        /// <param name="filePath">The file path to check.</param>
        /// <returns>True if the directory exists; otherwise, false.</returns>
        public static bool IsExists(string filePath)
        {
            try
            {
                return Directory.Exists(ResolveDirectoryLongPath(filePath));
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
        /// Resolves the directory path to handle long paths.
        /// </summary>
        /// <param name="path">The original file path.</param>
        /// <returns>The resolved file path.</returns>
        private static string ResolveDirectoryLongPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return path;
            if (path.Length > MaxPathLength && !path.StartsWith(@"\\"))
                return $@"\\?\{path}";
            else return path;
        }
    }
}
