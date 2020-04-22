namespace DropboxApiLibrary.Extensions
{
    public static class ExtensionString
    {
        public static string ToUri(this string path)
        {
            // Adds forward slash if not already exists
            if (!path.StartsWith("/"))
            {
                path = string.Concat("/", path);
            }

            // Change all backslashes to forward slashes
            return path.Replace("\\", "/");
        }

        public static string ToPath(this string uri)
        {
            if (uri.StartsWith("/"))
            {
                // Removes forward slash at beginning
                uri = uri.Substring(1);
            }

            return uri.Replace("/", "\\");
        }
    }
}

