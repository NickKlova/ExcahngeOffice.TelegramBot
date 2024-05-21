using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOffice.Common.Models {
	public class RateDto {
		public Guid Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime ModifiedOn { get; set; }
		public CurrencyDto? BaseCurrency { get; set; }
		public CurrencyDto? TargetCurrency { get; set; }
		public decimal SellRate { get; set; }
		public decimal BuyRate { get; set; }
		public bool IsActive { get; set; }
	}
}
