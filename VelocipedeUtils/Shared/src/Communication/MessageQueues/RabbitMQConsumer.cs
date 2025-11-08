using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace VelocipedeUtils.Shared.Models.MessageQueues;

/// <summary>
/// RabbitMQ consumer.
/// </summary>
public class RabbitMQConsumer<TArg1, TRes>
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly Timer _timer;
    System.Func<TArg1, TRes> _func;
    
    private readonly string _queueName;

    /// <summary>
    /// 
    /// </summary>
    public RabbitMQConsumer(string hostName, string queueName, System.TimeSpan timeInterval, System.Func<TArg1, TRes> func)
    {
        _queueName = queueName;
        var factory = new ConnectionFactory { HostName = hostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
        _timer = new Timer(OnTimerElapsed, null, System.TimeSpan.Zero, timeInterval);
        _func = func;
    }

    /// <summary>
    /// 
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Dispose();
        _channel.Close();
        _connection.Close();
        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnTimerElapsed(object state)
    {
        try
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                TArg1 inputString = JsonSerializer.Deserialize<TArg1>(message);
                _func(inputString);
            };
            _channel.BasicConsume(queue: _queueName,
                                autoAck: true,
                                consumer: consumer);
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine($"Exception: {ex}");
        }
    }
}
