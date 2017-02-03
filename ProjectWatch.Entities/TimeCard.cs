using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;
using FluentValidation;

namespace ProjectWatch.Entities
{
	[DataContract]
	public class TimeCard : ClientEntityBase, IIdentifiableEntity
	{
		#region Factory Method

		/// <summary>
		/// Create a new TimeCard object.
		/// </summary>
		/// <param name="time_ID">Initial value of the Time_ID property.</param>
		public static TimeCard CreateTimeCard(int time_ID)
		{
			return new TimeCard() { TimeId = time_ID };
		}

		public TimeCard(DateTime date)
		{
			TimeId = MakeIdFromDate(date);
		}

		///// <summary>
		///// Initializes a new instance of the <see cref="TimeCard"/> class.
		///// </summary>
		///// <param name="HoursOnBreak"></param>
		///// <param name="Note"></param>
		///// <param name="HoursOnTask"></param>
		///// <param name="PhaseId"></param>
		///// <param name="ProjectId"></param>
		///// <param name="EndTime"></param>
		///// <param name="TimeId"></param>
		///// <param name="StartTime"></param>
		//public TimeCard(double hoursOnBreak, string note, double hoursOnTask, int phaseId, int projectId, DateTime endTime, int timeId, DateTime startTime)
		//{

		//	ActiveBreakTimeBlock.HoursOnBreak = hoursOnBreak;
		//	ActiveTaskTimeBlock.Note = note;
		//	ActiveTaskTimeBlock.HoursOnTask = hoursOnTask;
		//	ActiveTaskTimeBlock.PhaseId = phaseId;
		//	ActiveTaskTimeBlock.ProjectId = projectId;
		//	ActiveTaskTimeBlock.EndTime = endTime;
		//	_timeId = timeId;
		//	ActiveTaskTimeBlock.StartTime = startTime;
		//}
		/// <summary>
		/// Initializes a new instance of the <see cref="TimeCard"/> class.
		/// </summary>
		public TimeCard()
		{
			TimeId = 0;
		}
		#endregion

		#region Primitive Properties

		private TimeBlock _activeTaskTimeBlock;
		private TimeBlock _activeBreakTimeBlock;
		private List<TimeBlock> workBlocks = new List<TimeBlock>();
		private List<TimeBlock> breakBlocks = new List<TimeBlock>();
		private int _timeId;


		[DataMember]
		public int TimeId
		{
			get { return  _timeId; }
			private set { _timeId = value; }
		}


		[DataMember]
		public double HoursOnTask
		{
			get { return ActiveTaskTimeBlock.HoursOnTask; }
			set
			{
				ActiveTaskTimeBlock.HoursOnTask = value;
				RaisePropertyChanged(() => HoursOnTask);
			}
		}


		[DataMember]
		public double HoursOnBreak
		{
			get { return ActiveBreakTimeBlock.HoursOnBreak; }
			set
			{
				ActiveBreakTimeBlock.HoursOnBreak = value;
				RaisePropertyChanged(() => HoursOnBreak);

			}
		}



		//[DataMember]
		//public string Note
		//{
		//	get { return _note; }
		//	set
		//	{
		//		_note = value;
		//	}
		//}

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
		#region Contract_Implementations

		public override int EntityId
		{
			get { return TimeId; }
			set { }
		}

		public TimeBlock ActiveTaskTimeBlock
		{
			get { return _activeTaskTimeBlock; }
			set { _activeTaskTimeBlock = value; }
		}

		public TimeBlock ActiveBreakTimeBlock
		{
			get { return _activeBreakTimeBlock; }
			set { _activeBreakTimeBlock = value; }
		}

		protected override IValidator GetValidator()
		{
			return new TimecardValidator();
		}
		public class TimecardValidator : AbstractValidator<TimeCard>
		{
			
			public TimecardValidator()
			{
				//RuleFor(obj => obj.StartTime).NotEmpty();
				//RuleFor(obj => obj.EndTime).NotEmpty();
				//RuleFor(obj => obj.StartTime.Day).GreaterThan(DateTime.Now.Day - 2);	// if today is day 10, and start day was day 9 then 9 > (10 - 2)
				//RuleFor(obj => obj.EndTime.Day ).GreaterThan(DateTime.Now.Day - 2);		// so stant and end can only be yesterday or today
				//RuleFor(obj => (obj.EndTime - obj.StartTime).Seconds).GreaterThan(0);	// StartTime must be before Endtime
			}
		}
		#endregion
		#region Methods
		int MakeIdFromDate(DateTime date)
		{
			return date.Year * 10000 + date.Month * 100 + date.Day;

		}
		#endregion

	}
}
