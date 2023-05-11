namespace FFF.Helpers
{
    internal static class DirectoryHelper
    {
        public static bool IsExists(string filePath) =>
            System.IO.Directory.Exists(ResolveDirectoryLongPath(filePath));

        private static string ResolveDirectoryLongPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return path;
            if (path.Length > 247 && !path.StartsWith(@"\\")) return $@"\\?\{path}";
            else return path;
        }
    }
}
