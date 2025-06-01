using AutoMapper;
using SupplierManager.Repository.Model;
using SupplierManager.Shared.DTO;
using System.Diagnostics.CodeAnalysis;


namespace CustomerManager.Business.Profiles;

/// <summary>
/// Marker per <see cref="AutoMapper"/>.
/// </summary>
public sealed class AssemblyMarker
{
	AssemblyMarker() { }
}

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class InputFileProfile : Profile
{
	public InputFileProfile()
	{
		CreateMap<CreateSupplierDto, Supplier>();
		CreateMap<Supplier, ReadSupplierDto>();
		CreateMap<UpdateSupplierDto, Supplier>();

		CreateMap<CreateOrderDto, Order>();
		CreateMap<Order, ReadOrderDto>();
		CreateMap<UpdateOrderDto, Order>();

		CreateMap<CreateProductOrderDto, ProductOrder>();
		CreateMap<ProductOrder, ReadProductOrderDto>();
		CreateMap<UpdateProductOrderDto, ProductOrder>();

		CreateMap<CreateProductDto, Product>();
		CreateMap<Product, ReadProductDto>();
		CreateMap<UpdateProductDto, Product>();
	}
}