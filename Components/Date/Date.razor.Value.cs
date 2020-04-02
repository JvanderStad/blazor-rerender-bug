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
			set => SetValue(value);
		}

		[Parameter]
		public EventCallback<DateTime?> ValueChanged { get; set; }


		private void ValueChangedInvoke(DateTime? value)
		{
			_value = value;
			Logger.LogError("Update Value: {_value}", _value);
			ValueChanged.InvokeAsync(_value);
		}

		public void SetValue(DateTime? value)
		{
			if (value == _value)
				return;

			Logger.LogError("SetValue: {value}", value);

			DisplayValueChangedInvoke(GetDisplayValue(value));
			ValueChangedInvoke(value);

			if (ClearError())
				HasErrorsChangedInvoke();
		}
	}
}
