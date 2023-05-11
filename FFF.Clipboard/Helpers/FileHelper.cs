namespace FFF.Helpers
{
    internal static class FileHelper
    {
        public static bool IsExists(string filePath) =>
            System.IO.File.Exists(ResolveLongPath(filePath));

        private static string ResolveLongPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return path;
            if (path.Length > 259 && !path.StartsWith(@"\\")) return $@"\\?\{path}";
            else return path;
        }
    }
}
