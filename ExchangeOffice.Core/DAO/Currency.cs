using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOffice.Core.DAO {
	public class Currency {
		public Guid Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime ModifiedOn { get; set; }
		public string? Code { get; set; }
		public string? Description { get; set; }
		public string? Symbol { get; set; }
		public bool IsActive { get; set; }
	}
}
