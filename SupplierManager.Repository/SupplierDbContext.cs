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

		mb.Entity<Product>().HasKey(s => s.Id);
		mb.Entity<Order>().HasKey(s => s.Id);

		base.OnModelCreating(mb);
	}
	public DbSet<Supplier> Suppliers { get; set; }
	public DbSet<Order> Orders { get; set; }
	public DbSet<Product> Products { get; set; }

}
