using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOffice.Application.Attributes {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class TextStepperAttribute : Attribute {
		public string? Name { get; set; }
		public int Step { get; set; }
		public TextStepperAttribute(string sessionName, int stepNumber) {
			Name = sessionName;
			Step = stepNumber;
		}
	}
}
