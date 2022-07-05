using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using POC.DSF.FileStorage.Service.Abstractions;
using POC.DSF.FileStorage.Service.Settings;

namespace POC.DSF.FileStorage.Service.Services
{
    public class AWSS3StorageService : IFileStorageService
    {
        private readonly IAmazonS3 _client;
        private readonly AWSS3Settings settings;
        public AWSS3StorageService(IOptions<AppSettings> options)
        {
            settings = options.Value.AWSS3Settings;
            var region = RegionEndpoint.USWest2;
            var accessKey = options.Value.AWSS3Settings.AccessKey;
            var secretKey = options.Value.AWSS3Settings.SecretKey;
            _client = new AmazonS3Client(accessKey, secretKey, region);
        }
        public async Task<(Stream, string)> DownloadAsync(string fileName)
        {
            try
            {
                GetObjectResponse response = await _client.GetObjectAsync(settings.BucketName, fileName);
                return (response.ResponseStream, response.Headers["Content-Type"]);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task UploadAsync(Stream stream, string fileName, string contentType)
        {
            try
            {
                var additionalProps = new Dictionary<string, object>()
                {
                    { "ContentType",contentType}
                };
                await _client.UploadObjectFromStreamAsync(settings.BucketName, fileName, stream, additionalProps);
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
