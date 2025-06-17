using Microsoft.Extensions.DependencyInjection;

namespace SupplierManager.Business.Kafka;

public class KafkaTopicsOutput : AbstractKafkaTopics {
    public string Versamenti { get; set; } = "Versamenti";

    public override IEnumerable<string> GetTopics() => [Versamenti];

}