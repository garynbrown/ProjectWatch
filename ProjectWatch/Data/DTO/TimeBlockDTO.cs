using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DTO
{
	public class TimeBlockDTO
	{
		private TimeBlock _timeBlock = null;
		private string _startTime;
		private string _endTime;
		private string _timeType;
		private List<Project> _projects;
		private List<Phase> _phases;
		private string _dtoPhase;
		private string _dtoProject;
		private string _dtoHours;

		public TimeBlockDTO(TimeBlock timeBlock, List<Project> projects, List<Phase> phases )
		{
			TBlock = timeBlock;
			_projects = projects;
			_phases = phases;
		}

		public string StartTime => TBlock.StartTime.ToString("t");

		public string EndTime => TBlock.EndTime.ToString("t");

		public string DtoTimeType => TBlock.TimeBlockType == TimeType.Break ? "Break" : "Task";

		public string DtoPhase
		{
			get
			{
				return ( TBlock.PhaseId == -1) ? "":_phases.Find(p => p.PhaseId == TBlock.PhaseId).PhaseName;
			}
		}

		public string DtoProject
		{
			get { return (TBlock.ProjectId == -1)? "":_projects.Find(p => p.ProjectId == TBlock.ProjectId).Name; }
		}

		public string DtoHours
		{
			get
			{
				TimeSpan t = TBlock.EndTime - TBlock.StartTime;
				//TimeSpan t = new TimeSpan(TBlock.EndTime.Hour - TBlock.StartTime.Hour,
				//	TBlock.EndTime.Minute - TBlock.StartTime.Minute,
				//	TBlock.EndTime.Second - TBlock.StartTime.Second);
				return $"{t.Hours}.{(t.Minutes*10/6):D2}";
			}
		}

		public TimeBlock TBlock
		{
			get { return _timeBlock; }
			set { _timeBlock = value; }
		}
	}
}
