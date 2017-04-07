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
using Newtonsoft.Json;

namespace ProjectWatch.Entities
{
	public enum TimeType
	{
		Break = 0,
		Task = 1
	}
	[DataContract]
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class TimeCard : ClientEntityBase, IIdentifiableEntity, ICloneable
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
		public static int MakeTimeCardIdFromDate(DateTime date)
		{
			return date.Year * 10000 + date.Month * 100 + date.Day;

		}

		public string TimeCardDate()
		{
			int year = (int)(TimeId/10000);
			int month = (int)((TimeId-(year*10000))/100);
			int day = ((TimeId - (year*10000 + month*100)));
			return $"{month}/{day}/{year}";
		}

		public DateTime TimeCardDateTime()
		{
			int year = (int)(TimeId / 10000);
			int month = (int)((TimeId - (year * 10000)) / 100);
			int day = ((TimeId - (year * 10000 + month * 100)));
			return new DateTime(year,month,day);
		}
		public TimeCard(DateTime date)
		{
			TimeId = MakeTimeCardIdFromDate(date);
		}

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
		//public List<TimeBlock> WorkBlocks = new List<TimeBlock>();
		//public List<TimeBlock> BreakBlocks = new List<TimeBlock>();
		public List<TimeBlock> TimeBlocks = new List<TimeBlock>();
		private int _timeId;


		[DataMember]
		public int TimeId
		{
			get { return  _timeId; }
			set { _timeId = value; }
		}


		//[DataMember]
		//public double HoursOnTask
		//{
		//	get { return ActiveTaskTimeBlock.HoursOnTask; }
		//	set
		//	{
		//		ActiveTaskTimeBlock.HoursOnTask = value;
		//		RaisePropertyChanged(() => HoursOnTask);
		//	}
		//}


		//[DataMember]
		//public double HoursOnBreak
		//{
		//	get { return ActiveBreakTimeBlock.HoursOnBreak; }
		//	set
		//	{
		//		ActiveBreakTimeBlock.HoursOnBreak = value;
		//		RaisePropertyChanged(() => HoursOnBreak);

		//	}
		//}



		//[DataMember]
		//public string Note
		//{
		//	get { return _note; }
		//	set
		//	{
		//		_note = value;
		//	}
		//}
		public void AddWorkBlock(TimeBlock timeBlock)
		{
			if (timeBlock.TimeBlockType != TimeType.Task)
			{
				timeBlock.TimeBlockType = TimeType.Task;
			}
				TimeBlocks.Add(timeBlock);
		}

		public void AddBreakBlock(TimeBlock timeBlock)
		{
			if (timeBlock.TimeBlockType != TimeType.Break)
			{
				timeBlock.TimeBlockType = TimeType.Break;
			}
				TimeBlocks.Add(timeBlock);
		}
		public void AddTimeBlock(TimeBlock timeBlock)
		{
			TimeBlocks.Add(timeBlock);
		}

		//public void AddWorkBlock(DateTime startTime, DateTime stopTime)
		//{
		//	TimeBlock workBlock = new TimeBlock(startTime, stopTime, 1);
		//	WorkBlocks.Add(workBlock);
		//}

		//public void AddBreakBlock(DateTime startTime, DateTime stopTime)
		//{
		//	TimeBlock breakBlock = new TimeBlock(startTime,stopTime, 0);
		//	BreakBlocks.Add(breakBlock);
		//}
		#endregion
		#region Contract_Implementations

		[JsonIgnore]
		public override int EntityId
		{
			get { return TimeId; }
			set { }
		}

		#region Properties
		[JsonIgnore]
		public TimeBlock ActiveTaskTimeBlock
		{
			get { return _activeTaskTimeBlock; }
			set { _activeTaskTimeBlock = value; }
		}
		[JsonIgnore]
		public TimeBlock ActiveBreakTimeBlock
		{
			get { return _activeBreakTimeBlock; }
			set { _activeBreakTimeBlock = value; }
		}

		//public int ProjectId { get; set; }
		//public int PhaseId { get; set; }
		#endregion
		#endregion
		#region Methods


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

		public object Clone()
		{
			TimeCard t = new TimeCard();
			t.TimeId = TimeId;
			//t.PhaseId = PhaseId;
			//t.ProjectId = ProjectId;
			t.TimeBlocks = new List<TimeBlock>();
			foreach (TimeBlock _timeBlock in TimeBlocks)
			{
				t.TimeBlocks.Add(_timeBlock.Clone() as TimeBlock);
			}

			return t;
		}
	}
}
