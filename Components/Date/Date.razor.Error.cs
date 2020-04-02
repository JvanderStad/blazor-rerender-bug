using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorBug
{
	public partial class Date
	{
		public List<InputControlError> Errors { get; set; } = new List<InputControlError>();

		[Parameter]
		public bool HasErrors { get; set; }

		/// <summary>
		/// Remove errorstate
		/// </summary>
		/// <returns>Has the errorstate changed?</returns>
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

		[Parameter]
		public EventCallback<bool> HasErrorsChanged { get; set; }

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

	}
}