using SupplierManager.Repository.Model;
using SupplierManager.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManager.Business.Abstraction
{
    public interface IBusiness
    {
        public Task CreateSupplierAsync(SupplierDto supplier, List<Product> productList, CancellationToken ct = default);
        public Task<SupplierDto> GetSupplierAsync(int supplierId, CancellationToken ct = default);
        public Task UpdateSupplierAsync(SupplierDto supplier, List<Product> ProductsToUpdate, CancellationToken ct = default );
        public Task DeleteSupplierAsync(int supplierId, CancellationToken ct = default);
	}
}
