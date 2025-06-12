using FileService.Application.Common.Exceptions;
using FileService.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FileService.Infrastructure.ExternalServices.FileService;

public class FileService(ILogger<FileService> logger) : IFileService
{
    public async Task<string> UploadAsync(IFormFile file, string directoryPath, string fileName)
    {
        var filePath = Path.Combine(directoryPath, fileName);
        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        logger.LogInformation($"File uploaded successfully to {filePath}");
        return filePath;
    }
    
    
    public Task DeleteAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            logger.LogWarning($"File not found: {filePath}");
            throw new NotFoundException("file", "key");
        }

        File.Delete(filePath);
        logger.LogInformation($"File deleted successfully from {filePath}");
        return Task.CompletedTask;
    }
}

