using AutoMapper;
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Cache.Clients.Interfaces;
using ExchangeOffice.Common.Models;
using ExchangeOffice.Core.Clients.Interfaces;
using ExchangeOffice.Core.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOffice.Application.Managers {
	public class RateManager : IRateManager {
		private readonly ICacheClient _cache;
		private readonly string _uniq = "RatesTempData:";
		private readonly IRateService _service;
		private readonly IMapper _mapper;
		public RateManager(ICacheClient cache, IRateService service, IMapper mapper) {
			_cache = cache;
			_service = service;
			_mapper = mapper;
		}

		public async Task<IEnumerable<RateDto>> GetRates(string key) {
			var daos = await _service.GetRates();
			var dtos = _mapper.Map<IEnumerable<RateDto>>(daos);
			var chacheKey = _uniq + key;
			var json = JsonConvert.SerializeObject(dtos, Formatting.Indented);
			await _cache.SetAsync(chacheKey, json);
			return dtos;
		}

		public async Task<IEnumerable<CurrencyDto>> GetAcceptedCurrencies(string key) {
			var chacheKey = _uniq + key;
			var json = await _cache.GetAsync(chacheKey);
			var dtos = JsonConvert.DeserializeObject<IEnumerable<RateDto>>(json);
			var currencies = dtos.Select(x=>x.BaseCurrency);
			return currencies;
		}
	}
}
