using StackExchange.Redis;

namespace Redis.Sentinel.Services
{
	public class RedisService
	{
		 static ConfigurationOptions sentinelOptions=> new()
		{
			EndPoints = 
			{
				{"locahost",6379},
				{"locahost",6380},
				{"locahost",6381}
			},

			CommandMap= CommandMap.Sentinel,
			AbortOnConnectFail = false
		};

	static	ConfigurationOptions masterOptions => new()
		{
			AbortOnConnectFail = false
		};

	static	public async Task<IDatabase> RedisMasterDatabase()
		{
			ConnectionMultiplexer sentinelConnnection = await ConnectionMultiplexer.SentinelConnectAsync(sentinelOptions);

			System.Net.EndPoint masterEndPoint = null;
			foreach (var item in sentinelConnnection.GetEndPoints())
			{
				IServer server = sentinelConnnection.GetServer(item);
				if (!server.IsConnected)
					continue;

			masterEndPoint= await server.SentinelGetMasterAddressByNameAsync("mymaster");
				break;
				
			}

			if (masterEndPoint != null)
			{
				var localMasterIP = masterEndPoint.ToString() switch
				{
					"172.18.0.2:6379" => "localhost:6379",
					"172.18.0.3:6379" => "localhost:6380",
					"172.18.0.4:6379" => "localhost:6381",
					"172.18.0.5:6379" => "localhost:6382",
					_ => throw new NotImplementedException("Unhandled masterEndPoint.")
				};
				ConnectionMultiplexer masterConnection = await ConnectionMultiplexer.ConnectAsync(localMasterIP);
				IDatabase database = masterConnection.GetDatabase();






				return database;


			}

			return await RedisMasterDatabase();
		}

		
	}
}
