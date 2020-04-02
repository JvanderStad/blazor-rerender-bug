using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;

namespace BlazorBug
{
	public partial class Date
    {
		private DateTime? _value { get; set; }

		[Parameter]
		public DateTime? Value
		{
			get => _value;
			set
			{
				if (value == _value)
					return;

				Logger.LogWarning("SetValue: {value}", value);

				DisplayValueChangedInvoke(value?.ToShortDateString());
				ValueChangedInvoke(value);
			}
		}

		[Parameter]
		public EventCallback<DateTime?> ValueChanged { get; set; }

		private void ValueChangedInvoke(DateTime? value)
		{
			_value = value;
			Logger.LogWarning("Set _value to '{_value}' and update bindings", _value);
			ValueChanged.InvokeAsync(_value);
		}
	}
}
