namespace FileService.Application.Common.Interfaces;

public interface IFileRepository : IBaseRepository<Domain.File.File, Guid>
{
    Task<Domain.File.File?> GetByFileNameAsync(string fileName, CancellationToken cancellationToken = default);
}