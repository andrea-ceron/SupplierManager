using CustomerManager.Repository.Model;
using SupplierManager.Shared.DTO;
using System.Text.Json;
using Utility.Kafka.MessageHandlers;

namespace CustomerManager.Business.Factory
{
    public static class TransactionalOutboxFactory
    {
		public static TransactionalOutbox CreateInsert(ProductDtoForKafka dto) => Create(dto, Operations.Insert);

		private static TransactionalOutbox Create(ProductDtoForKafka dto, string operation) => Create(nameof(ProductDtoForKafka), dto, operation);
		private static TransactionalOutbox Create<TDTO>(string table, TDTO dto, string operation) where TDTO : class, new()
		{

			OperationMessage<TDTO> opMsg = new OperationMessage<TDTO>()
			{
				Dto = dto,
				Operation = operation
			};

			return new TransactionalOutbox()
			{
				Table = table,
				Message = JsonSerializer.Serialize(opMsg)
			};
		}
	}
}
