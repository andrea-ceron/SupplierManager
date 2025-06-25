using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Kafka.DependencyInjection;

namespace SupplierManager.Business.Kafka;

public class KafkaTopicsOutput : AbstractKafkaTopics
{
	public string RawMaterials { get; set; } = "raw-materials";
	public override IEnumerable<string> GetTopics() => [RawMaterials];
}
