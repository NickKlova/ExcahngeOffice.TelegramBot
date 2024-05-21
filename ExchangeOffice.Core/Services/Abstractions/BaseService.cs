using Newtonsoft.Json;
using System.Text;

namespace ExchangeOffice.Core.Services.Abstractions {
	public abstract class BaseService {
		public async Task<string> PostAsync(string url, object data) {
			var body = JsonConvert.SerializeObject(data);
			using (HttpClient client = new HttpClient()) {
				StringContent content = new StringContent(body, Encoding.UTF8, "application/json");

				HttpResponseMessage response = await client.PostAsync(url, content);

				if (response.IsSuccessStatusCode) {
					string responseBody = await response.Content.ReadAsStringAsync();
					return responseBody;
				}
				return string.Empty;
			}
		}

		public async Task<string> GetAsync(string url) {
			using (HttpClient client = new HttpClient()) {
				var response = await client.GetAsync(url);
				if (response.IsSuccessStatusCode) {
					string responseBody = await response.Content.ReadAsStringAsync();
					return responseBody;
				}
				return string.Empty;
			}
		}
	}
}
