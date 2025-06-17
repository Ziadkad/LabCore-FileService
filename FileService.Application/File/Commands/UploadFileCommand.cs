using FileService.Application.Common.Exceptions;
using FileService.Application.Common.Extentions;
using FileService.Application.Common.Interfaces;
using FileService.Application.Common.Models;
using FileService.Application.File.Models;
using FileService.Application.Services;
using FileService.Application.Services.Interfaces.Broker;
using FileService.Domain.File;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Models;

namespace FileService.Application.File.Commands;

public record UploadFileCommand(IFormFile File,bool? IsAccessible,Guid ProjectId,Guid? StudyId, Guid? TaskId): IRequest<string>;

public class UploadFileCommandHandler(
    IUserContext userContext, 
    IFileRepository fileRepository,
    IFileService fileService, 
    IUnitOfWork unitOfWork, 
    IMessageProducer messageProducer
    ) : IRequestHandler<UploadFileCommand, string>
{
    public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        bool isAccessible = request.IsAccessible ?? userContext.GetUserRole() == UserRole.Researcher;

        var folders = new List<string> { "Projects", request.ProjectId.ToString() };
        var context = FileContext.Project;

        bool hasStudy = request.StudyId.HasValue && request.StudyId != Guid.Empty;
        bool hasTask = request.TaskId.HasValue && request.TaskId != Guid.Empty;

        if (hasStudy)
        {
            folders.Add(request.StudyId.ToString());
            context = FileContext.Study;
        }

        if (hasTask)
        {
            if (!hasStudy)
            {
                folders.Add(Guid.Empty.ToString());
            }
            folders.Add(request.TaskId.ToString());
            context = FileContext.Task;
        }
        
        string directoryPath = PathExtention.CreatePath(folders);
        
        PathExtention.CreateFolder(directoryPath);
        
        Guid id = Guid.NewGuid();
        
        string filename = PathExtention.CreateFileName(request.File, id);
        
        string path = await fileService.UploadAsync(request.File, directoryPath, filename);
        
        string fileExtension = Path.GetExtension(path).ToLowerInvariant();
        
        Domain.File.File file = new(id,filename,fileExtension,path,isAccessible,request.ProjectId,request.StudyId,request.TaskId,context);
        
        await fileRepository.AddAsync(file,cancellationToken);
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            if (!string.IsNullOrEmpty(path))
            {
                await fileService.DeleteAsync(path);
            }
            throw new InternalServerException();
        }

        FileMessage fileMessage = new FileMessage(file.Path, file.ProjectId, file.StudyId, file.TaskId, file.Context);
        await messageProducer.SendAsync(fileMessage);
        
        return path;
    }
}
