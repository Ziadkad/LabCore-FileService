using FileService.Application.Common.Interfaces;
using FileService.Application.Services;
using FileService.Application.Services.Interfaces.Broker;
using FileService.Infrastructure.Data;
using FileService.Infrastructure.ExternalServices.BrokerService;
using FileService.Infrastructure.ExternalServices.UserContext;
using FileService.Infrastructure.Repositories;
using MassTransit;
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
        services.AddScoped<IFileService, ExternalServices.FileService.FileService>();
        services.AddScoped<IMessageProducer, MessageProducer>();

        var brokerSettings = configuration.GetSection("BrokerSettings");
        var brokerUsername = brokerSettings["Username"];
        var brokerPassword = brokerSettings["Password"];
        var brokerHost = brokerSettings["Host"];
        
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(brokerHost, "/", h =>
                {
                    h.Username(brokerUsername);
                    h.Password(brokerPassword);
                });
                cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                cfg.UseInMemoryOutbox(); 
            });
        });

        services.AddMassTransitHostedService();

    }
}