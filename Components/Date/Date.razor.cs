using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace BlazorBug
{
	public partial class Date
    {
		[CascadingParameter]
		public Container Container { get; set; }

		[Inject]
		public ILogger<Date> Logger { get; set; }


		protected override void OnInitialized()
		{
			if (Container != null)
				Container.RegisterChild(this);
		}
		public void Dispose()
		{
			if (Container != null)
				Container.UnregisterChild(this);
		}
	}
}
