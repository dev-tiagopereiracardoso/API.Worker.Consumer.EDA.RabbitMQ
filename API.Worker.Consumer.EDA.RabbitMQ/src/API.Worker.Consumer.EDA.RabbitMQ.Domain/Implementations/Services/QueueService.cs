using API.Worker.Consumer.EDA.RabbitMQ.Domain.Implementations.Interfaces;
using API.Worker.Consumer.EDA.RabbitMQ.Models.Events;
using Microsoft.Extensions.Logging;

namespace API.Worker.Consumer.EDA.RabbitMQ.Domain.Implementations.Services
{
    public class QueueService : IQueueService
    {
        private readonly ILogger<QueueService> _logger;

        public QueueService(
                ILogger<QueueService> logger
            )
        {
            _logger = logger;
        }

        public async Task ExecuteConsumer(RegisterUserEvent json)
        {
            // rules to execute consumer
        }
    }
}