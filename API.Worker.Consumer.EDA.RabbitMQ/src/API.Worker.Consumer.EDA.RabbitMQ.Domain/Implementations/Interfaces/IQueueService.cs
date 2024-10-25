using API.Worker.Consumer.EDA.RabbitMQ.Models.Events;

namespace API.Worker.Consumer.EDA.RabbitMQ.Domain.Implementations.Interfaces
{
    public interface IQueueService
    {
        Task ExecuteConsumer(RegisterUserEvent json);
    }
}