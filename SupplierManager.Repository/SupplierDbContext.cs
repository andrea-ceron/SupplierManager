using Microsoft.EntityFrameworkCore;
using SupplierManager.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManager.Repository;

public class SupplierDbContext(DbContextOptions<SupplierDbContext> options): DbContext(options)
{
	protected override void OnModelCreating(ModelBuilder mb)
	{
		mb.Entity<Supplier>().HasKey(s => s.Id);
		mb.Entity<Supplier>().HasMany(s => s.Orders)
			.WithOne(o => o.Supplier)
			.HasForeignKey(o => o.SupplierId);
		mb.Entity<Supplier>().HasMany(s => s.Products)
			.WithOne(o => o.Supplier)
			.HasForeignKey(o => o.SupplierId);

		mb.Entity<Product>().HasKey(p => p.Id);
		mb.Entity<Product>().HasMany(p => p.ProductOrders)
					.WithOne(po => po.Product)
					.HasForeignKey(po => po.ProductId);

		mb.Entity<Order>().HasKey(o => o.Id);
		mb.Entity<Order>().HasMany(o => o.ProductOrder)
			.WithOne(po => po.Order)
			.HasForeignKey(po => po.OrderId);

		base.OnModelCreating(mb);
	}
	public DbSet<Supplier> Suppliers { get; set; }
	public DbSet<Order> Orders { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<ProductOrder> ProductOrders { get; set; }


}
