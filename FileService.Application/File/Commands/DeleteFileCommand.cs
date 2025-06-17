using FileService.Application.Common.Exceptions;
using FileService.Application.Common.Extentions;
using FileService.Application.Common.Interfaces;
using FileService.Application.Common.Models;
using FileService.Application.File.Models;
using FileService.Application.Services;
using FileService.Application.Services.Interfaces.Broker;
using MediatR;
using Shared.Models;

namespace FileService.Application.File.Commands;

public record DeleteFileCommand(List<string> Folders, string FileName):IRequest;

public class DeleteFileCommandHandler(    
    IUserContext userContext, 
    IFileRepository fileRepository,
    IFileService fileService, 
    IUnitOfWork unitOfWork, 
    IMessageProducer messageProducer) : IRequestHandler<DeleteFileCommand>
{
    public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        string directoryPath = PathExtention.CreatePath(request.Folders);
        string filePath = Path.Combine(directoryPath, request.FileName);
        Domain.File.File? file = await fileRepository.GetByFileNameAsync(request.FileName, cancellationToken);
        
        if (file is null || !Directory.Exists(directoryPath) || !System.IO.File.Exists(filePath))
        {
            throw new NotFoundException("File", request.FileName);
        }
        if (!file.IsAccessible && userContext.GetUserRole() == UserRole.Researcher)
        {
            throw new ForbiddenException();
        }

        FileMessage fileMessage = new FileMessage(file.Path, file.ProjectId, file.StudyId, file.TaskId, file.Context);
        fileRepository.Remove(file);
        var result = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (result <= 0)
        {
            throw new InternalServerException();
        }
        await fileService.DeleteAsync(filePath);
        
        await messageProducer.SendDeleteAsync(fileMessage);
    }
}