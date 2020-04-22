namespace DropboxApiLibrary.Models
{
    using System;

    /// <summary>
    /// Information regarding file or folder.
    /// </summary>
    public class FileMetadataInfo
    {
        /// <summary>
        /// Initialize file or folder information.
        /// </summary>
        /// <exception cref="ArgumentNullException">If file/folder name is null or empty</exception>
        /// <param name="name">File or folder name</param>
        public FileMetadataInfo(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("File name cannot be null or empty!");
            }

            Name = name;
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        public bool IsFile
        {
            get
            {
                return System.IO.Path.HasExtension(this.Name);
            }
        }
    }
}
