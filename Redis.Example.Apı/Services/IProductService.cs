using Redis.Example.Apı.Model;

namespace Redis.Example.Apı.Services
{
	public interface IProductService
	{
		Task<List<Product>> GetAsync();

		Task<Product> GetIdAsync(int id);

		Task<Product> CreateAsync(Product product);
	}
}
