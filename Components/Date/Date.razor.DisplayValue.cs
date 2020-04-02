using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;

namespace BlazorBug
{
	public partial class Date
    {
		private string _displayValue { get; set; }

		[Parameter]
		public string DisplayValue
		{
			get => _displayValue;
			set
			{
				if (value == _displayValue)
					return;

				Logger.LogWarning("SetDisplayValue: {value}", value);

				// Can we parse the text value into a DateTime?
				try
				{
					var parsed = DateTime.Parse(value);

					DisplayValueChangedInvoke(parsed.ToShortDateString());

					// Change DateTime _value to parsed value and update bindings
					ValueChangedInvoke(parsed);
				}
				catch (Exception)
				{
					Logger.LogError("Could not parse value {value}", value);

					// Could not parse
					// Continue to update _displayValue to the unparseble text value
					DisplayValueChangedInvoke(value);

					// Set the Value to null and update bindings
					ValueChangedInvoke(default);
				}
			}
		}

		[Parameter]
		public EventCallback<string> DisplayValueChanged { get; set; }

		/// <summary>
		/// Update _displayValue and update bindings
		/// </summary>
		/// <param name="displayValue"></param>
		private void DisplayValueChangedInvoke(string displayValue)
		{
			_displayValue = displayValue;
			Logger.LogWarning("Set _displayValue to '{_displayValue}' and update bindings", _displayValue);
			DisplayValueChanged.InvokeAsync(_displayValue);
		}
	}
}
