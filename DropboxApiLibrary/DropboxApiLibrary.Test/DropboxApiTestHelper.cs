namespace DwgCommon.DropboxAPI.Test
{
    using DropboxApiLibrary;
    using DropboxApiLibrary.Extensions;
    using System;

    /// <summary>
    /// Just a helper class for the tests.
    /// </summary>
    public class DropboxApiTestHelper
    {
        private const string TestingFolderRelativeUri = "/Testing";

        /// <summary>
        /// To make sure that Root folder is testing.
        /// </summary>
        /// <param name="targetUri">Relative uri to testing folder</param>
        protected string GetUri(string targetUri = "")
        {
            return (DropboxManager.AppRootUri + TestingFolderRelativeUri + targetUri).ToUri();
        }

        /// <summary>
        /// Gets the file/folder full path of the 'Resources' folder
        /// </summary>
        /// <param name="path">Relative folder/file name of 'Resources' folder</param>
        protected string GetResourcesFolderDir(string path = "")
        {
            string projectFullDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.Replace("\\bin\\Debug", "\\Resources");
            return !string.IsNullOrEmpty(path) ? System.IO.Path.Combine(projectFullDir, path) : projectFullDir;
        }
    }
}
