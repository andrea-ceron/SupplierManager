using SupplierManager.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManager.Repository.Abstraction
{
    public interface IRepository
    {
		public Task<Supplier> GetSupplierById(int customerId, CancellationToken ct = default);
	}
}
