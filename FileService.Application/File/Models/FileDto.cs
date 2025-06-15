using FileService.Domain.File;

namespace FileService.Application.File.Models;

public class FileDto(string path, Guid projectId, Guid? studyId, Guid? taskId, FileContext context)
{
    public string Path { get; set; } = path;
    public Guid ProjectId { get; set; } = projectId;
    public Guid? StudyId { get; set; } = studyId;
    public Guid? TaskId { get; set; } = taskId;
    public FileContext Context { get; set; } = context;
}