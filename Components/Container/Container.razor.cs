using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace BlazorBug
{
	/// <summary>
	/// Children can notify this parent container of their existance and notify
	/// if there are errors
	/// </summary>
	public partial class Container
	{
		[Parameter]
		public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// All the children present in this container
		/// </summary>
		private HashSet<ComponentBase> Children { get; set; } = new HashSet<ComponentBase>();

		public EventCallback<HashSet<ComponentBase>> ChildrenChanged { get; set; }

		public void RegisterChild(ComponentBase child)
		{
			Children.Add(child);
			ChildrenChanged.InvokeAsync(Children);
		}
		public void UnregisterChild(ComponentBase child)
		{
			Children.Remove(child);
			ChildrenChanged.InvokeAsync(Children);
		}

		public List<InputControlError> Errors { get; set; } = new List<InputControlError>();


		public EventCallback<List<InputControlError>> ErrorsChanged { get; set; }


		internal void UpdateErrors(Date inputControlBase)
		{
			Errors = Children.OfType<Date>().SelectMany(x => x.Errors).ToList();
			ErrorsChanged.InvokeAsync(Errors);
		}
	}
}
