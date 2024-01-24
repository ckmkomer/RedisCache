using Redis.Example.Apı.Model;
using Redis.Example.Apı.Repositors;

namespace Redis.Example.Apı.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;

		public ProductService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<Product> CreateAsync(Product product)
		{
                return  await _productRepository.CreateAsync(product);
		}

		public async Task<List<Product>> GetAsync()
		{
			return await _productRepository.GetAsync();
		}

		public async Task<Product> GetIdAsync(int id)
		{
			 var product = await _productRepository.GetIdAsync(id);
			return product;
		}
	}
}
