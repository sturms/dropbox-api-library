namespace DropboxApiLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Xml;
    using Models;
    using dropboxApi = global::Dropbox.Api;

    /// <summary>
    /// Dropbox API commonly used enpoint calls.
    /// </summary>
    public interface IDropboxManager
    {
        /// <summary>
        /// Loads the XML from dropbox.
        /// </summary>
        /// <remarks>
        /// https://content.dropboxapi.com/2/files/download?authorization=Bearer ACCESS_TOKEN&arg={"path": "/folder/filename.xml"}
        /// </remarks>
        /// <param name="svcUri">Service URI to XML file</param>
        /// <returns>Null if not exists or failed to load</returns>
        XmlDocument LoadXml(string svcUri);

        /// <summary>
        /// Retrieves all the files that are within Dropbox folder by given URI.
        /// </summary>
        /// <param name="svcFolderUri">Dropbox folder URI</param>
        /// <returns>Result that contains list with file metadata</returns>
        Task<dropboxApi.Files.ListFolderResult> GetFilesInFolder(string svcFolderUri);

        /// <summary>
        /// Gets the folder shared link which can be used, for example, 
        /// to download folder as .zip archive.
        /// </summary>
        /// <param name="svcUri">Dropbox folder URI</param>
        /// <returns>Folder shared link</returns>
        Task<string> GetFolderSharedLink(string svcUri);

        /// <summary>
        /// Creates a folder within specified Dropbox relative directory where the
        /// last part of this URI is the folder name.
        /// </summary>
        /// <param name="svcUri">Dropbox folder URI</param>
        /// <returns>Metadata for the created folder</returns>
        Task<dropboxApi.Files.CreateFolderResult> CreateFolder(string svcUri);

        /// <summary>
        /// Deletes file or folder by the specified Dropbox URI.
        /// </summary>
        /// <param name="svcUri">File or Folder URI</param>
        /// <returns>Deleted file or folder metadata</returns>
        Task<dropboxApi.Files.DeleteResult> DeleteFileOrFolder(string svcUri);

        /// <summary>
        /// Renames file or folder.
        /// </summary>
        /// <param name="oldSvcUri">Dropbox URI for the file or folder to rename</param>
        /// <param name="newSvcUri">The Dropbox URI for new file or folder name</param>
        /// <returns>Renamed file or folder metadata</returns>
        Task<dropboxApi.Files.RelocationResult> RenameFileOrFolder(string oldSvcUri, string newSvcUri);

        /// <summary>
        /// Uploads the files from PC local directory to Dropbox folder.
        /// </summary>
        /// <remarks>
        /// When providing the 'filePaths' list where the file sub directory does not exist in Dropbox,
        /// new folder will be automatically created in Dropbox.
        /// </remarks>
        /// <param name="svcPath">Dropbox folder URI where to upload files (Example: /app/UploadFolder)</param>
        /// <param name="localDirPath">Path from where to upload files</param>
        /// <param name="filePaths">File URIs (Example: [ "/app/UploadFolder/file.jpg", "/app/UploadFolder/SubFolder/file2.jpg" ] etc.)</param>
        /// <returns>List with uploaded file metadata</returns>
        Task<List<dropboxApi.Files.FileMetadata>> UploadFiles(string svcPath, string localDirPath, List<string> filePaths);

        /// <summary>
        /// Gets file or folder metadata (file size, creation time, extension etc.).
        /// </summary>
        /// <param name="svcUri">Dropbox URI for the file or folder</param>
        /// <returns>File or folder metadata</returns>
        Task<dropboxApi.Files.Metadata> GetFileOrFolderMetadata(string svcUri);

        /// <summary>
        /// Downloads a file by given Dropbox source URI to local directory.
        /// </summary>
        /// <param name="svcFileUri">Dropbox file URI</param>
        /// <param name="localFilePath">Local directory where to download file</param>
        /// <returns>True if file download went successfully, else False</returns>
        Task<bool> DownloadFile(string svcFileUri, string localFilePath);

        /// <summary>
        /// Downloads a folder as a .zip archive.
        /// </summary>
        /// <param name="svcUri">Dropbox folder URI</param>
        /// <param name="localFilePath">Local directory where to download the folder</param>
        /// <returns>True if folder download went successfully, else False</returns>
        Task<bool> DownloadFolder(string svcUri, string localFilePath);

        /// <summary>
        /// Downloads files or folders from given Dropbox URI to specified local directory.
        /// </summary>
        /// <param name="files">List with download file routes</param>
        /// <param name="CompletedCallback">Callback delegate that is called on operation completion</param>
        Task Download(List<FileRequestInfo> files, Action<bool, bool> CompletedCallback);
    }
}
