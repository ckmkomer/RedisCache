using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Redis.Example.Apı.Model;
using Redis.Example.Apı.Repositors;
using Redis.Example.Apı.Services;
using RedisApıService;

namespace Redis.Example.Apı.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{

		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
		return Ok (await _productService.GetAsync());
			
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			await _productService.GetIdAsync(id);
			return Ok();
		}


		[HttpPost]
		public async Task<IActionResult> Create(Product product)
		{
			return Created(String.Empty,await _productService.CreateAsync(product));
			
		}
	}
}
