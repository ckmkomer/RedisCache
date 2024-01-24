using Redis.Example.Apı.Model;
using Redis.Example.Apı.Repository;
using RedisApıService;
using StackExchange.Redis;
using System.Text.Json;

namespace Redis.Example.Apı.Repositors
{
	public class ProductRepositoryWithCasche : IProductRepository
	{
		private const string productKey = "productCaches";
		private readonly IProductRepository _repository;
		private readonly RedisService _redisService;
		private readonly IDatabase _cacheRepository;
		

		

		public ProductRepositoryWithCasche(IProductRepository repository, RedisService redisService)
		{
			_repository = repository;
			_redisService = redisService;
			_cacheRepository = _redisService.GetDatabase(0);
		}

		public async Task<Product> CreateAsync(Product product)
		{

			var newProduct = await _repository.CreateAsync(product);

			if (await _cacheRepository.KeyExistsAsync(productKey))
			{
				await _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(newProduct));
			}

			return newProduct;

		}

		public async Task<List<Product>> GetAsync()
		{
			if (!await _cacheRepository.KeyExistsAsync(productKey))
			{
				return await LoadToCacheFromDbAsync();
			}
			var products = new List<Product>();
			var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);
			foreach( var cacheProduct in cacheProducts.ToList())
			{
				var prduct= JsonSerializer.Deserialize<Product>(cacheProduct.Value);
				products.Add(prduct);
			}
			return products;
		}

		public async Task<Product> GetIdAsync(int id)
		{
			if (_cacheRepository.KeyExists(productKey))
			{
				var product = await _cacheRepository.HashGetAsync(productKey, id);
				return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;

			}

			var products = await LoadToCacheFromDbAsync();
			return products.FirstOrDefault(x => x.Id == id);
		}

		private async Task<List<Product>>LoadToCacheFromDbAsync()
		{
			var products = await _repository.GetAsync();
			products.ForEach(p =>
			{
				_cacheRepository.HashSetAsync(productKey, p.Id,JsonSerializer.Serialize(p));
			});

			return products;
		}



	}
}
