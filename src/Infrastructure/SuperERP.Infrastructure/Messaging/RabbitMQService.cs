using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace SuperERP.Infrastructure.Messaging;

public interface IMessageBus
{
    Task PublishAsync<T>(string queue, T message);
}

public class RabbitMQService : IMessageBus, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQService(string connectionString)
    {
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public Task PublishAsync<T>(string queue, T message)
    {
        _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
        
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);
        
        _channel.BasicPublish(exchange: "", routingKey: queue, body: body);
        
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
