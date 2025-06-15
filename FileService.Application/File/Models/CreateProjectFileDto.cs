using FileService.Domain.File;
using Microsoft.AspNetCore.Http;

namespace FileService.Application.File.Models;

public record CreateProjectFileDto(IFormFile File,bool? IsAccessible,Guid ProjectId,Guid? StudyId, Guid? TaskId);