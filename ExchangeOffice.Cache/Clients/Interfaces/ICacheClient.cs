namespace ExchangeOffice.Cache.Clients.Interfaces {
	public interface ICacheClient {
		public Task<string?> GetAsync(string key);
		public Task SetAsync(string key, string value);
		public Task DeleteAsync(string key);
	}
}
