using System.Runtime.Serialization;

namespace FileService.Domain.File;

public enum FileContext
{
    [EnumMember(Value = "Project")]
    Project,
    
    [EnumMember(Value = "Study")]
    Study,
    
    [EnumMember(Value = "Task")]
    Task
}