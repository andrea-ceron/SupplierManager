using Microsoft.EntityFrameworkCore;
using SupplierManager.Business;
using SupplierManager.Business.Abstraction;
using SupplierManager.Repository;
using SupplierManager.Repository.Abstraction;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SupplierDbContext>(options => options.UseSqlServer("name=ConnectionStrings:SupplierDbContext", b => b.MigrationsAssembly("SupplierManager.Api")));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBusiness, Business>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
	try
	{
	var db = scope.ServiceProvider.GetRequiredService<SupplierDbContext>();
		db.Database.Migrate();
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
	}
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
