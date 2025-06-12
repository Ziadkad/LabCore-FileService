using Microsoft.AspNetCore.Http;

namespace FileService.Application.Services;

public interface IFileService
{
    Task<string> UploadAsync(IFormFile file, string directoryPath, string fileName);
    Task DeleteAsync(string filePath);
}