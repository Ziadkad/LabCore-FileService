using Microsoft.AspNetCore.Http;

namespace FileService.Application.Common.Extentions;

public static class PathExtention
{
    public static string CreatePath(List<string> folders)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        string directoryPath = Path.Combine(basePath,"Files");
        foreach (var folder in folders)
        {
            directoryPath = Path.Combine(directoryPath, folder);
        }
        return directoryPath;
    }

    public static string CreateFileName(IFormFile file, Guid id)
    {
        return id+file.FileName;
    }

    public static void CreateFolder(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}