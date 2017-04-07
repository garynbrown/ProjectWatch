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
		private List<Project> _projects;
		private List<Company> _companies;
		private List<Phase> _phases;

		public TimeCardDTO(TimeCard timeCard, List<Project> projects , List<Company> companies, List<Phase> phases)
		{
			TCard = timeCard;
			_projects = projects;
			_companies = companies;
			_phases = phases;
			MakeTCDTO();
		}

		public int TimeCardId;
		private string _workTime;
		public string BreakTime;
		private string TC_Date="";
		//public string TC_Project="";
		//public string TC_Phase="";
		//public string TC_Company="";

		public string TcDate => TCard.TimeCardDate();

		public string WorkTime
		{
			get
			{
				return _workTime;
			}

			set
			{
				_workTime = value;
			}
		}

		public List<string> PhaseNames
		{
			get { return _phaseNames; }
			set { _phaseNames = value; }
		}

		public List<string> ProjectNames
		{
			get { return _projectNames; }
			set { _projectNames = value; }
		}

		public List<string> CompanyNames
		{
			get { return _companyNames; }
			set { _companyNames = value; }
		}

		List<string> _phaseNames = new List<string>();
		List<string> _projectNames = new List<string>();
		List<string> _companyNames = new List<string>();

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
				if ( !string.IsNullOrEmpty(phaseName) && !_phaseNames.Contains(phaseName))
				{
					_phaseNames.Add(phaseName);
				}
				projectToCheck = _projects.Find(p => p.ProjectId == _timeBlock.ProjectId);
				if ( projectToCheck != null && !_projectNames.Contains(projectToCheck.Name))
				{
					_projectNames.Add(projectToCheck.Name);
					indx = _companies.FindIndex(c => c.CompanyId == projectToCheck.CompnayId);
					companyName = (indx >-1) ? _companies[indx].CompanyName : "";
					//TcDate = (string.IsNullOrEmpty(TC_Date)) ? projectToCheck.StartDate.ToShortDateString():TcDate;
					if (!string.IsNullOrEmpty(companyName) && !CompanyNames.Contains(companyName))
					{
						CompanyNames.Add(companyName);
					}
				}

			}
			WorkTime = $"{WorkSpan.Hours}.{WorkSpan.Minutes/60}";
			BreakTime = BreakSpan.ToString();
			//Project project = _projects.Find(p => p.ProjectId == _timeCard.ProjectId);
			//if (project != null)
			//{
			//	TC_Phase =_phases == null ? "" : _phases.Find(p => p.PhaseId == _timeCard.PhaseId).PhaseName;
			//	TC_Company = _companies == null ? "" : _companies.Find(c => c.CompanyId == project.CompnayId).CompanyName;
			//	TcDate = project.StartDate.ToShortDateString();
			//}
		}

	}
}
