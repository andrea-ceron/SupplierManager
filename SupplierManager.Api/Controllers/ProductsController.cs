﻿using Microsoft.AspNetCore.Mvc;
using SupplierManager.Business.Abstraction;
using SupplierManager.Shared.DTO;

namespace SupplierManager.Api.Controllers;


[ApiController]
[Route("[controller]/[action]")]
public class ProductController(IBusiness business, ILogger<ProductController> logger) : Controller
{
	private readonly IBusiness _business = business;
	private readonly ILogger<ProductController> _logger = logger;



	[HttpPost(Name = "CreateListOfProducts")]
	public async Task<ActionResult> CreateProduct(IEnumerable<CreateProductDto> payload)
	{
		await _business.CreateListOfProductsAsync(payload);
		return Ok();
	}

	[HttpGet(Name = "ReadProductListOfSupplier")]
	public async Task<ActionResult<ReadProductDto>> GetProductListOfSupplier(int SupplierId)
	{
		List<ReadProductDto>? productListDto = await _business.GetProductListBySupplierId(SupplierId);
		if (productListDto == null) return NotFound("Supplier non trovato");
		return Ok(productListDto);
	}

	[HttpPut(Name = "UpdateProductList")]
	public async Task<ActionResult> UpdateProductList(List<UpdateProductDto> payload)
	{
		await _business.UpdateListOfProductsAsync(payload);
		return Ok();
	}

	[HttpDelete(Name = "DeleteProduct")]
	public async Task<ActionResult> DeleteProduct(int SupplierId)
	{
		await _business.DeleteProductAsync(SupplierId);
		return Ok();
	}

}
