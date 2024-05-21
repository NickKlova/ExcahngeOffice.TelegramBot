using ExchangeOffice.Common.Models;

namespace ExchangeOffice.Application.Managers.Interfaces {
	public interface IRateManager {
		public Task<IEnumerable<RateDto>> GetRatesAsync(string key);
		public Task<IEnumerable<CurrencyDto>> GetAcceptedCurrenciesAsync(string key);
		public Task<IEnumerable<CurrencyDto>> GetReceivedCurrenciesAsync(string key, string currencyId);
		public Task FillAcceptedCurrencyAsync(string key, string currencyId);
		public Task<string> FillReturnedCurrencyAsync(string key, string currencyId);
		public Task CacheMessage(string key, string messageId);
		public Task<string> GetLastBotMessageId(string key);
		}
	}
