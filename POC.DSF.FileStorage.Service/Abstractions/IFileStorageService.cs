namespace POC.DSF.FileStorage.Service.Abstractions
{
    public interface IFileStorageService
    {
        Task UploadAsync(Stream stream, string fileName, string contentType);
        Task<(Stream, string)> DownloadAsync(string fileName);
    }
}
