using ExchangeOffice.Application.Constants;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Cache.Clients.Interfaces;
using ExchangeOffice.Common.Models;
using Newtonsoft.Json;

namespace ExchangeOffice.Application.Handlers.Messages.Abstractions {
	public abstract class BaseMessageHandler {
		private readonly ICacheClient _cacheClient;
		public BaseMessageHandler(IManagerProvider provider) {
			_cacheClient = provider.GetCacheClient();
		}

		protected async Task ConfigureStepperAsync(object key, StepperInfo config) {
			var stringKey = key.ToString();
			if (stringKey == null) {
				throw new Exception("Key can't be null!");
			}
			await NextOrFinishStepAsync(stringKey, config);
		}

		protected async Task DeleteStepperAsync(object key) {
			var stringKey = key.ToString();
			if (string.IsNullOrEmpty(stringKey)) {
				return;
			}
			await _cacheClient.DeleteAsync(stringKey);
		}

		protected async Task NextOrFinishStepAsync(object key, StepperInfo? value = null) {
			var stringKey = key.ToString();
			if (string.IsNullOrEmpty(stringKey)) {
				return;
			}
			if (value == null) {
				var valueJson = await _cacheClient.GetAsync(stringKey);
				if (valueJson == null) {
					return;
				}
				value = JsonConvert.DeserializeObject<StepperInfo>(valueJson);
				if (value == null) {
					return;
				}
			}
			value!.CurrentStep++;
			if (value.CurrentStep > value.StepsCount) {
				await _cacheClient.DeleteAsync(stringKey);
				return;
			}
			var jsonValue = JsonConvert.SerializeObject(value);
			await _cacheClient.SetAsync(stringKey, jsonValue);
		}
	}
}
