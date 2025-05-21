using Microsoft.EntityFrameworkCore;
using SupplierManager.Repository.Abstraction;
using SupplierManager.Repository.Model;
using System.Linq;

namespace SupplierManager.Repository
{
	public class Repository(SupplierDbContext dbContext) : IRepository
	{
		public async Task<Supplier?> GetSupplierById(int customerId, CancellationToken ct = default)
		{
			return await dbContext.Suppliers.Where(s => s.Id == customerId).SingleOrDefaultAsync(ct);
		}
	}
}
