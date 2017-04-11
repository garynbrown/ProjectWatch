using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ProjectWatch.Data.DataRepositories;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DTO
{
	public class TimeCardDTO
	{
		public TimeCard TCard { get; }
		private readonly List<Project> _projects;
		private readonly List<Company> _companies;
		private readonly List<Phase> _phases;

		public TimeCardDTO(TimeCard timeCard, List<Project> projects , List<Company> companies, List<Phase> phases)
		{
			TCard = timeCard;
			_projects = projects;
			_companies = companies;
			_phases = phases;
			MakeTCDTO();
		}

		public int TimeCardId;
		public string BreakTime;
		private string TC_Date="";

		public string TcDate => TCard.TimeCardDate();

		public string WorkTime { get; set; }

		public List<string> PhaseNames { get; set; } = new List<string>();

		public List<string> ProjectNames { get; set; } = new List<string>();

		public List<string> CompanyNames { get; set; } = new List<string>();

		void MakeTCDTO()
		{
			TimeCardId = TCard.TimeId;
			TimeSpan WorkSpan = new TimeSpan();
			TimeSpan BreakSpan = new TimeSpan();
			string phaseName = "";
			string projectName = "";
			string companyName = "";
			Project projectToCheck = null;
			int indx = -1;
			foreach (TimeBlock _timeBlock in TCard.TimeBlocks)
			{
				if (_timeBlock.TimeBlockType == TimeType.Task)
				{
					WorkSpan = WorkSpan.Add(_timeBlock.EndTime - _timeBlock.StartTime);
				}
				else
				{
					BreakSpan = BreakSpan.Add(_timeBlock.EndTime - _timeBlock.StartTime);
				}
				indx = _phases.FindIndex(p => p.PhaseId == _timeBlock.PhaseId);
				phaseName = (indx > -1) ? _phases[indx].PhaseName : "" ; 
				if ( !string.IsNullOrEmpty(phaseName) && !PhaseNames.Contains(phaseName))
				{
					PhaseNames.Add(phaseName);
				}
				projectToCheck = _projects.Find(p => p.ProjectId == _timeBlock.ProjectId);
				if ( projectToCheck != null && !ProjectNames.Contains(projectToCheck.Name))
				{
					ProjectNames.Add(projectToCheck.Name);
					indx = _companies.FindIndex(c => c.CompanyId == projectToCheck.CompnayId);
					companyName = (indx >-1) ? _companies[indx].CompanyName : "";
					if (!string.IsNullOrEmpty(companyName) && !CompanyNames.Contains(companyName))
					{
						CompanyNames.Add(companyName);
					}
				}

			}
			WorkTime = $"{WorkSpan.Hours}.{WorkSpan.Minutes/60}";
			BreakTime = BreakSpan.ToString();
		}

	}
}
