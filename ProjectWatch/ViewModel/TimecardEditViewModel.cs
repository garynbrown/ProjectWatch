using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Data.DTO;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	public class TimecardEditViewModel : ViewModelCommon
	{
		private TimeCardMainViewModel timecardMain = null;
		private TimeCard _timecardUnderEdit = new TimeCard();
		private bool _startAm;
		private bool _startPm;
		private bool _endAm;
		private bool _endPm;
		private string _endTime;
		private string _startTime;
		private List<Company> _companies;
		private List<Project> _projects;
		private List<Phase> _phases;
		private Company _selectedCompany;
		private Project _selectedProject;
		private Phase _selectedPhase;
		private bool _workType;
		private bool _breakType;
		private List<TimeBlock> _timeBlocks;
		private TimeBlockDTO _selectedTimeBlock;
		private string _timeCardDate;
		private ObservableCollection<TimeBlockDTO> _displayTimeBlocks;

		public TimecardEditViewModel(TimeCard timecard )
		{
			_timecardUnderEdit = timecard.Clone() as TimeCard;
			
			SaveCommand = new RelayCommand(OnSave);

		}

		public TimecardEditViewModel() : this(TimeCard.CreateTimeCard(-1))
		{
			
		}
		public RelayCommand SaveCommand { get; set; }

		public void OnSave()
		{
			if (_timecardUnderEdit.TimeId == -1)
			{
				//saveCommand
			}
			else
			{
				// update
			}
			// update time card list

		}

		public string TimeCardDate
		{
			get { return _timeCardDate; }
			set
			{
				if (Set(() => TimeCardDate, ref _timeCardDate, value, false))
				{
					DateTime newDate;
					if (!DateTime.TryParse(_timeCardDate, out newDate))
					{
						TimeCardDate = "";
					}
					else
					{
						//todo if old Id is not -1 then delete the old file
						_timecardUnderEdit.TimeId = TimeCard.MakeTimeCardIdFromDate(newDate);
					}
				}
			}
		}

		public List<TimeBlock> TimeBlocks
		{
			get { return _timeBlocks; }
			set
			{
				Set(() => TimeBlocks, ref _timeBlocks, value, false);
			}
		}

		public TimeBlockDTO SelectedTimeBlock
		{
			get { return _selectedTimeBlock; }
			set { _selectedTimeBlock = value; }
		}

		public List<Company> Companies
		{
			get { return _companies; }
			set { _companies = value; }
		}

		public Company SelectedCompany
		{
			get { return _selectedCompany; }
			set { _selectedCompany = value; }
		}

		public List<Project> Projects
		{
			get { return _projects; }
			set { _projects = value; }
		}

		public Project SelectedProject
		{
			get { return _selectedProject; }
			set { _selectedProject = value; }
		}

		public List<Phase> Phases
		{
			get { return _phases; }
			set { _phases = value; }
		}

		public Phase SelectedPhase
		{
			get { return _selectedPhase; }
			set { _selectedPhase = value; }
		}

		public string StartTime
		{
			get { return _startTime; }
			set { _startTime = value; }
		}

		public string EndTime
		{
			get { return _endTime; }
			set { _endTime = value; }
		}

		public bool StartAM
		{
			get { return _startAm; }
			set
			{
				Set(() => StartAM, ref _startAm, value, false);
			}
		}

		public bool WorkType
		{
			get { return _workType; }
			set { _workType = value; }
		}

		public bool BreakType
		{
			get { return _breakType; }
			set { _breakType = value; }
		}

		public bool StartPM
		{
			get { return _startPm; }
			set
			{
				Set(() => StartPM, ref _startPm, value, false);
			}
		}

		public bool EndAM
		{
			get { return _endAm; }
			set
			{
				Set(() => EndAM, ref _endAm, value, false);
			}
		}

		public bool EndPM
		{
			get { return _endPm; }
			set
			{
				Set(() => EndPM, ref _endPm, value, false);
			}
		}

		private void MakeDisplayDtos(List<TimeBlock> timeBlocks)
		{
			List<TimeBlockDTO> DTOs = new List<TimeBlockDTO>();
			foreach (TimeBlock _timeBlock in timeBlocks)
			{
				DTOs.Add(new TimeBlockDTO(_timeBlock, timecardMain.TimeCardMainProjects.ToList(),
					timecardMain.TimeCardMainPhases.ToList()));
			}
			DisplayTimeBlocks = new ObservableCollection<TimeBlockDTO>(DTOs);
		}

		public ObservableCollection<TimeBlockDTO> DisplayTimeBlocks
		{
			get { return _displayTimeBlocks; }
			set { Set(() => DisplayTimeBlocks, ref _displayTimeBlocks, value, false);}
		}

		protected override void OnViewLoaded()
		{
			timecardMain = ClientEntityBase.Container.GetExportedValue<TimeCardMainViewModel>();
			_companies = timecardMain.companyRepository.Get().EntitySet.ToList();
			_phases = timecardMain.phaseRepository.Get().EntitySet.ToList();
			_projects = timecardMain.projectRepository.Get().EntitySet.ToList();
			StartAM = true;
			EndPM = true;
			WorkType = true;
			if (_timecardUnderEdit.TimeId != -1)
			{
				TimeCardDate = _timecardUnderEdit.TimeCardDate();
				
				MakeDisplayDtos(_timecardUnderEdit.WorkBlocks);
			}
		}
	}
}
