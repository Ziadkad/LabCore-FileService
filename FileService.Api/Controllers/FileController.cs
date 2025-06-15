using FileService.Application.File.Commands;
using FileService.Application.File.Models;
using FileService.Application.File.Queries;
using FileService.Domain.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Api.Controllers;

public class FileController :BaseController
{
    private const string PROJECTS = "Projects";
    
    [Authorize]
    [HttpGet("app/Files/Projects/{projectId:guid}/{filename}")]
    public async Task<IActionResult> GetProjectFile(string filename, Guid projectId)
    {
        List<string> folders = new List<string>()
        {
            PROJECTS , projectId.ToString()
        };
        var result = await Mediator.Send(new GetFileQuery(folders, filename));
        return result;
    }
    
        
    [Authorize]
    [HttpGet("app/Files/Projects/{projectId:guid}/{studyId:guid}/{filename}")]
    public async Task<IActionResult> GetStudyFile(string filename, Guid projectId, Guid studyId)
    {
        List<string> folders = new List<string>()
        {
            PROJECTS , projectId.ToString(), studyId.ToString()
        };
        var result = await Mediator.Send(new GetFileQuery(folders, filename));
        return result;
    }

    
    [Authorize]
    [HttpGet("app/Files/Projects/{projectId:guid}/{studyId:guid}/{taskId:guid}/{filename}")]
    public async Task<IActionResult> GetTaskFile(string filename, Guid projectId, Guid studyId, Guid taskId)
    {
        List<string> folders = new List<string>()
        {
            PROJECTS , projectId.ToString(), studyId.ToString(), taskId.ToString()
        };
        var result = await Mediator.Send(new GetFileQuery(folders, filename));
        return result;
    }
    

    [Authorize]
    [HttpPost("Upload")]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    [Authorize]
    [HttpDelete("{projectId:guid}/{filename}")]
    public async Task<IActionResult> DeleteProjectFile(string filename, Guid projectId)
    {
        List<string> folders = new List<string>()
        {
            PROJECTS , projectId.ToString()
        };
        await Mediator.Send(new DeleteFileCommand(folders, filename));
        return NoContent();
    }
    
        
    [Authorize]
    [HttpDelete("{projectId:guid}/{studyId:guid}/{filename}")]
    public async Task<IActionResult> DeleteStudyFile(string filename, Guid projectId, Guid studyId)
    {
        List<string> folders = new List<string>()
        {
            PROJECTS , projectId.ToString(), studyId.ToString()
        };
        await Mediator.Send(new DeleteFileCommand(folders, filename));
        return NoContent();
    }

    
    [Authorize]
    [HttpDelete("{projectId:guid}/{studyId:guid}/{taskId:guid}/{filename}")]
    public async Task<IActionResult> DeleteTaskFile(string filename, Guid projectId, Guid studyId, Guid taskId)
    {
        List<string> folders = new List<string>()
        {
            PROJECTS , projectId.ToString(), studyId.ToString(), taskId.ToString()
        };
        await Mediator.Send(new DeleteFileCommand(folders, filename));
        return NoContent();
    }
    
}