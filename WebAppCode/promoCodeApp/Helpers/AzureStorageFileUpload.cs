using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using promoCodeApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;

namespace promoCodeApp.Helpers
{
    public class AzureStorageFileUpload
    {
        string containerName = ConfigurationManager.AppSettings["containerName"];
        public string uploadFiletoStorage(string fileName, HttpPostedFileBase file)
        {

            CloudStorageAccount storageAccount = null;
            CloudBlobClient blobClient = null;
            CloudBlobContainer container = null;
            CloudBlockBlob blockBlob = null;

            try
            {
                storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureStorage"].ConnectionString);
                blobClient = storageAccount.CreateCloudBlobClient();
                container = blobClient.GetContainerReference(containerName);

                blockBlob = container.GetBlockBlobReference(fileName);
                blockBlob.Properties.ContentType = file.ContentType;
                blockBlob.UploadFromStream(file.InputStream);

                return blockBlob.Uri.ToString();
            }
            finally
            {
                storageAccount = null;
                blobClient = null;
                container = null;
                blockBlob = null;
            }
        }

        public string readFileFromAzureStorage(string fileName)
        {
            string data = null;
            CloudStorageAccount storageAccount = null;
            CloudBlobClient blobClient = null;
            CloudBlobContainer container = null;
            CloudBlockBlob blockBlob = null;

            try
            {
                storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureStorage"].ConnectionString);
                blobClient = storageAccount.CreateCloudBlobClient();
                container = blobClient.GetContainerReference(containerName);
                blockBlob = container.GetBlockBlobReference(fileName);

                using (var memoryStream = new MemoryStream())
                {
                    blockBlob.DownloadToStream(memoryStream);

                    data = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
            finally
            {
                storageAccount = null;
                blobClient = null;
                container = null;
                blockBlob = null;
            }



            return data;
        }

        public List<AzureFileViewModel> getFileLinks(List<string> filesNames)
        {
            var fileList = new List<AzureFileViewModel>();

            CloudStorageAccount storageAccount = null;
            CloudBlobClient blobClient = null;
            CloudBlobContainer container = null;
            //CloudBlockBlob blockBlob = null;

            try
            {
                storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureStorage"].ConnectionString);
                blobClient = storageAccount.CreateCloudBlobClient();
                container = blobClient.GetContainerReference(containerName);

                foreach (var file in filesNames)
                {
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(file);

                    var sasToken = blockBlob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                    {
                        Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write,
                        SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),//Asssuming user stays on the page for an hour.
                        SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),

                    });

                    var blobUrl = blockBlob.Uri.AbsoluteUri + sasToken;

                    fileList.Add(new AzureFileViewModel
                    {
                        FileName = blockBlob.Name,
                        AzureUrl = blobUrl
                    });

                    blockBlob = null;
                }

                return fileList;

            }
            finally
            {
                storageAccount = null;
                blobClient = null;
                container = null;
                fileList = null;
            }

        }

        public string getFileLinks(string file)
        {
            var fileList = new List<AzureFileViewModel>();

            CloudStorageAccount storageAccount = null;
            CloudBlobClient blobClient = null;
            CloudBlobContainer container = null;
            //CloudBlockBlob blockBlob = null;

            try
            {
                storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureStorage"].ConnectionString);
                blobClient = storageAccount.CreateCloudBlobClient();
                container = blobClient.GetContainerReference(containerName);

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(file);

                var sasToken = blockBlob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                {
                    Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),//Asssuming user stays on the page for an hour.
                    SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),

                });

                var blobUrl = blockBlob.Uri.AbsoluteUri + sasToken;

                blockBlob = null;


                return blobUrl;

            }
            finally
            {
                storageAccount = null;
                blobClient = null;
                container = null;
                fileList = null;
            }

        }
    }
}