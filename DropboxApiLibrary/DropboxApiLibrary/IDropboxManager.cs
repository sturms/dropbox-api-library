namespace DropboxApiLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Xml;
    using Models;
    using dropboxApi = global::Dropbox.Api;

    /// <summary>
    /// TODO: Add comment
    /// </summary>
    public interface IDropboxManager
    {
        /// <summary>
        /// TODO: Add comment
        /// </summary>
        Task<dropboxApi.Files.ListFolderResult> GetFilesInFolder(string svcFolderUri);

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        Task<string> GetFolderSharedLink(string svcUri);

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        Task<dropboxApi.Files.CreateFolderResult> CreateFolder(string svcUri);

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        Task<dropboxApi.Files.DeleteResult> DeleteFileOrFolder(string svcUri);

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        Task<dropboxApi.Files.RelocationResult> RenameFileOrFolder(string oldSvcUri, string newSvcUri);

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        Task<List<dropboxApi.Files.FileMetadata>> UploadFiles(string svcPath, string localDirPath, List<string> filePaths);

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        Task<dropboxApi.Files.Metadata> GetFileOrFolderMetadata(string svcUri);

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        Task<bool> DownloadFile(string svcFileUri, string localFilePath);

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        Task<bool> DownloadFolder(string svcUri, string localFilePath);

        /// <summary>
        /// Downloads files or folders from given service dir to specified local directory
        /// </summary>
        /// <param name="files">List with download file routes</param>
        Task Download(List<FileRequestInfo> files, Action<bool, bool> CompletedCallback);

        /// <summary>
        /// Loads the xml from dropbox
        /// </summary>
        /// <remarks>
        /// https://content.dropboxapi.com/2/files/download?authorization=Bearer ACCESS_TOKEN&arg={"path": "/folder/filename.xml"}
        /// </remarks>
        /// <param name="svcUri">service uri to xml file</param>
        /// <returns>null if not exists or failed to load</returns>
        XmlDocument LoadXml(string svcUri);
    }
}
