using System.ComponentModel.Composition;

namespace ProjectWatch.Support
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class PreferenceSettings
	{
		
		#region Properties

		public string InvoicePath { get; set; }
		public int LastPhase { get; set; } = -1;

		public int LastProject { get; set; } = -1;

		private bool ProVersion { get; set; } = false;

		#endregion
	}
}
