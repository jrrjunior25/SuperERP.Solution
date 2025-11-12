namespace SuperERP.Infrastructure.Services;

public interface IStorageService
{
    Task<string> UploadAsync(string fileName, Stream fileStream);
    Task<Stream> DownloadAsync(string fileUrl);
    Task DeleteAsync(string fileUrl);
}

public class LocalStorageService : IStorageService
{
    private readonly string _basePath;

    public LocalStorageService(string basePath)
    {
        _basePath = basePath;
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> UploadAsync(string fileName, Stream fileStream)
    {
        var filePath = Path.Combine(_basePath, fileName);
        
        using var fileStreamOutput = File.Create(filePath);
        await fileStream.CopyToAsync(fileStreamOutput);
        
        return filePath;
    }

    public Task<Stream> DownloadAsync(string fileUrl)
    {
        return Task.FromResult<Stream>(File.OpenRead(fileUrl));
    }

    public Task DeleteAsync(string fileUrl)
    {
        if (File.Exists(fileUrl))
            File.Delete(fileUrl);
        
        return Task.CompletedTask;
    }
}
