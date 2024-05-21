using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOffice.Core.DAO {
	public class Rate {
		public Guid Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime ModifiedOn { get; set; }
		public Currency? BaseCurrency { get; set; }
		public Currency? TargetCurrency { get; set; }
		public decimal SellRate { get; set; }
		public decimal BuyRate { get; set; }
		public bool IsActive { get; set; }
	}
}
