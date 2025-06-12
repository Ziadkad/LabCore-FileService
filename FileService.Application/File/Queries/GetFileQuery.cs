using FileService.Application.Common.Exceptions;
using FileService.Application.Common.Extentions;
using FileService.Application.Common.Interfaces;
using FileService.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Application.File.Queries;

public record GetFileQuery(List<string> Folders, string FileName) : IRequest<PhysicalFileResult>;

public class GetFileQueryHandler(IFileRepository fileRepository, IUserContext userContext) : IRequestHandler<GetFileQuery, PhysicalFileResult>
{
    public async Task<PhysicalFileResult> Handle(GetFileQuery request, CancellationToken cancellationToken)
    {
        string directoryPath = PathExtention.CreatePath(request.Folders);
        string filePath = Path.Combine(directoryPath, request.FileName);
        Domain.File.File file = await fileRepository.GetByFileNameAsync(request.FileName);
        
        if (file is null || !Directory.Exists(directoryPath) || !System.IO.File.Exists(filePath))
        {
            throw new NotFoundException("File", request.FileName);
        }

        if (!file.IsAccessible && userContext.GetUserRole() == UserRole.Researcher)
        {
            throw new ForbiddenException();
        }
        var extension = Path.GetExtension(request.FileName).ToLowerInvariant();
        var contentType = FileExtention.GetContentType(extension);
        var result = new PhysicalFileResult(filePath, contentType);
        return await Task.FromResult(result);
    }
}
