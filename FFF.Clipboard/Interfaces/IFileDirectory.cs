namespace FFF.Interfaces
{
    public interface IFileDirectory
    {
        /// <summary>
        /// Determines whether a file or directory exists at the specified path.
        /// </summary>
        /// <param name="filePath">The path to the file or directory to check for existence.</param>
        /// <returns>
        /// true if the file or directory exists at the specified path; otherwise, false.
        /// </returns>
        bool IsExists(string filePath);
    }
}
