using ExchangeOffice.Core.DAO;
using ExchangeOffice.Core.Services.Abstractions;
using ExchangeOffice.Core.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOffice.Core.Services {
	public class RateService : BaseService, IRateService {
		public async Task<IEnumerable<Rate>> GetRates() {
			var rates = await GetAsync("api/rate/getall");
			var dao = JsonConvert.DeserializeObject<IEnumerable<Rate>>(rates);
			return dao;
		}
	}
}
