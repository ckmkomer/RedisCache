using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed_Caching.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		IDistributedCache _distributedCache;

		public ValuesController(IDistributedCache distributedCache)
		{
			_distributedCache = distributedCache;
		}


		[HttpGet("set")]
		public async Task<IActionResult> Set(string name, string surname)
		{
			// String türündeki 'name' değerini önbelleğe ekler
			await _distributedCache.SetStringAsync("name", name, options: new()
			{
				AbsoluteExpiration = DateTime.Now.AddSeconds(30),
				SlidingExpiration = TimeSpan.FromSeconds(5)
			}); 

			// Byte dizisi olarak 'surname' değerini önbelleğe ekler
			await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname), options: new()
			{
				AbsoluteExpiration = DateTime.Now.AddSeconds(30),
				SlidingExpiration = TimeSpan.FromSeconds(5)
			});

			// Başarılı bir işlem sonucu döndürür
			return Ok();
		}
		/*     Set metodu HTTP GET taleplerini dinler ve "set" yoluyla çağrılır.
    name değeri string türünde _distributedCache üzerine eklenir.
    surname değeri byte dizisi olarak _distributedCache üzerine eklenir.
    İşlem başarıyla tamamlandığında HTTP 200 OK durumu döndürülür.*/

		[HttpGet("get")]
		public async Task<IActionResult> Get()
		{
			// 'name' değerini önbellekten alır
			var name = await _distributedCache.GetStringAsync("name");

			// 'surname' değerini byte dizisi olarak önbellekten alır
			var surnameBinary = await _distributedCache.GetAsync("surname");

			// Byte dizisini string'e çevirerek 'surname' değerini elde eder
			var surname = Encoding.UTF8.GetString(surnameBinary);

			// 'name' ve 'surname' değerlerini içeren bir nesneyi döndürür
			return Ok(new
			{
				name,
				surname,
			});
		}

		
	}
}
