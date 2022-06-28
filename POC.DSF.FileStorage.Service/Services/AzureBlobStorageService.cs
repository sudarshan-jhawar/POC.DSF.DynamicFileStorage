using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using POC.DSF.FileStorage.Service.Abstractions;
using POC.DSF.FileStorage.Service.Settings;
using System.Net;

namespace POC.DSF.FileStorage.Service.Services
{
    public class AzureBlobStorageService : IFileStorageService
    {
        private readonly BlobContainerClient blobContainerClient;
        public AzureBlobStorageService(IOptions<AppSettings> settings)
        {
            blobContainerClient = new(settings.Value.AzureBlobSettings.ConnectionString, settings.Value.AzureBlobSettings.BlobContainer);
        }
        public async Task<(Stream, string)> Download(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
            }

            try
            {
                var blobClient = blobContainerClient.GetBlobClient(fileName);
                BlobDownloadResult response = await blobClient.DownloadContentAsync();
                return (response.Content.ToStream(), response.Details.ContentType);

            }
            catch (RequestFailedException rfe)
            {
                throw new Exception($"Error in getting file. Reason : {rfe.Message} Http Status : {rfe.Status}");
            }
        }

        public async Task Upload(Stream stream, string fileName, string contentType)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentException($"'{nameof(contentType)}' cannot be null or whitespace.", nameof(contentType));
            }

            try
            {
                var blobClient = blobContainerClient.GetBlobClient(fileName);
                BlobContentInfo response = await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } });
            }
            catch (RequestFailedException rfe)
            {
                throw new Exception($"Error in uploading file. Reason : {rfe.Message} Http Status : {rfe.Status}");
            }
        }
    }
}
