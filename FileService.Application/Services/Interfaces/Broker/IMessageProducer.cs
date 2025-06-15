using FileService.Application.File.Models;

namespace FileService.Application.Services.Interfaces.Broker;

public interface IMessageProducer
{
    Task SendAsync(FileDto message);
    Task SendDeleteAsync(FileDto message);
}