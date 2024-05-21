using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Cache.Clients.Interfaces;
using ExchangeOffice.Common.Models;
using ExchangeOffice.Core.Clients.Interfaces;
using Newtonsoft.Json;

namespace ExchangeOffice.Application.Managers {
	public class ContactManager : IContactManager {
		private readonly ICacheClient _cache;
		private readonly string _uniq = "contactmanager";
		private readonly IContactService _service;
		public ContactManager(ICacheClient cache, IContactService service) {
			_cache = cache;
			_service = service;
		}

		public async Task FillFullnameAsync(string chatId, string fullName) {
			var contactEntity = new ContactDto();
			contactEntity.FullName = fullName;
			var json = JsonConvert.SerializeObject(contactEntity);

			var key = chatId + _uniq;
			await _cache.SetAsync(key, json);
		}

		public async Task FillEmailAsync(string chatId, string email) {
			var key = chatId + _uniq;
			var json = await _cache.GetAsync(key);
			var contactEntity = JsonConvert.DeserializeObject<ContactDto>(json);
			contactEntity.Email = email;
			var contactjson = JsonConvert.SerializeObject(contactEntity);
			await _cache.SetAsync(key, contactjson);
		}

		public async Task FillPhoneAsync(string chatId, string phone) {
			var key = chatId + _uniq;
			var json = await _cache.GetAsync(key);
			var contactEntity = JsonConvert.DeserializeObject<ContactDto>(json);
			contactEntity.Phone = phone;
			var contactjson = JsonConvert.SerializeObject(contactEntity);
			await _cache.SetAsync(key, contactjson);
		}

		public async Task CreateContactAsync(string chatId) {
			var key = chatId + _uniq;
			var json = await _cache.GetAsync(key);
			var contactEntity = JsonConvert.DeserializeObject<ContactDto>(json);
			await _service.CreateContactAsync(chatId, contactEntity);
		}

		public async Task<ContactDto> GetContactAsync(string chatId) {
			return await _service.GetContactAsync(chatId);
		}
	} 
}
