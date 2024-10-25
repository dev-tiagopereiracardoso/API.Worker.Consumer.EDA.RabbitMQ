using API.Worker.Consumer.EDA.RabbitMQ.Domain.Implementations.Interfaces;
using API.Worker.Consumer.EDA.RabbitMQ.Models.Events;
using MassTransit;
using Newtonsoft.Json;

namespace API.Worker.Consumer.EDA.RabbitMQ.Service.Consumers
{
    public class ServiceConsumer : IConsumer<RegisterUserEvent>
    {
        private readonly ILogger<ServiceConsumer> _logger;

        private readonly IQueueService _queueService;

        public ServiceConsumer(
                ILogger<ServiceConsumer> logger, 
                IQueueService queueService
            )
        {
            _logger = logger;
            _queueService = queueService;
        }

        public async Task Consume(ConsumeContext<RegisterUserEvent> context)
        {
            var json = context.Message;

            await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(json));

            await _queueService.ExecuteConsumer(json);
        }
    }
}
