namespace DwgCommon.DropboxAPI.Test
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DropboxApiLibrary;
    using DropboxApiLibrary.Extensions;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Integration tests which verifies Dropbox API calls
    /// to check whether it is possible to create new folders, files etc.
    /// </summary>
    [TestClass]
    public class DropboxApiFilesTest : DropboxApiTestHelper
    {
        private IDropboxManager _dropboxMgr;

        [TestInitialize]
        public void Initialize()
        {
            _dropboxMgr = new DropboxManager();
        }

        [TestMethod]
        public async Task DropboxApiFilesTest_GetFileMetadata_NotNullReturned()
        {
            string uri = base.GetUri("/NotEmptyFolder/Sample.xml");
            var result = await _dropboxMgr.GetFileOrFolderMetadata(uri);
            result.Should().NotBeNull();
            result.AsFile.Rev.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public async Task DropboxApiFilesTest_UploadFileAndDeleteItAfterwards_UploadFileResultEqualsToOne()
        {
            string uri = base.GetUri("/NotEmptyFolder");
            string fileRelativePath = "SampleFolder\\SampleTextFile.txt";
            string resourcesFolderRootDir = base.GetResourcesFolderDir();
            var result = await _dropboxMgr.UploadFiles(uri, resourcesFolderRootDir, new List<string> { fileRelativePath });
            result.Count.Should().Be(1);

            // Deletes the uploaded file
            await DeleteFileOrFolder_IsSuccess(uri + fileRelativePath.ToUri());

            // After that deletes also its parent folder 'SampleFolder'
            await DeleteFileOrFolder_IsSuccess(uri + "/SampleFolder");
        }

        [TestMethod]
        public async Task DropboxApiFilesTest_RenameFileAndThenChangeBackToPreviousName_IsRenamed()
        {
            string oldUri = base.GetUri("/NotEmptyFolder/Sample.xml");
            string newUri = base.GetUri("/NotEmptyFolder/SampleRenamed.xml");

            await this.RenameFile_IsRenamed(oldUri, newUri);

            // Rename to previous name to not fail other tests
            await this.RenameFile_IsRenamed(newUri, oldUri);
        }

        [TestMethod]
        public async Task DropboxApiFilesTest_TryPerformingUploadFileOperationWithoutAnyFiles_UploadFilesResultShouldBeNull()
        {
            string uri = base.GetUri("/NotEmptyFolder");
            string resourcesRootDir = base.GetResourcesFolderDir();
            var result = await _dropboxMgr.UploadFiles(uri, resourcesRootDir, new List<string> { /* Empty list not allowed */ });
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task DropboxApiFilesTest_DownloadFile_FileDownloaded()
        {
            string uri = base.GetUri("/NotEmptyFolder/Sample.xml");
            string resourcesRootDir = base.GetResourcesFolderDir();
            string localFilePath = System.IO.Path.Combine(resourcesRootDir, "Sample.xml");
            var result = await _dropboxMgr.DownloadFile(uri, localFilePath);
            result.Should().Be(true);

            // Delete file locally
            System.IO.File.Delete(localFilePath);
        }

        private async Task DeleteFileOrFolder_IsSuccess(string svcUri)
        {
            var result = await _dropboxMgr.DeleteFileOrFolder(svcUri);
            result.Should().NotBeNull();
        }

        private async Task RenameFile_IsRenamed(string fileOldUriName, string fileNewUriName)
        {
            var result = await _dropboxMgr.RenameFileOrFolder(fileOldUriName, fileNewUriName);
            result.Should().NotBeNull();
            result.Metadata.PathDisplay.Should().Be(fileNewUriName);
        }
    }
}
