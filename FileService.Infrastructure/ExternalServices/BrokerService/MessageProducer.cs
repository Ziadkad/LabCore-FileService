using FileService.Application.File.Models;
using FileService.Application.Services.Interfaces.Broker;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Shared.Models;

namespace FileService.Infrastructure.ExternalServices.BrokerService;

public class MessageProducer(ISendEndpointProvider sendEndpointProvider, IConfiguration configuration) : IMessageProducer
{

    public async Task SendAsync(FileMessage message)
    {
        var brokerSettings =  configuration.GetSection("BrokerSettings");
        string queueName = brokerSettings["AddFileEndpoint"];
        if (string.IsNullOrWhiteSpace(queueName))
            throw new ArgumentException("Queue name must not be empty", nameof(queueName));
        
        var uri = new Uri($"queue:{queueName}");
        
        var endpoint = await sendEndpointProvider.GetSendEndpoint(uri);
        
        await endpoint.Send(message);
    }

    public async Task SendDeleteAsync(FileMessage message)
    {
        var brokerSettings =  configuration.GetSection("BrokerSettings");
        string queueName = brokerSettings["DeleteFileEndpoint"];
        if (string.IsNullOrWhiteSpace(queueName))
            throw new ArgumentException("Queue name must not be empty", nameof(queueName));
        
        var uri = new Uri($"queue:{queueName}");
        
        var endpoint = await sendEndpointProvider.GetSendEndpoint(uri);
        
        await endpoint.Send(message);
    }
}