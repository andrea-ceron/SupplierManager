using SupplierManager.Repository;
using SupplierManager.Repository.Model;
using SupplierManager.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManager.Business.Abstraction
{
    public interface IBusiness
    {
        public Task CreateSupplierAsync(CreateSupplierDto supplier, CancellationToken ct = default);
        public Task<ReadSupplierDto> GetSupplierAsync(int supplierId, CancellationToken ct = default);

        public Task UpdateSupplierAsync(UpdateSupplierDto supplierDto, CancellationToken ct = default);

		public Task DeleteSupplierAsync(int supplierId, CancellationToken ct = default);
        public Task CreateOrderAsync(CreateOrderDto createOrderDto, CancellationToken ct = default);
        public Task DeleteOrderAsync(int OrderId, CancellationToken ct = default);
        public Task<ReadOrderDto> GetOrderByIdAsync(int OrderId, CancellationToken ct = default);

        public Task<List<ReadOrderDto>?> GetAllOrdersBySupplierIdAsync(int SupplierId, CancellationToken ct = default);

        public Task CreateListOfProductsAsync(IEnumerable<CreateProductDto> productDto, CancellationToken ct = default);
        public Task<List<ReadProductDto>> GetProductListBySupplierId(int SupplierId, CancellationToken ct = default);
        public Task UpdateProductAsync(UpdateProductDto productDto, CancellationToken ct = default);
		public Task DeleteProductAsync(int productId, CancellationToken ct = default);

	}
}
