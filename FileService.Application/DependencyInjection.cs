using System.Reflection;
using FileService.Domain.Common;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Application;

public static class DependencyInjection
{
    public static void RegisterApplicationServices(
        this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(BaseModel))!));

    }
}