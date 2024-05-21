using ExchangeOffice.Common.Models;
using Newtonsoft.Json;

namespace ExchangeOffice.Application.Managers.Interfaces {
	public interface IContactManager {
		public Task FillFullnameAsync(string chatId, string fullName);

		public Task FillEmailAsync(string chatId, string email);

		public Task FillPhoneAsync(string chatId, string phone);

		public Task CreateContactAsync(string chatId);
	}
}
