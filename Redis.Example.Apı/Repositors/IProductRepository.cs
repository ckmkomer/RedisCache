using Redis.Example.Apı.Model;

namespace Redis.Example.Apı.Repositors
{
	public interface IProductRepository
	{
		Task<List<Product>> GetAsync();

		Task<Product> GetIdAsync(int id);

		Task<Product> CreateAsync(Product product  );
	}
}
