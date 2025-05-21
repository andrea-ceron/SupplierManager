using SupplierManager.Repository;
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
        public Task CreateSupplierAsync(SupplierDto supplier, List<ProductDto> productList, CancellationToken ct = default);
        public Task<SupplierDto> GetSupplierAsync(int supplierId, CancellationToken ct = default);
        public Task UpdateSupplierAsync(SupplierDto supplier, List<ProductDto> ProductsToUpdate, CancellationToken ct = default );
        public Task DeleteSupplierAsync(int supplierId, CancellationToken ct = default);
        public Task<int> CreateOrderAsync(DateTime delivery, int supplierId, List<ProductOrderDto> products, CancellationToken ct = default);
        public Task DeleteOrderAsync(int OrderId, CancellationToken ct = default);
		public Task<OrderDto> GetOrderByIdAsync(int OrderId, CancellationToken ct = default);
        public Task<List<OrderDto>?> GetAllOrdersBySupplierIdAsync(int SupplierId, CancellationToken ct = default);



	}
}
