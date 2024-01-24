using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Formats.Tar;

namespace InMemory_Caching.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		readonly IMemoryCache _memoryCache;

		public ValuesController(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}


		//[HttpGet("set/{name}")]
		//public void setName(string name)
		//{
		//	_memoryCache.Set("name",name);

		//}


		//[HttpGet]
		//public string GetName()
		//{
		//	if (_memoryCache.TryGetValue<string>("name", out string name))
		//	{
		//		return name.Substring(3);
		//	}
		//	return "";
		//	//return _memoryCache.Get<string>("name");
		//	//return name.Substring(3);
		//}

		[HttpGet("setDate")]
		public void SetDate()
		{
			// Bellek önbelleğine bir DateTime değeri ekleniyor
			_memoryCache.Set<DateTime>("date", DateTime.Now, options: new()
			{
				// Belirtilen tarihe kadar mutlak bir süre belirleniyor (30 saniye sonra)
				AbsoluteExpiration = DateTime.Now.AddSeconds(30),

				// Kayıt oluşturulduktan sonra 5 saniye boyunca kullanılmadığı takdirde silinecek
				SlidingExpiration = TimeSpan.FromSeconds(5)
			});
		}

		/* SetDate metodu HTTP GET taleplerini dinliyor ve "setDate" yoluyla çağrılıyor.
	   Bellek önbelleğine "date" adında bir anahtarla mevcut tarih ve saat(DateTime.Now) değeri ekleniyor.
	   AbsoluteExpiration ile belirtilen tarihe kadar bu değer önbellekte kalacak ve SlidingExpiration ile belirtilen süre boyunca kullanılmadığı takdirde silinecek.*/

		[HttpGet]
		public DateTime GetDate()
		{
			// Bellek önbelleğinden "date" anahtarıyla kaydedilmiş DateTime değeri alınıyor
			return _memoryCache.Get<DateTime>("date");
		}

		/*GetDate metodu HTTP GET taleplerini dinliyor.
	 Bellek önbelleğinden "date" adındaki anahtarla ilişkilendirilmiş DateTime değeri alınıp geri döndürülüyor.*/

	}
}
