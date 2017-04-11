using System;
using System.Collections.Generic;
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

		public static TimeCard CreateTimeCard(int time_ID)
		{
			return new TimeCard() { TimeId = time_ID };
		}
		public static int MakeTimeCardIdFromDate(DateTime date)
		{
			return date.Year * 10000 + date.Month * 100 + date.Day;

		}


		public TimeCard(DateTime date)
		{
			TimeId = MakeTimeCardIdFromDate(date);
		}

		public TimeCard()
		{
			TimeId = 0;
		}
		#endregion
		public DateTime TimeCardDateTime()
		{
			int year = (int)(TimeId / 10000);
			int month = (int)((TimeId - (year * 10000)) / 100);
			int day = ((TimeId - (year * 10000 + month * 100)));
			return new DateTime(year,month,day);
		}
		#region Primitive Properties

		public List<TimeBlock> TimeBlocks = new List<TimeBlock>();


		[DataMember]
		public int TimeId { get; set; }




		#endregion
		#region Contract_Implementations

		#region Overrides
		[JsonIgnore]
		public override int EntityId
		{
			get { return TimeId; }
			set { }
		}
		#endregion

		#region Properties
		[JsonIgnore]
		public TimeBlock ActiveTaskTimeBlock { get; set; }

		[JsonIgnore]
		public TimeBlock ActiveBreakTimeBlock { get; set; }

		#endregion
		#endregion
		#region Methods

		public void AddTimeBlock(TimeBlock timeBlock)
		{
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
		public void AddWorkBlock(TimeBlock timeBlock)
		{
			if (timeBlock.TimeBlockType != TimeType.Task)
			{
				timeBlock.TimeBlockType = TimeType.Task;
			}
			TimeBlocks.Add(timeBlock);
		}

		protected override IValidator GetValidator()
		{
			return new TimecardValidator();
		}

		public string TimeCardDate()
		{
			int year = (int)(TimeId/10000);
			int month = (int)((TimeId-(year*10000))/100);
			int day = ((TimeId - (year*10000 + month*100)));
			return $"{month}/{day}/{year}";
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
			t.TimeBlocks = new List<TimeBlock>();
			foreach (TimeBlock _timeBlock in TimeBlocks)
			{
				t.TimeBlocks.Add(_timeBlock.Clone() as TimeBlock);
			}

			return t;
		}
	}
}
