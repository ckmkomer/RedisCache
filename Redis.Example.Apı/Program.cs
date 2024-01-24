using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Redis.Example.Apý.Context;
using Redis.Example.Apý.Repositors;
using Redis.Example.Apý.Repository;
using Redis.Example.Apý.Services;
using RedisApýService;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IProductService,ProductService>();

builder.Services.AddScoped<IProductRepository>(sp =>
{
	var appDbContext = sp.GetRequiredService<AppDbContext>();
	var repostiry= new ProductRepository(appDbContext);
	var redisService =sp.GetRequiredService<RedisService>();
	return new ProductRepositoryWithCasche(repostiry,redisService);

});

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseInMemoryDatabase("myDatabase");
});

builder.Services.AddSingleton<RedisService>(sp =>
{
	return new RedisService(builder.Configuration["CacheOptions:url"]); 
	});

builder.Services.AddSingleton<IDatabase>(sp =>
{
	var redisService = sp.GetService<RedisService>();//redis databse
	return redisService.GetDatabase(0);
});

var app = builder.Build();

using(var scope =app.Services.CreateScope())
{
	var dbContext= scope.ServiceProvider.GetRequiredService<AppDbContext>();
	dbContext.Database.EnsureCreated();
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
