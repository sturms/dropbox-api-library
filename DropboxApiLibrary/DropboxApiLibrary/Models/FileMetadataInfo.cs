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
        /// The file or folder full path.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Flag to check whether this is a file not folder metadata info.
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
