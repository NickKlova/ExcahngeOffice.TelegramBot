using ExchangeOffice.Common.Models;

namespace ExchangeOffice.Application.Managers.Interfaces {
	public interface IContactManager {
		public Task NextOrFinishStepAsync(object key, StepperInfo? value = null);
		public Task DeleteStepperAsync(object key);
			public void SetData(string id, string str);
	}
}
