using ExchangeOffice.Core.DAO;
using ExchangeOffice.Core.Services.Abstractions;
using ExchangeOffice.Core.Services.Interfaces;
using Newtonsoft.Json;

namespace ExchangeOffice.Core.Services {
	public class RateService : BaseService, IRateService {
		public async Task<IEnumerable<Rate>> GetRates() {
			var rates = await GetAsync("api/rate/getall");
			var dao = JsonConvert.DeserializeObject<IEnumerable<Rate>>(rates);
			return dao;
		}
	}
}
