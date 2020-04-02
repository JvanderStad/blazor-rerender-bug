using System;

namespace BlazorBug
{
	public class ParseDisplayValueResult
	{
		public string DisplayValue { get; set; }
		public DateTime? Value { get; set; }
		public InputControlError InputControlError { get; set; }
	}
}
