namespace DwgCommon.DropboxAPI.Test
{
    using DropboxApiLibrary;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DropboxApiXmlFilesTest : DropboxApiTestHelper
    {
        private IDropboxManager _dropboxMgr;

        [TestInitialize]
        public void Initialize()
        {
            _dropboxMgr = new DropboxManager();
        }

        [TestMethod]
        public void DropboxApiXmlFilesTest_LoadExistingXMLFile_NotNullReturned()
        {
            var responseXMLDocument = _dropboxMgr.LoadXml(base.GetUri("/NotEmptyFolder/Sample.xml"));
            responseXMLDocument.Should().NotBeNull();
        }

        [TestMethod]
        public void DropboxApiXmlFilesTest_LoadNonExistingXMLFile_NullReturned()
        {
            var responseXMLDocument = _dropboxMgr.LoadXml(base.GetUri("/NotEmptyFolder/NonExistingFile.xml"));
            responseXMLDocument.Should().BeNull();
        }
    }
}
