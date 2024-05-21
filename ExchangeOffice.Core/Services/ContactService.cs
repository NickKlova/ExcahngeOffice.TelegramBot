using AutoMapper;
using ExchangeOffice.Cache.Clients.Interfaces;
using ExchangeOffice.Common.Models;
using ExchangeOffice.Core.Clients.Interfaces;
using ExchangeOffice.Core.DAO;
using ExchangeOffice.Core.Services.Abstractions;
using Newtonsoft.Json;

namespace ExchangeOffice.Core.Clients {
	public class ContactService : BaseService, IContactService {
		private readonly IMapper _mapper;
		private readonly ICacheClient _cache;
		public ContactService(IMapper mapper, ICacheClient cache) {
			_mapper = mapper;
			_cache = cache;
		}
		public async Task<ContactDto?> CreateContactAsync(string cacheId, ContactDto data) {
			var dao = _mapper.Map<Contact>(data);
			dao.IsBlacklist = false;
			var response = await PostAsync("api/contact/create/", dao);
			var resultData = JsonConvert.DeserializeObject<ContactDto>(response);
			if (resultData == null) {
				return null;
			}
			await _cache.SetAsync($"ContactId:{cacheId}", resultData.Id.ToString());
			return resultData; 
		}

		public async Task<ContactDto?> GetContactAsync(string cacheId) {
			var contactId = await _cache.GetAsync($"ContactId:{cacheId}");
			if (string.IsNullOrEmpty(contactId)) {
				return null;
			}
			var chacheId = $"Contact:{contactId}";
			var chachedData = await _cache.GetAsync(chacheId);
			if (!string.IsNullOrEmpty(chachedData)) {
				return JsonConvert.DeserializeObject<ContactDto>(chachedData);
			}
			var json = await GetAsync($"api/contact/get?id={contactId}");
			if (string.IsNullOrEmpty(json)) { return null; }
			await _cache.SetAsync(chacheId, json);
			return JsonConvert.DeserializeObject<ContactDto>(json);
		}
	}
}
