

using Shared.Models;

namespace FileService.Application.Services.Interfaces.Broker;

public interface IMessageProducer
{
    Task SendAsync(FileMessage message);
    Task SendDeleteAsync(FileMessage message);
}