using FileService.Application.Common.Interfaces;
using FileService.Application.Services;
using FileService.Infrastructure.Data;
using FileService.Infrastructure.ExternalServices.UserContext;
using FileService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Infrastructure;

public static class DependencyInjection
{
    public static void RegisterDataServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
        
        services.AddScoped<IUnitOfWork>(c => c.GetRequiredService<AppDbContext>());
        services.AddScoped<IFileRepository, FileRepository>();

        
        
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddTransient<IFileService, ExternalServices.FileService.FileService>();
    }
}