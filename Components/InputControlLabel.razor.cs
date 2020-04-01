using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Blazor
{
	public partial class InputControlLabel
	{
		[Parameter]
		public RenderFragment ChildContent { get; set; }

		public bool HasAttention { get; set; }

		[Parameter]
		public string Label { get; set; }

		public void ChildrenChanged(HashSet<ComponentBase> children)
		{
		
		}

		public void ErrorsChanged(List<InputControlError> errors)
		{
		}

		protected override void OnInitialized()
		{
			InputControlContainer.ChildrenChanged += ChildrenChanged;
			InputControlContainer.ErrorsChanged += ErrorsChanged;

		}
		public void Dispose()
		{
			InputControlContainer.ChildrenChanged -= ChildrenChanged;
			InputControlContainer.ErrorsChanged -= ErrorsChanged;
		}
	}
}
