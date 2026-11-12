using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fundoo.Notification.Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly EmailService _emailService;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, EmailService emailService)
    {
        _logger = logger;
        _configuration = configuration;
        _emailService = emailService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification Worker running at: {time}", DateTimeOffset.Now);

        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost"
        };

        using var connection = await factory.CreateConnectionAsync(stoppingToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        var queueName = _configuration["RabbitMQ:QueueName"] ?? "fundoo_queue";
        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Received RabbitMQ Message: {message}", message);

            // Test email bhejan layi (baad vich isnu apne email nal replace kar layin)
            await _emailService.SendEmailAsync("sharmashrikant1486@gmail.com", "Fundoo App Notification", message);
        };

        await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}