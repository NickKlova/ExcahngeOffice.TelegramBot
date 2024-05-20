using ExchangeOffice.Cache.Clients.Interfaces;
using StackExchange.Redis;

namespace ExchangeOffice.Cache.Clients {
	public class RedisClient : ICacheClient {
		private readonly IDatabase _cache;
		public RedisClient(IDatabase cache) {
			_cache = cache;
		}

		public async Task<string?> GetAsync(string key) {
			return await _cache.StringGetAsync(key);
		}

		public async Task SetAsync(string key, string value) {
			await _cache.StringSetAsync(key, value);
		}

		public async Task DeleteAsync(string key) {
			await _cache.KeyDeleteAsync(key);
		}
	}
}
