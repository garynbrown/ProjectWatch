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

namespace ProjectWatch.Entities
{
	[DataContract]
	public class TimeBlock: ObservableObject
	{
		private const int WorkBlockType = 1;
		const int BreakBlockType = 0;
		private int _phaseId;
		private int _projectId;
		private DateTime _endTime;
		private DateTime _startTime;
		private string _note;
		private double _hoursOnTask;
		private double _hoursOnBreak;

		/// <summary>
		/// Initializes a new instance of the <see cref="TimeBlock"/> class.
		/// </summary>
		/// <param name="timeBlockType"></param>
		/// <param name="startTime"></param>
		/// <param name="stopTime"></param>
		public TimeBlock( DateTime startTime, DateTime endTime, int timeBlockType = WorkBlockType)
		{
			this.TimeBlockType = timeBlockType;
			this.StartTime = startTime;
			this.EndTime = endTime;
		}
		[DataMember]
		public int TimeBlockType { get; set; }


		public override string ToString()
		{
			string blockType = TimeBlockType == WorkBlockType ? "Work" : "Break";
			return $" {blockType} started at {StartTime} and ended at {EndTime}";
		}
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
		public DateTime EndTime
		{
			get { return _endTime; }
			set { Set(() => EndTime, ref _endTime, value); }
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
			{ Set(() => StartTime, ref _startTime, value); }
		}

		public double HoursOnTask
		{
			get { return _hoursOnTask; }
			set { _hoursOnTask = value; }
		}

		public double HoursOnBreak
		{
			get { return _hoursOnBreak; }
			set { _hoursOnBreak = value; }
		}

		public string Note
		{
			get { return _note; }
			set { _note = value; }
		}
	}
}
