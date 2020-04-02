using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace BlazorBug
{
	public partial class Label
	{
		[Parameter]
		public RenderFragment ChildContent { get; set; }

		public bool HasAttention { get; set; }

		[Parameter]
		public string Title { get; set; }
	}
}
