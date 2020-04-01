using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Blazor
{
	public partial class InputControlContainer
	{
		private HashSet<ComponentBase> Children { get; set; } = new HashSet<ComponentBase>();

		public Action<HashSet<ComponentBase>> ChildrenChanged { get; set; }

		public void RegisterChild(ComponentBase child)
		{
			Children.Add(child);
			ChildrenChanged?.Invoke(Children);
		}
		public void UnregisterChild(ComponentBase child)
		{
			Children.Remove(child);
			ChildrenChanged?.Invoke(Children);
		}

		public List<InputControlError> Errors { get; set; } = new List<InputControlError>();


		public Action<List<InputControlError>> ErrorsChanged { get; set; }


		internal void UpdateErrors(InputControlDate inputControlBase)
		{
			Errors = Children.OfType<InputControlDate>().SelectMany(x => x.Errors).ToList();
			ErrorsChanged?.Invoke(Errors);
		}
	}
}
