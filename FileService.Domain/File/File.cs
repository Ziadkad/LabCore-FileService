using FileService.Domain.Common;

namespace FileService.Domain.File;

public class File : BaseModel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string FileType { get; private set; }
    public string Path { get; private set; }
    public bool IsAccessible { get; private set; }
    
    public Guid ProjectId { get; private set; }
    public Guid? StudyId { get; private set; } 
    public Guid? TaskId { get; private set; }

    public FileContext Context { get; private set; }

    public File(Guid id, string name, string fileType, string path, bool isAccessible, Guid projectId, Guid? studyId, Guid? taskId, FileContext context)
    {
        Id = id;
        Name = name;
        FileType = fileType;
        Path = path;
        IsAccessible = isAccessible;
        ProjectId = projectId;
        StudyId = studyId;
        TaskId = taskId;
        Context = context;
    }
}