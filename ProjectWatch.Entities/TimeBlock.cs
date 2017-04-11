using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;
using FluentValidation;
using Newtonsoft.Json;

namespace ProjectWatch.Entities
{
	[DataContract]
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class TimeBlock: ClientEntityBase, ICloneable
	{

		#region Factory
		public static TimeBlock CreateWorkBlock(DateTime startTime, DateTime endTime, int projectId, int phaseId)
		{
			TimeBlock t = new TimeBlock(startTime, endTime);
			t.ProjectId = projectId;
			t.PhaseId = phaseId;
			TimeSpan ts = t.GetTimeSpan();
			t.HoursOnTask = ts.Hours + (double) (ts.Minutes/60.0d) + (double) (ts.Seconds/360.0d);
			t.HoursOnBreak = 0.0d;
			return t;
		}
		public static TimeBlock CreateBreakBlock(DateTime startTime, DateTime endTime)
		{
			TimeBlock t = new TimeBlock(startTime, endTime,0);
			t.ProjectId = -1;
			t.PhaseId = -1;
			TimeSpan ts = t.GetTimeSpan();
			t.HoursOnBreak = ts.Hours + (double)(ts.Minutes / 60.0d) + (double)(ts.Seconds / 360.0d);
			t.HoursOnTask = 0.0d;
			return t;
		}
		#endregion

		#region Methods
		public TimeSpan GetTimeSpan()
		{ return new TimeSpan(EndTime.Hour - StartTime.Hour, EndTime.Minute - StartTime.Minute, EndTime.Second - StartTime.Second); }
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="TimeBlock"/> class.
		/// </summary>
		/// <param name="timeBlockType"></param>
		/// <param name="startTime"></param>
		/// <param name="stopTime"></param>
		public TimeBlock( DateTime startTime, DateTime endTime, TimeType timeBlockType = TimeType.Task)
		{
			this.TimeBlockType = timeBlockType;
			this.StartTime = startTime;
			this.EndTime = endTime;
			ProjectId = -1;
			PhaseId = -1;
		}

		public TimeBlock()
		{
			ProjectId = -1;
			PhaseId = -1;

		}
		#endregion
		#region Properties
		[DataMember]
		public DateTime EndTime { get; set; }

		public double HoursOnBreak { get; set; }

		public double HoursOnTask { get; set; }

		public int BillingId { get; set; }
		public string Note { get; set; }

		[DataMember]
		public bool IsBilled { get; set; }
		[DataMember]
		public int PhaseId { get; set; }

		[DataMember]
		public int ProjectId { get; set; }


		[DataMember]
		public DateTime StartTime { get; set; }

		[DataMember]
		public TimeType TimeBlockType { get; set; }
		#endregion

		#region Overrides

		public override int EntityId
		{
			get { return  (int)StartTime.TimeOfDay.TotalSeconds; }
			set {  }
		}
		public override string ToString()
		{
			string blockType = TimeBlockType == TimeType.Task ? "Work" : "Break";
			return $" {blockType} started at {StartTime} and ended at {EndTime}";
		}
		#endregion

		#region Contract_Implementations
		public object Clone()
		{
			TimeBlock t = new TimeBlock(StartTime, EndTime, TimeBlockType);
			t.HoursOnBreak = HoursOnBreak;
			t.HoursOnTask = HoursOnTask;
			t.PhaseId = PhaseId;
			t.ProjectId = ProjectId;
			t.Note = Note;
			t.IsBilled = IsBilled;
			return t;
		}
		#endregion
	}
}
