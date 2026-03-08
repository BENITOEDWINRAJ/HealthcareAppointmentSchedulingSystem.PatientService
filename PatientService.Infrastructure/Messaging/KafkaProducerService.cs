using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using System.Text.Json;
using SharedKafka.Events;

namespace PatientService.Infrastructure.Messaging
{
    public class KafkaProducerService
    {
        private readonly IProducer<string, string> _producer;

        public KafkaProducerService()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishUserCreated(UserCreatedEvent userEvent)
        {
            var message = new Message<string, string>
            {
                Key = userEvent.UserId.ToString(),
                Value = JsonSerializer.Serialize(userEvent)
            };

            await _producer.ProduceAsync("user-created-topic", message);
        }
    }
}
