using ExchangeOffice.Cache.Clients;
using ExchangeOffice.Cache.Clients.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ExchangeOffice.Cache.Extensions {
	public static class CacheExtensions {
		public static void AddCacheLayer(this IServiceCollection services) {
			services.AddSingleton(provider => {
				var redisConnectionString = "127.0.0.1:6379";
				var redis = ConnectionMultiplexer.Connect(redisConnectionString);
				var db = redis.GetDatabase(10);
				return db;
			});
			services.AddSingleton<ICacheClient, RedisClient>();
		}
	}
}
