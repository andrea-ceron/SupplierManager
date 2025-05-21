using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SupplierManager.Business;
using SupplierManager.Business.Abstraction;
using SupplierManager.Shared;

namespace SupplierManager.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SupplierController(IBusiness business, ILogger<SupplierController> logger) : Controller
{
	private readonly IBusiness _business = business;
	private readonly ILogger<SupplierController> _logger = logger;



	[HttpPost(Name = "CreateSupplier")]
	public async Task<ActionResult> CreateSupplier(SupplierDto supplier)
	{

		await _business.CreateSupplierAsync(supplier);
		return Ok();


	}

	[HttpGet(Name = "ReadSupplier")]
	public async Task<ActionResult> GetSupplier(int SupplierId)
	{
		SupplierDto? Supplier = await _business.GetSupplierAsync(SupplierId);
		if (Supplier == null) return NotFound("Supplier non trovato");
		return Ok(Supplier);
	}

	[HttpPut(Name = "UpdateSupplier")]
	public async Task<ActionResult> UpdateSupplier(SupplierDto Supplier)
	{
		await _business.UpdateSupplierAsync(Supplier, Supplier.AddingAddress, Supplier.RemovingAddress);
		return Ok();
	}

	[HttpDelete(Name = "DeleteSupplier")]
	public async Task<ActionResult> DeleteSupplier(int SupplierId)
	{
		await _business.DeleteSupplierAsync(SupplierId);
		return Ok();
	}

}
