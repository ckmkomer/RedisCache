using Microsoft.EntityFrameworkCore;
using Redis.Example.Apı.Context;
using Redis.Example.Apı.Model;
using Redis.Example.Apı.Repositors;

namespace Redis.Example.Apı.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly AppDbContext _context;

		public ProductRepository(AppDbContext context)
		{
			_context = context;
		}

		

		public async Task<Product> CreateAsync(Product product)
		{
		await	_context.Products.AddAsync(product);

			await _context.SaveChangesAsync();
			return product;
		}

		public async Task<List<Product>> GetAsync()
		{
			return await _context.Products.ToListAsync();
		}

		public async Task <Product> GetIdAsync(int id)
		{
		return await _context.Products.FindAsync(id);
		}
	}

		
	}

