namespace DropboxApiLibrary.Models
{
    using System;
    using Extensions;

    /// <summary>
    /// Holds file local directory paths and service url's.
    /// </summary>
    public class FileRequestInfo
    {
        private readonly string _localRootPath;
        private readonly string _localFileRelativePath;
        private readonly string _serviceRootUri;
        private readonly string _serviceFileRelativeRoute;

        /// <summary>
        /// Holds file local directory paths and service url's.
        /// </summary>
        /// <exception cref="ArgumentNullException">If any of the given arguments are null or empty</exception>
        /// <param name="localRootPath">The local root directory</param>
        /// <param name="localFileRelativePath">Local file relative path from root directory</param>
        /// <param name="serviceRootUri">The service root uri</param>
        /// <param name="serviceFileRelativeRoute">Service file relative route from root uri</param>
        public FileRequestInfo(string localRootPath, string localFileRelativePath, string serviceRootUri, string serviceFileRelativeRoute)
        {
            if (string.IsNullOrEmpty(localRootPath) 
                || string.IsNullOrEmpty(localFileRelativePath)
                || string.IsNullOrEmpty(serviceRootUri)
                || string.IsNullOrEmpty(serviceFileRelativeRoute))
            {
                throw new ArgumentNullException("File root path/uri and file relative path/uri cannot be null!");
            }

            _localRootPath = localRootPath;
            _localFileRelativePath = localFileRelativePath;
            _serviceRootUri = serviceRootUri;
            _serviceFileRelativeRoute = serviceFileRelativeRoute;
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        public bool IsFile
        {
            get
            {
                return System.IO.Path.HasExtension(_localFileRelativePath);
            }
        }

        /// <summary>
        /// The local root directory.
        /// </summary>
        public string LocalRootPath
        {
            get
            {
                return _localRootPath.ToPath();
            }
        }

        /// <summary>
        /// The local file path directory relative 
        /// from local root directory.
        /// </summary>
        public string LocalFileRelativePath
        {
            get
            {
                return _localFileRelativePath.ToPath();
            }
        }

        /// <summary>
        /// The service root uri.
        /// </summary>
        public string ServiceRootUri
        {
            get
            {
                return _serviceRootUri.ToUri();
            }
        }

        /// <summary>
        /// The service file uri relative from service root uri.
        /// </summary>
        public string ServiceFileRelativeRoute
        {
            get
            {
                return _serviceFileRelativeRoute.ToUri();
            }
        }

        /// <summary>
        /// Gets local file full path.
        /// </summary>
        /// <returns>LocalRootPath + LocalFileRelativePath</returns>
        public string GetLocalFileFullPath()
        {
            return System.IO.Path.Combine(LocalRootPath, LocalFileRelativePath);
        }

        /// <summary>
        /// Gets local file full path without file name.
        /// If the full path does not contain file name, just return the full path.
        /// </summary>
        /// <returns>Full file path without file name</returns>
        public string GetLocalFileFullPathWithoutFileName()
        {
            return IsFile ? System.IO.Path.GetDirectoryName(this.GetLocalFileFullPath()) : this.GetLocalFileFullPath();
        }

        /// <summary>
        /// Gets service file full uri.
        /// </summary>
        /// <returns>ServiceRootUri + ServiceFileRelativeRoute</returns>
        public string GetServiceFileFullUri()
        {
            return ServiceRootUri + ServiceFileRelativeRoute;
        }
    }
}
