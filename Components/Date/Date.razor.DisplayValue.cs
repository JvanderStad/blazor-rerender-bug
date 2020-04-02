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


		public void SetDisplayValue(string value)
		{

			if (value == _displayValue)
				return;

			Logger.LogError("SetDisplayValue: {value}", value);

			var parsed = ParseDisplayValue(value);
			if (parsed.InputControlError != null)
			{
				DisplayValueChangedInvoke(value);
				ValueChangedInvoke(default);

				SetError(parsed.InputControlError);
				HasErrorsChangedInvoke();
				return;
			}

			DisplayValueChangedInvoke(parsed.DisplayValue);
			ValueChangedInvoke(parsed.Value);

			if (ClearError())
				HasErrorsChangedInvoke();
		}

		private void DisplayValueChangedInvoke(string displayValue)
		{
			_displayValue = displayValue;
			Logger.LogError("Update DisplayValue: {_value}", _displayValue);
			DisplayValueChanged.InvokeAsync(_displayValue);
		}

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

		public virtual string GetDisplayValue(DateTime? value)
		{
			if (value == null)
				return null;

			return value.Value.ToShortDateString();
		}
	}
}
