namespace DwgCommon.DropboxAPI.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DropboxApiLibrary;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Integration tests which verifies Dropbox API calls
    /// to check whether it is possible to create new folders, files etc.
    /// </summary>
    [TestClass]
    public class DropboxApiFoldersTest : DropboxApiTestHelper
    {
        private IDropboxManager _dropboxMgr;

        [TestInitialize]
        public void Initialize()
        {
            _dropboxMgr = new DropboxManager();
        }

        [TestMethod]
        public async Task DropboxApiFoldersTest_GetFolderMetadata_NotNullReturned()
        {
            string uri = base.GetUri("/EmptyFolder");
            var result = await _dropboxMgr.GetFileOrFolderMetadata(uri);
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task DropboxApiFoldersTest_GetSubDirectories_NotNullReturned()
        {
            string uri = base.GetUri("/EmptyFolder");
            var result = await _dropboxMgr.GetFilesInFolder(uri);
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task DropboxApiFoldersTest_GetSharedFolderUrl_UrlNotNull()
        {
            string uri = base.GetUri("/EmptyFolder");
            string sharedLink = await _dropboxMgr.GetFolderSharedLink(uri);
            sharedLink.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public async Task DropboxApiFoldersTest_GetSubDirectoriesFromEmptyFolder_CountEqualsToZero()
        {
            string uri = base.GetUri("/EmptyFolder");
            var result = await _dropboxMgr.GetFilesInFolder(uri);
            result.Should().NotBeNull();
            result.Entries.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task DropboxApiFoldersTest_GetSubDirectoriesFromNotEmptyFolder_DirectoriesFound()
        {
            string uri = base.GetUri("/NotEmptyFolder");
            var result = await _dropboxMgr.GetFilesInFolder(uri);
            result.Should().NotBeNull();
            result.Entries.Where(x => x.IsFolder).Count().Should().Be(1);
        }

        [TestMethod]
        public async Task DropboxApiFoldersTest_GetSubDirectoriesFromNotEmptyFolder_FilesFound()
        {
            string uri = base.GetUri("/NotEmptyFolder");
            var result = await _dropboxMgr.GetFilesInFolder(uri);
            result.Should().NotBeNull();
            result.Entries.Where(x => x.IsFile).Count().Should().Be(1);
        }

        [TestMethod]
        public async Task DropboxApiFoldersTest_CreateNewFolderWithNoExistingNameAndDeleteItAfterwards_IsSuccess()
        {
            string uri = base.GetUri("/NotEmptyFolder/Folder2");
            await this.CreateNewFolder_NotNull(uri);
            await this.DeleteFolder_IsDeleted(uri);
        }

        [TestMethod]
        public async Task DropboxApiFoldersTest_RenameFolderAndThenChangeBackToPreviousName_IsRenamed()
        {
            string oldUri = base.GetUri("/NotEmptyFolder/Folder1");
            string newUri = base.GetUri("/NotEmptyFolder/Folder1Renamed");

            await this.RenameFolder_IsRenamed(oldUri, newUri);

            // Rename to previous name to not fail other tests
            await this.RenameFolder_IsRenamed(newUri, oldUri);
        }

        [TestMethod]
        public async Task DropboxApiFoldersTest_TryToUploadFolderWithItsContents_UploadResultCountShouldBeZero()
        {
            string uri = base.GetUri("/NotEmptyFolder");
            string resourcesFolderRootDir = base.GetResourcesFolderDir();
            var result = await _dropboxMgr.UploadFiles(uri, resourcesFolderRootDir, new List<string> { "SampleFolder" });
            result.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task DropboxApiFoldersTest_DownloadFolderAsZipArchive_ArchiveDownloaded()
        {
            string uri = base.GetUri("/NotEmptyFolder");
            string resourcesRootDir = base.GetResourcesFolderDir();
            bool result = await _dropboxMgr.DownloadFolder(uri, resourcesRootDir);
            result.Should().Be(true);

            // Delete file locally
            System.IO.File.Delete(System.IO.Path.Combine(resourcesRootDir, "NotEmptyFolder.zip"));
        }

        private async Task CreateNewFolder_NotNull(string folderUri)
        {
            var result = await _dropboxMgr.CreateFolder(folderUri);
            result.Should().NotBeNull();
        }

        private async Task DeleteFolder_IsDeleted(string folderUri)
        {
            var result = await _dropboxMgr.DeleteFileOrFolder(folderUri);
            result.Should().NotBeNull();
        }

        private async Task RenameFolder_IsRenamed(string folderOldUriName, string folderNewUriName)
        {
            var result = await _dropboxMgr.RenameFileOrFolder(folderOldUriName, folderNewUriName);
            result.Should().NotBeNull();
            result.Metadata.PathDisplay.Should().Be(folderNewUriName);
        }
    }
}
