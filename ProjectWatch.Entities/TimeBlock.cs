using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Core.Common.Core;
using FluentValidation;

namespace ProjectWatch.Entities
{
	[DataContract]
	class TimeBlock
	{
		const int WorkBlockType = 1;
		const int BreakBlockType = 0;
		/// <summary>
		/// Initializes a new instance of the <see cref="TimeBlock"/> class.
		/// </summary>
		/// <param name="timeBlockType"></param>
		/// <param name="startTime"></param>
		/// <param name="stopTime"></param>
		public TimeBlock( DateTime startTime, DateTime stopTime, int timeBlockType = WorkBlockType)
		{
			this.TimeBlockType = timeBlockType;
			this.StartTime = startTime;
			this.StopTime = stopTime;
		}
		[DataMember]
		public int TimeBlockType { get; set; }

		[DataMember]
		public DateTime StartTime { get; set; }

		[DataMember]
		public DateTime StopTime { get; set; }
		public override string ToString()
		{
			string blockType = TimeBlockType == WorkBlockType ? "Work" : "Break";
			return $" {blockType} started at {StartTime} and ended at {StopTime}";
		}
	}
}
