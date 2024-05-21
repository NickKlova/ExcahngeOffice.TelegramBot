using ExchangeOffice.Common.Models;

namespace ExchangeOffice.Application.Managers.Interfaces {
	public interface IRateManager {
		public Task<IEnumerable<RateDto>> GetRates(string key);
		public Task<IEnumerable<CurrencyDto>> GetAcceptedCurrencies(string key);
		}
	}
