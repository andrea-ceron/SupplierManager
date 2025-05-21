using SupplierManager.Business.Abstraction;
using SupplierManager.Repository.Abstraction;
using SupplierManager.Repository.Model;
using SupplierManager.Shared;

namespace SupplierManager.Business
{
	public class Business(IRepository repository) : IBusiness
	{
		public Task CreateSupplierAsync(SupplierDto supplier, List<Product> productList, CancellationToken ct = default)
		{
			throw new NotImplementedException();
		}

		public Task DeleteSupplierAsync(int supplierId, CancellationToken ct = default)
		{
			throw new NotImplementedException();
		}

		public async Task<SupplierDto> GetSupplierAsync(int supplierId, CancellationToken ct = default)
		{
			
			Supplier? res =  await repository.GetSupplierById(supplierId, ct);
			if(res == null)
			{
				//throw new Exception("da definire");
			}

			SupplierDto supplier = new()
			{
				Id = res.Id,
				Orders = 
			};
		}

		public Task UpdateSupplierAsync(SupplierDto supplier, List<Product> ProductsToUpdate, CancellationToken ct = default)
		{
			throw new NotImplementedException();
		}
	}
}
