using FileService.Application.Common.Interfaces;
using FileService.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using File = FileService.Domain.File.File;

namespace FileService.Infrastructure.Repositories;

public class FileRepository(AppDbContext dbContext) : BaseRepository<File,Guid>(dbContext), IFileRepository
{
    public async Task<File?> GetByFileNameAsync(string fileName, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(f => f.Name == fileName, cancellationToken);
    }
}