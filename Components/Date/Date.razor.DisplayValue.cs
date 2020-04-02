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
			set => SetDisplayValue(value);
		}

		[Parameter]
		public EventCallback<string> DisplayValueChanged { get; set; }

		/// <summary>
		/// DisplayValue property setter
		/// </summary>
		/// <param name="value"></param>
		public void SetDisplayValue(string value)
		{
			if (value == _displayValue)
				return;

			Logger.LogWarning("SetDisplayValue: {value}", value);

			// Can we parse the text value into a DateTime?
			var parsed = ParseDisplayValue(value);
			if (parsed.InputControlError != null)
			{
				Logger.LogError("Could not parse value {value}", value);

				// Could not parse
				// Continue to update _displayValue to the unparseble text value
				DisplayValueChangedInvoke(value);
				
				// Set the Value to null and update bindings
				ValueChangedInvoke(default);

				// Set the error
				SetError(parsed.InputControlError);
				
				// Notify error state has changed
				HasErrorsChangedInvoke();
				return;
			}

			// Change _displayvalue to the parsed value and update bindings
			DisplayValueChangedInvoke(parsed.DisplayValue);
			
			// Change DateTime _value to parsed value and update bindings
			ValueChangedInvoke(parsed.Value);

			// Notify error state has changed
			if (ClearError())
				HasErrorsChangedInvoke();
		}

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

		/// <summary>
		/// Parse the user input
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public virtual ParseDisplayValueResult ParseDisplayValue(string value)
		{
			if (String.IsNullOrEmpty(value))
			{
				return new ParseDisplayValueResult
				{
					DisplayValue = "",
					Value = default
				};
			}

			try
			{
				var parsed = DateTime.Parse(value);
				return new ParseDisplayValueResult
				{
					DisplayValue = GetDisplayValue(parsed),
					Value = parsed
				};
			}
			catch (Exception exception)
			{
				return new ParseDisplayValueResult
				{
					InputControlError = new InputControlError
					{
						Message = $"Error parsing value '{value}'",
						Details = exception.Message,
					}
				};
			}
		}

		/// <summary>
		/// Convert the value into a display value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public virtual string GetDisplayValue(DateTime? value)
		{
			if (value == null)
				return null;

			return value.Value.ToShortDateString();
		}
	}
}
