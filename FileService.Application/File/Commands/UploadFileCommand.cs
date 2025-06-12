using FileService.Application.Common.Exceptions;
using FileService.Application.Common.Extentions;
using FileService.Application.Common.Interfaces;
using FileService.Application.Common.Models;
using FileService.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FileService.Application.File.Commands;

public record UploadFileCommand(IFormFile File,List<string>Folders,bool? IsAccessible,Guid ProjectId): IRequest<string>;

public class UploadFileCommandHandler(IUserContext userContext, IFileRepository fileRepository,IFileService fileService, IUnitOfWork unitOfWork) : IRequestHandler<UploadFileCommand, string>
{
    public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        bool isAccessible = request.IsAccessible ?? userContext.GetUserRole() == UserRole.Researcher;

        Guid id = Guid.NewGuid();
        
        string directoryPath = PathExtention.CreatePath(request.Folders);
        
        PathExtention.CreateFolder(directoryPath);
        
        string filename = PathExtention.CreateFileName(request.File, id);
        
        string path = await fileService.UploadAsync(request.File, directoryPath, filename);
        
        string fileExtension = Path.GetExtension(path).ToLowerInvariant();
        
        Domain.File.File file = new(id,filename,fileExtension,path,isAccessible,request.ProjectId);
        
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
        return path;
    }
}
