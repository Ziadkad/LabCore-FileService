using FileService.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    private IMediator? _mediator;
    
    private IUserContext? _userContext;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>() ?? throw new InvalidOperationException("Mediator not found in request services.");
    
    protected IUserContext UserContext => _userContext ??= HttpContext.RequestServices.GetService<IUserContext>() ?? throw new InvalidOperationException("UserContext not found in request services.");
}