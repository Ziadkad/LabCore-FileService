using FileService.Application.File.Commands;
using FileService.Application.File.Models;
using FileService.Application.File.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Api.Controllers;

public class FileController :BaseController
{
    [Authorize]
    [HttpGet("{projectId:guid}/{filename}")]
    public async Task<IActionResult> GetProjectFile(string filename, Guid projectId)
    {
        List<string> folders = new List<string>()
        {
            "Files", "Projects" , projectId.ToString()
        };
        var result = await Mediator.Send(new GetFileQuery(folders, filename));
        return result;
    }
    
    [Authorize]
    [HttpPost("project")]
    public async Task<IActionResult> UploadProjectFile([FromForm] CreateProjectFileDto projectFileDto)
    {
        List<string> folders = new List<string>()
        {
            "Files", "Projects" , projectFileDto.ProjectId.ToString()
        };
        var result = await Mediator.Send(new UploadFileCommand(projectFileDto.File, folders,projectFileDto.IsAccessible,projectFileDto.ProjectId));
        return Ok(result);
    }
}