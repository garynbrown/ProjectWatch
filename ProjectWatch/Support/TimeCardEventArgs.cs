using ProjectWatch.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWatch.Support
{
	public class TimeCardEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TimeCardEventArgs"/> class.
		/// </summary>
		public TimeCardEventArgs(TimeCard timecard, bool isNew, bool hasErrors)
		{
			Timecard = timecard;
			IsNew = isNew;
			HasErrors = hasErrors;
		}
		// Fields...

		public TimeCard Timecard { get; set; }
		public bool HasErrors { get; set; }
		public bool IsNew { get; set; }
	}
}
