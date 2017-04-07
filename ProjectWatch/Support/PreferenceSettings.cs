using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWatch.Support
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class PreferenceSettings
	{
		#region Fields
		private DateTime _lastBillingDate = DateTime.MinValue;
		private int _lastPhase = -1;
		private int _lastProject = -1;
		private bool _proVersion = false;
		#endregion
		#region Properties

		//public DateTime LastBillingDate
		//{
		//	get { return _lastBillingDate; }
		//	set { _lastBillingDate = value; }
		//}
		public string InvoicePath { get; set; }
		public int LastPhase
		{
			get { return _lastPhase; }
			set { _lastPhase = value; }
		}

		public int LastProject
		{
			get { return _lastProject; }
			set { _lastProject = value; }
		}
		private bool ProVersion
		{
			get
			{
				return _proVersion;
			}
			set
			{
				_proVersion = value;
			}
		}
		#endregion
	}
}
