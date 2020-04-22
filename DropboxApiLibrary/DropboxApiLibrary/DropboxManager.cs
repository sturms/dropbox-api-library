namespace DropboxApiLibrary
{
    using Extensions;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Xml;
    using dropboxApi = global::Dropbox.Api;

    /// <summary>
    /// TODO: Add comment
    /// </summary>
    public class DropboxManager : IDropboxManager
    {
        private const string ACCESS_TOKEN = "<MY_ACCESS_TOKEN>"; // Set your access token here (it is quite long string)
        private const string APP_ROOT_URI = "/Apps/MyFolder"; // Set your application root folder name

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        public static Dictionary<string, FileMetadataInfo> FilesToDownload = new Dictionary<string, FileMetadataInfo>();

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        public static int FilesToDownloadCount { get; set; }

        /// <summary>
        /// Returns access token
        /// </summary>
        public static string AccessToken
        {
            get
            {
                return ACCESS_TOKEN;
            }
        }

        /// <summary>
        /// The root path where DwgOperations app files are stored.
        /// </summary>
        public static string AppRootUri
        {
            get
            {
                return APP_ROOT_URI;
            }
        }

        /// <summary>
        /// Loads the xml from dropbox
        /// </summary>
        /// <remarks>
        /// https://content.dropboxapi.com/2/files/download?authorization=Bearer ACCESS_TOKEN&arg={"path": "/folder/filename.xml"}
        /// </remarks>
        /// <param name="svcUri">service uri to xml file</param>
        /// <returns>null if not exists or failed to load</returns>
        public XmlDocument LoadXml(string svcUri)
        {
            svcUri = svcUri.ToUri();
            XmlDocument xmlDoc = new XmlDocument();
            string uri = new Uri(string.Format("https://content.dropboxapi.com/2/files/download?authorization=Bearer%20{1}&arg=%7B%22path%22%3A%22{0}%22%7D", svcUri, ACCESS_TOKEN)).AbsoluteUri;

            try
            {
                xmlDoc.Load(uri);
                return xmlDoc;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="svcFolderUri"></param>
        /// <returns></returns>
        public async Task<dropboxApi.Files.ListFolderResult> GetFilesInFolder(string svcFolderUri)
        {
            try
            {
                dropboxApi.Files.ListFolderResult result = null;

                using (var client = new dropboxApi.DropboxClient(AccessToken))
                {
                    result = await client.Files.ListFolderAsync(svcFolderUri);
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="svcUri"></param>
        /// <returns></returns>
        public async Task<dropboxApi.Files.Metadata> GetFileOrFolderMetadata(string svcUri)
        {
            try
            {
                dropboxApi.Files.Metadata result = null;

                using (var client = new dropboxApi.DropboxClient(AccessToken))
                {
                    result = await client.Files.GetMetadataAsync(svcUri);
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="svcUri"></param>
        /// <returns></returns>
        public async Task<string> GetFolderSharedLink(string svcUri)
        {
            try
            {
                dropboxApi.Sharing.ListSharedLinksResult result = null;

                using (var client = new dropboxApi.DropboxClient(AccessToken))
                {
                    result = await client.Sharing.ListSharedLinksAsync(svcUri, directOnly: true);
                    if (result.Links.Count == 0)
                    {
                        var sharedLinkMeta = await client.Sharing.CreateSharedLinkWithSettingsAsync(svcUri);
                        return sharedLinkMeta.Url;
                    }
                }

                return result.Links[0].Url;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="svcUri"></param>
        /// <returns></returns>
        public async Task<dropboxApi.Files.CreateFolderResult> CreateFolder(string svcUri)
        {
            try
            {
                dropboxApi.Files.CreateFolderResult result = null;

                using (var client = new dropboxApi.DropboxClient(AccessToken))
                {
                    result = await client.Files.CreateFolderV2Async(svcUri);
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="svcUri"></param>
        /// <returns></returns>
        public async Task<dropboxApi.Files.DeleteResult> DeleteFileOrFolder(string svcUri)
        {
            try
            {
                dropboxApi.Files.DeleteResult result = null;

                using (var client = new dropboxApi.DropboxClient(AccessToken))
                {
                    result = await client.Files.DeleteV2Async(svcUri);
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="svcUri"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
        public async Task<dropboxApi.Files.FileMetadata> RestoreFileOrFolder(string svcUri, string revision)
        {
            try
            {
                dropboxApi.Files.FileMetadata result = null;

                using (var client = new dropboxApi.DropboxClient(AccessToken))
                {
                    result = await client.Files.RestoreAsync(svcUri, revision);
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="oldSvcUri"></param>
        /// <param name="newSvcUri"></param>
        /// <returns></returns>
        public async Task<dropboxApi.Files.RelocationResult> RenameFileOrFolder(string oldSvcUri, string newSvcUri)
        {
            try
            {
                dropboxApi.Files.RelocationResult result = null;

                using (var client = new dropboxApi.DropboxClient(AccessToken))
                {
                    result = await client.Files.MoveV2Async(oldSvcUri, newSvcUri, autorename: false);
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="svcPath"></param>
        /// <param name="localDirPath"></param>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        public async Task<List<dropboxApi.Files.FileMetadata>> UploadFiles(string svcPath, string localDirPath, List<string> filePaths)
        {
            var uploadResults = new List<dropboxApi.Files.FileMetadata>();

            try
            {
                if (filePaths == null || filePaths.Count() == 0)
                {
                    throw new ArgumentNullException("UploadFiles: files list was empty");
                }

                foreach (var relativePath in filePaths)
                {
                    string fullLocalDir = Path.Combine(localDirPath, relativePath);
                    string fullSvcUri = svcPath.ToUri() + relativePath.ToUri();

                    if (File.Exists(fullLocalDir))
                    {
                        using (var client = new dropboxApi.DropboxClient(AccessToken))
                        using (Stream fileStream = File.OpenRead(fullLocalDir))
                        {
                            var result = await client.Files.UploadAsync(fullSvcUri, body: fileStream);
                            uploadResults.Add(result);
                        }
                    }
                    else
                    {
                        throw new FileNotFoundException($"DropboxManager: File with name: {relativePath} does not exists!");
                    }
                }

                return uploadResults;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Downloads files or folders from given service dir to specified local directory
        /// </summary>
        /// <param name="files">List with download file routes</param>
        public async Task Download(List<FileRequestInfo> files, Action<bool, bool> CompletedCallback)
        {
            // Resets download files count
            FilesToDownloadCount = files.Count;

            foreach (var item in files)
            {
                Directory.CreateDirectory(item.GetLocalFileFullPathWithoutFileName());
                var fileMetadata = new FileMetadataInfo(item.GetLocalFileFullPath());
                FilesToDownload.Add(item.GetServiceFileFullUri(), fileMetadata);
            }

            for (int i = 0; i < FilesToDownload.Count; i++)
            {
                KeyValuePair<string, FileMetadataInfo> itemToDownload = FilesToDownload.ElementAt(i);
                bool isDownloadSuccessful;
                if (itemToDownload.Value.IsFile)
                {
                    isDownloadSuccessful = await DownloadFile(itemToDownload.Key, itemToDownload.Value.Name);
                }
                else
                {
                    isDownloadSuccessful = await DownloadFolder(itemToDownload.Key, itemToDownload.Value.Name);
                }

                CompletedCallback.Invoke(isDownloadSuccessful, false);
            }
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="svcUri"></param>
        /// <param name="localFilePath"></param>
        /// <returns></returns>
        public async Task<bool> DownloadFolder(string svcUri, string localFilePath)
        {
            try
            {
                string sharedFolderUrl = await this.GetFolderSharedLink(svcUri);
                string fullPath = Path.Combine(localFilePath, $"{Path.GetFileName(svcUri)}.zip");

                using (var webClient = new WebClient())
                {
                    await Task.Run(() =>
                    {
                        // dl=1 flag shows that folder will be downloaded as .zip archieve
                        webClient.DownloadFile(new Uri(sharedFolderUrl.Replace("dl=0", "dl=1")), fullPath);
                    });
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="svcFileUri"></param>
        /// <param name="localFilePath"></param>
        /// <returns></returns>
        public async Task<bool> DownloadFile(string svcFileUri, string localFilePath)
        {
            try
            {
                using (var client = new dropboxApi.DropboxClient(AccessToken))
                {
                    dropboxApi.Files.Metadata fileMetadata = await this.GetFileOrFolderMetadata(svcFileUri);
                    var result = await client.Files.DownloadAsync(svcFileUri);

                    using (Stream sourceStream = await result.GetContentAsStreamAsync())
                    using (FileStream source = File.Open(localFilePath, FileMode.Create))
                    {
                        await sourceStream.CopyToAsync(source);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
