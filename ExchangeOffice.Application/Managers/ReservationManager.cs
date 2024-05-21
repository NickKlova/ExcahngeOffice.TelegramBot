using AutoMapper;
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Cache.Clients.Interfaces;
using ExchangeOffice.Common.Models;
using ExchangeOffice.Common.Models.Temp;
using ExchangeOffice.Core.Services.Interfaces;
using Newtonsoft.Json;

namespace ExchangeOffice.Application.Managers {
	public class ReservationManager : IReservationManager {
		private readonly ICacheClient _cache;
		private readonly IRateService _service;
		private readonly IMapper _mapper;
		private readonly string _uniq;
		private readonly string _uniqFill;
		public ReservationManager(ICacheClient cache, IRateService service, IMapper mapper) {
			_cache = cache;
			_service = service;
			_mapper = mapper;
			_uniq = "ReservationTempData:";
			_uniqFill = "FillReservationByUser:";
		}

		private string GetCacheAccessKey(string key) {
			return _uniq + key;
		}
		private string GetCacheFillByUserKey(string key) {
			return _uniqFill + key;
		}
		public async Task<IEnumerable<RateDto>> GetRatesAsync(string key) {
			var daos = await _service.GetRates();
			var dtos = _mapper.Map<IEnumerable<RateDto>>(daos);
			var cacheKey = GetCacheAccessKey(key);
			var cachedJson = JsonConvert.SerializeObject(dtos, Formatting.Indented);
			await _cache.SetAsync(cacheKey, cachedJson);
			return dtos;
		}
		public async Task<IEnumerable<CurrencyDto>> GetAcceptedCurrenciesAsync(string key) {
			var cacheKey = GetCacheAccessKey(key);
			var cachedJson = await _cache.GetAsync(cacheKey);
			if (string.IsNullOrEmpty(cachedJson)) {
				return Enumerable.Empty<CurrencyDto>();
			}

			var entities = JsonConvert.DeserializeObject<IEnumerable<RateDto>>(cachedJson);
			if (entities == null) {
				return Enumerable.Empty<CurrencyDto>();
			}
			var acceptedCurrencies = entities
				.Select(x => x.BaseCurrency)
				.Where(x => x != null)
				.Cast<CurrencyDto>();

			return acceptedCurrencies;
		}
		public async Task<IEnumerable<CurrencyDto>> GetReceivedCurrenciesAsync(string key, string currencyId) {
			var cacheKey = GetCacheAccessKey(key);
			var cachedJson = await _cache.GetAsync(cacheKey);
			if (string.IsNullOrEmpty(cachedJson)) {
				return Enumerable.Empty<CurrencyDto>();
			}

			var entities = JsonConvert.DeserializeObject<IEnumerable<RateDto>>(cachedJson);
			if (entities == null) {
				return Enumerable.Empty<CurrencyDto>();
			}
			var receivedCurrencies = entities
				.Where(x => x.BaseCurrency != null && x.BaseCurrency.Id == new Guid(currencyId))
				.Select(x => x.TargetCurrency)
				.Where(x => x != null)
				.Cast<CurrencyDto>();

			return receivedCurrencies;
		}

		public async Task CacheMessage(string key, string messageId) {
			await _cache.SetAsync($"LastBotMsgId:{key}", messageId);
		}

		public async Task<string> GetLastBotMessageId(string key) {
			return await _cache.GetAsync($"LastBotMsgId:{key}");
		}

		public async Task FillAcceptedCurrencyAsync(string key, string currencyId) {
			var cacheKey = GetCacheFillByUserKey(key);

			var tempEntity = new RateTemp() {
				BaseCurrencyId = new Guid(currencyId),
			};
			var tempJson = JsonConvert.SerializeObject(tempEntity);
			await _cache.SetAsync(cacheKey, tempJson);
		}

		public async Task<string> FillReturnedCurrencyAsync(string key, string currencyId) {
			var cacheKey = GetCacheFillByUserKey(key);
			var tempJson = await _cache.GetAsync(cacheKey);
			if (string.IsNullOrEmpty(tempJson)) {
				return string.Empty;
			}

			var tempEntity = JsonConvert.DeserializeObject<RateTemp>(tempJson);
			if (tempEntity == null) {
				return string.Empty;
			}

			var cacheAccessKey = GetCacheAccessKey(key);
			var json = await _cache.GetAsync(cacheAccessKey);
			if (string.IsNullOrEmpty(json)) {
				return string.Empty;
			}

			var rates = JsonConvert.DeserializeObject<IEnumerable<RateDto>>(json);
			if (rates == null) {
				return string.Empty;
			}

			var rate = rates
				.Where(x => x.BaseCurrency != null && x.BaseCurrency.Id == tempEntity.BaseCurrencyId && x.TargetCurrency != null && x.TargetCurrency.Id == new Guid(currencyId))
				.FirstOrDefault();
			var serializableJson = JsonConvert.SerializeObject(rates, Formatting.Indented);
			if (rate == null) {
				return string.Empty;
			}
			await _cache.SetAsync(cacheKey, serializableJson);
			return $"You give: {rate.BaseCurrency.Code}\n" +
				$"You get: {rate.TargetCurrency.Code}\n" +
				$"Rate: {rate.SellRate}";
		}

		public async Task<RateDto> GetUserFilledRate(string key) {
			var cacheKey = GetCacheFillByUserKey(key);

			var json = await _cache.GetAsync(cacheKey);
			var entity = JsonConvert.DeserializeObject<RateDto>(json);
			return entity;
		}

		//pubic async Task CreateReservation
	}
}
