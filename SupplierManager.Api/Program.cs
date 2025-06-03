using Microsoft.EntityFrameworkCore;
using SupplierManager.Api.Middlewares;
using SupplierManager.Business;
using SupplierManager.Business.Abstraction;
using SupplierManager.Repository;
using SupplierManager.Repository.Abstraction;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SupplierDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("SupplierDbContext"),
		b => b.MigrationsAssembly("SupplierManager.Api")
	)
);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBusiness, Business>();
builder.Logging.AddConsole();

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
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
