using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor
{
	public partial class InputControlDate
    {
		public string CssClass => HasErrors ? "has-errors" : "";


		protected override void OnInitialized()
		{
			RegisterContainer();
		}
		public void Dispose()
		{
			UnRegisterContainer();
		}



		private string _displayValue { get; set; }


		public string DisplayValue
		{
			get => _displayValue;
			set => SetDisplayValue(value);
		}

		[Parameter]
		public EventCallback<string> DisplayValueChanged { get; set; }


		public void SetDisplayValue(string value)
		{
			var errorsChanged = false;
			if (value == _displayValue)
				return;

			var parsed = ParseDisplayValue(value);
			if (parsed.InputControlError != null) 
			{
				_displayValue = value;
				DisplayValueChanged.InvokeAsync(_displayValue);

				_value = default;
				ValueChangedInvoke();

				SetError(parsed.InputControlError);
				HasErrorsChangedInvoke();
				return;
			}
			else
			{
				_displayValue = parsed.DisplayValue;
				DisplayValueChanged.InvokeAsync(_displayValue);

				_value = parsed.Value;

				errorsChanged |= ClearError();
			}

			ValueChangedInvoke();

			errorsChanged |= !ValidateValue();

			if (errorsChanged)
				HasErrorsChangedInvoke();

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

		public class ParseDisplayValueResult
		{
			public string DisplayValue { get; set; }
			public DateTime? Value { get; set; }
			public InputControlError InputControlError { get; set; }
		}


		
		public virtual string GetDisplayValue(DateTime? value)
		{
			if (value == null)
				return null;

			return value.Value.ToShortDateString();
		}


		
		public virtual DateTime? TransformValue(DateTime? value)
		{
			return value;
		}

		private DateTime? _value { get; set; }
		[Parameter]
		public DateTime? Value
		{
			get => _value;
			set => SetValue(value);
		}

		[Parameter]
		public EventCallback<DateTime?> ValueChanged { get; set; }


		private void ValueChangedInvoke()
		{
			ValueChanged.InvokeAsync(_value);
			ValueChangedCallback?.Invoke(_value);
		}

		[Parameter]
		public Action<DateTime?> ValueChangedCallback { get; set; }



		public void SetValue(DateTime? value)
		{
			if (value == _value)
				return;

			var errorsChanged = ClearError();

			_value = TransformValue(value);
			_displayValue = GetDisplayValue(_value);
			DisplayValueChanged.InvokeAsync(_displayValue);

			ValueChangedInvoke();

			errorsChanged |= !ValidateValue();

			if (errorsChanged)
				HasErrorsChangedInvoke();
		}


		private bool ValidateValue()
		{
			return false;
		}

	
		public List<InputControlError> Errors { get; set; } = new List<InputControlError>();

	
		[Parameter]
		public bool HasErrors { get; set; }

		[Parameter]
		public EventCallback<bool> HasErrorsChanged { get; set; }

		
		protected bool ClearError()
		{
			if (Errors.Count == 0)
				return false;

			Errors.Clear();
			return true;
		}
		protected bool SetError(InputControlError error)
		{
			var changed = ClearError();
			changed |= AddError(error);
			return changed;
		}
		protected bool AddError(InputControlError error)
		{
			if (error == null)
				return false;

			Errors.Add(error);
			return true;
		}

		
		protected string ErrorSummary { get; set; }

		protected void HasErrorsChangedInvoke()
		{
			HasErrors = Errors.Count > 0;
			HasErrorsChanged.InvokeAsync(HasErrors);

			ErrorSummary = String.Join("\n", Errors.Select(x => $"{x.Message}{(String.IsNullOrEmpty(x.Details) ? "" : $" ({x.Details})")}"));

			if (Container != null)
			{
				Container.UpdateErrors(this);
			}
		}

		
		[CascadingParameter(Name = "InputControlContainer")]
		public InputControlContainer Container { get; set; }

		public void RegisterContainer()
		{
			if (Container != null)
				Container.RegisterChild(this);
		}
		public void UnRegisterContainer()
		{
			if (Container != null)
				Container.UnregisterChild(this);
		}
	}

	public class InputControlError
	{
		public string Message { get; set; }
		public string Details { get; set; }
	}
}
