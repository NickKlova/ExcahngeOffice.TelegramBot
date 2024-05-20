using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Cache.Clients.Interfaces;
using ExchangeOffice.Common.Models;
using Newtonsoft.Json;

namespace ExchangeOffice.Application.Managers {
	public class ContactManager : IContactManager {
		private readonly ICacheClient _cache;
		public ContactManager(ICacheClient cache) {
			_cache = cache;
		}

		public async Task DeleteStepperAsync(object key) {
			var stringKey = key.ToString();
			if (string.IsNullOrEmpty(stringKey)) {
				return;
			}
			await _cache.DeleteAsync(stringKey);
		}

		public async Task NextOrFinishStepAsync(object key, StepperInfo? value = null) {
			var stringKey = key.ToString();
			if(string.IsNullOrEmpty(stringKey)) {
				return;
			}
			if (value == null) {
				var valueJson = await _cache.GetAsync(stringKey);
				if (valueJson == null) {
					return;
				}
				value = JsonConvert.DeserializeObject<StepperInfo>(valueJson);
			}
			value!.CurrentStep++;
			if (value.CurrentStep > value.StepsCount) {
				await _cache.DeleteAsync(stringKey);
				return;
			}
			var jsonValue = JsonConvert.SerializeObject(value);
			await _cache.SetAsync(stringKey, jsonValue);
		}

		public void SetData(string id, string str) {
			_cache.SetAsync(id, str).Wait();
		}
	} 
}
