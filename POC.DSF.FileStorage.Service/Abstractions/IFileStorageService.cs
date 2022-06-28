namespace POC.DSF.FileStorage.Service.Abstractions
{
    public interface IFileStorageService
    {
        Task Upload(Stream stream, string fileName, string contentType);
        Task<(Stream, string)> Download(string fileName);
    }
}
