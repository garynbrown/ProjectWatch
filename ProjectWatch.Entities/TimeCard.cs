using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;
using Core.Common.Core;
using FluentValidation;

namespace ProjectWatch.Entities
{
	[DataContract]
	public class TimeCard : ClientEntityBase
	{
		#region Factory Method

		/// <summary>
		/// Create a new TimeCard object.
		/// </summary>
		/// <param name="time_ID">Initial value of the Time_ID property.</param>
		public static TimeCard CreateTimeCard(int time_ID)
		{
			return new TimeCard() { _timeId = time_ID };
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TimeCard"/> class.
		/// </summary>
		/// <param name="HoursOnBreak"></param>
		/// <param name="Note"></param>
		/// <param name="HoursOnTask"></param>
		/// <param name="PhaseId"></param>
		/// <param name="ProjectId"></param>
		/// <param name="EndTime"></param>
		/// <param name="TimeId"></param>
		/// <param name="StartTime"></param>
		public TimeCard(double HoursOnBreak, string Note, double HoursOnTask, int PhaseId, int ProjectId, DateTime EndTime, int TimeId, DateTime StartTime)
		{
			_hoursOnBreak = HoursOnBreak;
			_note = Note;
			_hoursOnTask = HoursOnTask;
			_phaseId = PhaseId;
			_projectId = ProjectId;
			_endTime = EndTime;
			_timeId = TimeId;
			_startTime = StartTime;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TimeCard"/> class.
		/// </summary>
		public TimeCard()
		{
			_hoursOnBreak = 0d;
			_note = String.Empty;
			_hoursOnTask = 0d;
			_phaseId = 0;
			_projectId = 0;
			_endTime = DateTime.MinValue;
			_timeId = 0;
			_startTime = DateTime.MinValue;
		}
		#endregion

		#region Primitive Properties
		private double _hoursOnBreak;
		private string _note;
		private double _hoursOnTask;
		private int _phaseId;
		private int _projectId;
		private DateTime _endTime;
		private int _timeId;
		private DateTime _startTime;
		private List<TimeBlock> workBlocks = new List<TimeBlock>();
		private List<TimeBlock> breakBlocks = new List<TimeBlock>();

		[DataMember]
		public int ProjectId
		{
			get { return _projectId; }
			set
			{
				_projectId = value;
			}
		}
		
		[DataMember]
		public int TimeId
		{
			get { return _timeId; }
			set
			{
				if (_timeId != value)
				{
					_timeId = value;
					RaisePropertyChanged(() => TimeId);
				}
			}
		}

		[DataMember]
		public DateTime EndTime
		{
			get { return _endTime; }
			set
			{
				_endTime = value;
			}
		}

		[DataMember]
		public int PhaseId
		{
			get { return _phaseId; }
			set
			{
				_phaseId = value;
			}
		}
		

		[DataMember]
		public DateTime StartTime
		{
			get { return _startTime; }
			set
			{
				_startTime = value;
				RaisePropertyChanged(() => StartTime);
			}
		}

		[DataMember]
		public double HoursOnTask
		{
			get { return _hoursOnTask; }
			set
			{
				_hoursOnTask = value;
				RaisePropertyChanged(() => HoursOnTask);
			}
		}


		[DataMember]
		public double HoursOnBreak
		{
			get { return _hoursOnBreak; }
			set
			{
				_hoursOnBreak = value;
				RaisePropertyChanged(() => HoursOnBreak);

			}
		}



		[DataMember]
		public string Note
		{
			get { return _note; }
			set
			{
				_note = value;
			}
		}

		public void AddWorkBlock(DateTime startTime, DateTime stopTime)
		{
			TimeBlock workBlock = new TimeBlock(startTime, stopTime, 1);
			workBlocks.Add(workBlock);
		}

		public void AddBreakBlock(DateTime startTime, DateTime stopTime)
		{
			TimeBlock breakBlock = new TimeBlock(startTime,stopTime, 0);
			breakBlocks.Add(breakBlock);
		}
		#endregion
		public class TimecardValidator : AbstractValidator<TimeCard>
		{
			
			public TimecardValidator()
			{
				RuleFor(obj => obj.StartTime).NotEmpty();
				RuleFor(obj => obj.EndTime).NotEmpty();
				RuleFor(obj => obj.StartTime.Day).GreaterThan(DateTime.Now.Day - 2);	// if today is day 10, and start day was day 9 then 9 > (10 - 2)
				RuleFor(obj => obj.EndTime.Day ).GreaterThan(DateTime.Now.Day - 2);		// so stant and end can only be yesterday or today
				RuleFor(obj => (obj.EndTime - obj.StartTime).Seconds).GreaterThan(0);	// StartTime must be before Endtime
			}
		}

		protected override IValidator GetValidator()
		{
			return new TimecardValidator();
		}
	}
}
