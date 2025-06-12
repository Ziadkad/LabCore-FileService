using FileService.Application.Common.Models;

namespace FileService.Application.Common.Interfaces;

public interface IUserContext
{
    Guid GetCurrentUserId();
    UserRole GetUserRole();
}