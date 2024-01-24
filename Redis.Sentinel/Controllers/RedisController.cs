﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Redis.Sentinel.Services;

namespace Redis.Sentinel.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RedisController : ControllerBase
	{
		//localhost:4320/api/redis/setvalue/name/omr
		[HttpGet("[action]/{key}/{value}")]
		public async Task<IActionResult> SetValue(string  key, string value)
		{
			var redis = await RedisService.RedisMasterDatabase();
			await redis.StringSetAsync(key, value);
			return Ok();
		}

		//localhost:4320/api/redis/getvalue/name
		[HttpGet("[action]/{key}")]
		public async Task<IActionResult> GetValue(string key)
		{
			var redis = await RedisService.RedisMasterDatabase();
			await redis.StringGetAsync(key);
			return Ok();
		}
	}
}