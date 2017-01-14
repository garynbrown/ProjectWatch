using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.UI;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class TimecardViewModel : ViewModelCommon
	{
		public override string ViewTitle
		{
			get { return "Time Card Tools"; }
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TimecardViewModel"/> class.
		/// </summary>
		public TimecardViewModel(TimeCard timeCard)
		{
			_currentTimeCard = timeCard;
		}
		// Fields...
		private TimeCard _currentTimeCard;

		public TimeCard CurrentTimeCard
		{
			get { return _currentTimeCard; }
			set
			{
				_currentTimeCard = value;
			}
		}
		
	}
}
