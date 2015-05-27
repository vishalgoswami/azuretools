using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure;


using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace AzureBloblUploader
{
    /// <summary>
    /// Blob storage manager class
    /// </summary>
    public class BlobManager
    {
        #region CONTENT TYPES ...
        private static readonly Dictionary<string, string> MIMETypesDictionary = new Dictionary<string, string>
          {
            {"ai", "application/postscript"},
            {"aif", "audio/x-aiff"},
            {"aifc", "audio/x-aiff"},
            {"aiff", "audio/x-aiff"},
            {"asc", "text/plain"},
            {"atom", "application/atom+xml"},
            {"au", "audio/basic"},
            {"avi", "video/x-msvideo"},
            {"bcpio", "application/x-bcpio"},
            {"bin", "application/octet-stream"},
            {"bmp", "image/bmp"},
            {"cdf", "application/x-netcdf"},
            {"cgm", "image/cgm"},
            {"class", "application/octet-stream"},
            {"cpio", "application/x-cpio"},
            {"cpt", "application/mac-compactpro"},
            {"csh", "application/x-csh"},
            {"css", "text/css"},
            {"dcr", "application/x-director"},
            {"dif", "video/x-dv"},
            {"dir", "application/x-director"},
            {"djv", "image/vnd.djvu"},
            {"djvu", "image/vnd.djvu"},
            {"dll", "application/octet-stream"},
            {"dmg", "application/octet-stream"},
            {"dms", "application/octet-stream"},
            {"doc", "application/msword"},
            {"docx","application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {"dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
            {"docm","application/vnd.ms-word.document.macroEnabled.12"},
            {"dotm","application/vnd.ms-word.template.macroEnabled.12"},
            {"dtd", "application/xml-dtd"},
            {"dv", "video/x-dv"},
            {"dvi", "application/x-dvi"},
            {"dxr", "application/x-director"},
            {"eps", "application/postscript"},
            {"etx", "text/x-setext"},
            {"exe", "application/octet-stream"},
            {"ez", "application/andrew-inset"},
            {"gif", "image/gif"},
            {"gram", "application/srgs"},
            {"grxml", "application/srgs+xml"},
            {"gtar", "application/x-gtar"},
            {"hdf", "application/x-hdf"},
            {"hqx", "application/mac-binhex40"},
            {"htm", "text/html"},
            {"html", "text/html"},
            {"ice", "x-conference/x-cooltalk"},
            {"ico", "image/x-icon"},
            {"ics", "text/calendar"},
            {"ief", "image/ief"},
            {"ifb", "text/calendar"},
            {"iges", "model/iges"},
            {"igs", "model/iges"},
            {"jnlp", "application/x-java-jnlp-file"},
            {"jp2", "image/jp2"},
            {"jpe", "image/jpeg"},
            {"jpeg", "image/jpeg"},
            {"jpg", "image/jpeg"},
            {"js", "application/x-javascript"},
            {"kar", "audio/midi"},
            {"latex", "application/x-latex"},
            {"lha", "application/octet-stream"},
            {"lzh", "application/octet-stream"},
            {"m3u", "audio/x-mpegurl"},
            {"m4a", "audio/mp4a-latm"},
            {"m4b", "audio/mp4a-latm"},
            {"m4p", "audio/mp4a-latm"},
            {"m4u", "video/vnd.mpegurl"},
            {"m4v", "video/x-m4v"},
            {"mac", "image/x-macpaint"},
            {"man", "application/x-troff-man"},
            {"mathml", "application/mathml+xml"},
            {"me", "application/x-troff-me"},
            {"mesh", "model/mesh"},
            {"mid", "audio/midi"},
            {"midi", "audio/midi"},
            {"mif", "application/vnd.mif"},
            {"mov", "video/quicktime"},
            {"movie", "video/x-sgi-movie"},
            {"mp2", "audio/mpeg"},
            {"mp3", "audio/mpeg"},
            {"mp4", "video/mp4"},
            {"mpe", "video/mpeg"},
            {"mpeg", "video/mpeg"},
            {"mpg", "video/mpeg"},
            {"mpga", "audio/mpeg"},
            {"ms", "application/x-troff-ms"},
            {"msh", "model/mesh"},
            {"mxu", "video/vnd.mpegurl"},
            {"nc", "application/x-netcdf"},
            {"oda", "application/oda"},
            {"ogg", "application/ogg"},
            {"pbm", "image/x-portable-bitmap"},
            {"pct", "image/pict"},
            {"pdb", "chemical/x-pdb"},
            {"pdf", "application/pdf"},
            {"pgm", "image/x-portable-graymap"},
            {"pgn", "application/x-chess-pgn"},
            {"pic", "image/pict"},
            {"pict", "image/pict"},
            {"png", "image/png"}, 
            {"pnm", "image/x-portable-anymap"},
            {"pnt", "image/x-macpaint"},
            {"pntg", "image/x-macpaint"},
            {"ppm", "image/x-portable-pixmap"},
            {"ppt", "application/vnd.ms-powerpoint"},
            {"pptx","application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            {"potx","application/vnd.openxmlformats-officedocument.presentationml.template"},
            {"ppsx","application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
            {"ppam","application/vnd.ms-powerpoint.addin.macroEnabled.12"},
            {"pptm","application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
            {"potm","application/vnd.ms-powerpoint.template.macroEnabled.12"},
            {"ppsm","application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
            {"ps", "application/postscript"},
            {"qt", "video/quicktime"},
            {"qti", "image/x-quicktime"},
            {"qtif", "image/x-quicktime"},
            {"ra", "audio/x-pn-realaudio"},
            {"ram", "audio/x-pn-realaudio"},
            {"ras", "image/x-cmu-raster"},
            {"rdf", "application/rdf+xml"},
            {"rgb", "image/x-rgb"},
            {"rm", "application/vnd.rn-realmedia"},
            {"roff", "application/x-troff"},
            {"rtf", "text/rtf"},
            {"rtx", "text/richtext"},
            {"sgm", "text/sgml"},
            {"sgml", "text/sgml"},
            {"sh", "application/x-sh"},
            {"shar", "application/x-shar"},
            {"silo", "model/mesh"},
            {"sit", "application/x-stuffit"},
            {"skd", "application/x-koan"},
            {"skm", "application/x-koan"},
            {"skp", "application/x-koan"},
            {"skt", "application/x-koan"},
            {"smi", "application/smil"},
            {"smil", "application/smil"},
            {"snd", "audio/basic"},
            {"so", "application/octet-stream"},
            {"spl", "application/x-futuresplash"},
            {"src", "application/x-wais-source"},
            {"sv4cpio", "application/x-sv4cpio"},
            {"sv4crc", "application/x-sv4crc"},
            {"svg", "image/svg+xml"},
            {"swf", "application/x-shockwave-flash"},
            {"t", "application/x-troff"},
            {"tar", "application/x-tar"},
            {"tcl", "application/x-tcl"},
            {"tex", "application/x-tex"},
            {"texi", "application/x-texinfo"},
            {"texinfo", "application/x-texinfo"},
            {"tif", "image/tiff"},
            {"tiff", "image/tiff"},
            {"tr", "application/x-troff"},
            {"tsv", "text/tab-separated-values"},
            {"txt", "text/plain"},
            {"ustar", "application/x-ustar"},
            {"vcd", "application/x-cdlink"},
            {"vrml", "model/vrml"},
            {"vxml", "application/voicexml+xml"},
            {"wav", "audio/x-wav"},
            {"wbmp", "image/vnd.wap.wbmp"},
            {"wbmxl", "application/vnd.wap.wbxml"},
            {"wml", "text/vnd.wap.wml"},
            {"wmlc", "application/vnd.wap.wmlc"},
            {"wmls", "text/vnd.wap.wmlscript"},
            {"wmlsc", "application/vnd.wap.wmlscriptc"},
            {"wrl", "model/vrml"},
            {"xbm", "image/x-xbitmap"},
            {"xht", "application/xhtml+xml"},
            {"xhtml", "application/xhtml+xml"},
            {"xls", "application/vnd.ms-excel"},                        
            {"xml", "application/xml"},
            {"xpm", "image/x-xpixmap"},
            {"xsl", "application/xml"},
            {"xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {"xltx","application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
            {"xlsm","application/vnd.ms-excel.sheet.macroEnabled.12"},
            {"xltm","application/vnd.ms-excel.template.macroEnabled.12"},
            {"xlam","application/vnd.ms-excel.addin.macroEnabled.12"},
            {"xlsb","application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
            {"xslt", "application/xslt+xml"},
            {"xul", "application/vnd.mozilla.xul+xml"},
            {"xwd", "image/x-xwindowdump"},
            {"xyz", "chemical/x-xyz"},
            {"zip", "application/zip"}
          };
        #endregion

        private readonly CloudStorageAccount _account;
        private readonly CloudBlobClient _blobClient;

        private static string GetMIMEType(string extension)
        {
            //get file extension
            //string extension = Path.GetExtension(fileName).ToLowerInvariant();

            if (extension.Length > 0 &&
                MIMETypesDictionary.ContainsKey(extension.Remove(0, 1)))
            {
                return MIMETypesDictionary[extension.Remove(0, 1)];
            }
            return "application/octet-stream";
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BlobManager" /> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string in app.config or web.config file.</param>
        public BlobManager(string connectionStringName)
        {
            _account = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);

            _blobClient = _account.CreateCloudBlobClient();
            //_blobClient.RetryPolicy = RetryPolicies.Retry(4, TimeSpan.Zero);
        }

        public bool ConfigureCors()
        {
            try
            {
                var serviceProperties = _blobClient.GetServiceProperties();
                var cors = new CorsRule();
                cors.AllowedOrigins.Add("*");
                //cors.AllowedOrigins.Add("mysite.com"); // more restrictive may be preferable
                cors.AllowedMethods = CorsHttpMethods.Get;
                cors.AllowedHeaders.Add("*");
                cors.MaxAgeInSeconds = 3600;
                serviceProperties.Cors.CorsRules.Add(cors);
                _blobClient.SetServiceProperties(serviceProperties);
                return true;
            }
            catch (StorageException ex)
            {
                return false;
            }

        }
        /// <summary>
        /// Updates or created a blob in Azure blobl storage
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobName">Name of the blob.</param>
        /// <param name="content">The content of the blob.</param>
        /// <returns></returns>
        public bool PutBlob(string containerName, string blobName, string path, string contentType)
        {
            return ExecuteWithExceptionHandlingAndReturnValue(
                    () =>
                    {
                        CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
                        CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
                        using (var fileStream = System.IO.File.OpenRead(path))
                        {
                            blob.UploadFromStream(fileStream);
                            blob.Properties.ContentType = contentType;
                            blob.SetProperties();
                        } 
                        
                    });
        }

        /// <summary>
        /// Creates the container in Azure blobl storage
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns>True if contianer was created successfully</returns>
        public bool CreateContainer(string containerName)
        {
            return ExecuteWithExceptionHandlingAndReturnValue(
                    () =>
                    {
                        CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
                        container.CreateIfNotExists();
                        container.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess =
                                BlobContainerPublicAccessType.Blob
                        }); 
                    });
        }

        /// <summary>
        /// Checks if a container exist.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns>True if conainer exists</returns>
        /*public bool DoesContainerExist(string containerName)
        {
            bool returnValue = false;
            ExecuteWithExceptionHandling(
                    () =>
                    {
                        IEnumerable<CloudBlobContainer> containers = _blobClient.ListContainers();
                        returnValue = containers.Any(one => one.Name == containerName);
                    });
            return returnValue;
        }*/

        /// <summary>
        /// Uploads the directory to blobl storage
        /// </summary>
        /// <param name="sourceDirectory">The source directory name.</param>
        /// <param name="containerName">Name of the container to upload to.</param>
        /// <returns>True if successfully uploaded</returns>
        public bool UploadDirectory(string sourceDirectory, string containerName, string prefixAzureFolderName, string type)
        {
            return UploadDirectory(sourceDirectory, containerName, prefixAzureFolderName);
        }

        private bool UploadDirectory(string sourceDirectory, string containerName, string prefixAzureFolderName)
        {
            return ExecuteWithExceptionHandlingAndReturnValue(
                () =>
                {
                    
                    CreateContainer(containerName);
                    
                    var folder = new DirectoryInfo(sourceDirectory);
                    var files = folder.GetFiles();
                    foreach (var fileInfo in files)
                    {
                        string blobName = fileInfo.Name;
                        if (!string.IsNullOrEmpty(prefixAzureFolderName))
                        {
                            blobName = prefixAzureFolderName + "/" + blobName;
                        }
                        PutBlob(containerName, blobName, fileInfo.FullName, GetMIMEType(fileInfo.Extension));
                    }
                    var subFolders = folder.GetDirectories();
                    foreach (var directoryInfo in subFolders)
                    {
                        var prefix = directoryInfo.Name;
                        if (!string.IsNullOrEmpty(prefixAzureFolderName))
                        {
                            prefix = prefixAzureFolderName + "/" + prefix;
                        }
                        UploadDirectory(directoryInfo.FullName, containerName, prefix);
                    }
                });
        }

       

        private void ExecuteWithExceptionHandling(Action action)
        {
            try
            {
                action();
            }
            catch (StorageException ex)
            {
                if ((int)ex.RequestInformation.HttpStatusCode != 409)
                {
                    throw;
                }
            }
        }

        private bool ExecuteWithExceptionHandlingAndReturnValue(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (StorageException ex)
            {
                if ((int)ex.RequestInformation.HttpStatusCode == 409)
                {
                    return false;
                }
                throw;
            }
        }
    }
}
