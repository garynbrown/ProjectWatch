using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
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
		private DashboardViewModel dashboardViewModel = null;
		private TimeCard _timecardUnderEdit = new TimeCard();
		TimeBlock _timeBlockUnderEdit = null;
		private string _endTime;
		private string _startTime;
		private List<Company> _allCompanies;
		private List<Company> _companies;
		private List<Project> _allProjects;
		private List<Project> _projects;
		private List<Phase> _allPhases;
		private List<Phase> _phases;
		private Company _selectedCompany;
		private Project _selectedProject;
		private Phase _selectedPhase;
		private bool _workType ;
		private bool _breakType;
		private List<TimeBlock> _timeBlocks;
		private TimeBlockDTO _selectedTimeBlock;
		private string _timeCardDate;
		private bool inNewBlockMode = false;
		private ObservableCollection<TimeBlockDTO> _displayTimeBlocks;
		private bool isTimecardActive = false;
		private string _totalBreakTime;
		private string _totalTaskTime;
		//private bool _dirtyTimeBlock;

		public TimecardEditViewModel(TimeCard timecard)
		{
			_timecardUnderEdit = timecard.Clone() as TimeCard;
			_timecardUnderEdit.CleanAll();
			if (_timecardUnderEdit.TimeId != -1)
			{
				_timeCardDate = _timecardUnderEdit.TimeCardDate();
			}
			_timeBlockUnderEdit = new TimeBlock();
			_timeBlockUnderEdit.CleanAll();
			
			SaveCommand = new RelayCommand(OnSave, CanSave);
			DeleteTimeBlockCommand = new RelayCommand<TimeBlockDTO>(OnDeleteTimeBlock);
			UpdateTimeBlockCommand = new RelayCommand(OnUpdateTimeBlockCommand, canUpdate);
			CreateTimeBlockCommand = new RelayCommand(OnCreateTimeBlock, CanCreateTimeBlock);
			ClearFieldsCommand = new RelayCommand(OnClearFields);

		}

		public TimecardEditViewModel() : this(TimeCard.CreateTimeCard(-1))
		{

		}

		public RelayCommand<TimeBlockDTO> DeleteTimeBlockCommand { get; set; }

		void OnDeleteTimeBlock(TimeBlockDTO timeBlockDto)
		{
			_timecardUnderEdit.TimeBlocks.Remove(timeBlockDto.TBlock);
			MakeDisplayDtos(_timecardUnderEdit.TimeBlocks);
			_timecardUnderEdit.MakeDirty();
			SaveCommand.RaiseCanExecuteChanged();

		}
		public RelayCommand SaveCommand { get; set; }

		public void OnSave()
		{
			//todo if old Id is not -1 then delete the old file
			// todo if this is curretly active in dashbord, then stop first
			if (_timecardUnderEdit.TimeId == -1)
			{
				dashboardViewModel.timeCardRepository.Add(_timecardUnderEdit);
			}
			else
			{
				dashboardViewModel.timeCardRepository.Update(_timecardUnderEdit);
			}
			// update time card list
			_timecardUnderEdit.CleanAll();
			inNewBlockMode = false;
			SaveCommand.RaiseCanExecuteChanged();

		}

		bool CanSave()
		{
			// todo check if dashboard is running, can't edit current time card
			return _timecardUnderEdit.IsDirty && !isTimecardActive;
		}

		public RelayCommand UpdateTimeBlockCommand { get; set; }

		public string SaveButtonContent
		{
			get
			{
				if (dashboardViewModel.TimerState != State.Stopped  
					&& (_timecardUnderEdit == null || dashboardViewModel.CurrentTimeCard.TimeId == _timecardUnderEdit.TimeId))
				{
					isTimecardActive = true;
					return "Time Card Active";
				}
				else
				{
					isTimecardActive = false;
					return "Save TimeCard";
				}
				
			}
		}

		void OnUpdateTimeBlockCommand()
		{
			if (!_timeBlockUnderEdit.IsDirty)
				return;
			_timeBlockUnderEdit.TimeBlockType = (WorkType) ? TimeType.Task : TimeType.Break;
			_timecardUnderEdit.TimeBlocks.Remove(_selectedTimeBlock.TBlock);
			_timecardUnderEdit.TimeBlocks.Add(_timeBlockUnderEdit);
			MakeDisplayDtos(_timecardUnderEdit.TimeBlocks);
			_timeBlockUnderEdit.CleanAll();
			_timecardUnderEdit.MakeDirty();
			//inNewBlockMode = false;
			//CreateTimeBlockCommand.RaiseCanExecuteChanged();
			//UpdateTimeBlockCommand.RaiseCanExecuteChanged();
			SaveCommand.RaiseCanExecuteChanged();
			OnClearFields();
		}
		bool canUpdate()
		{
			return (_timeBlockUnderEdit != null && _timeBlockUnderEdit.IsDirty 
				&& !inNewBlockMode && !string.IsNullOrEmpty(_startTime) && !string.IsNullOrEmpty(_endTime));
		}

		public RelayCommand CreateTimeBlockCommand { get; set; }

		void OnCreateTimeBlock()
		{
			if (!_timeBlockUnderEdit.IsDirty)
				return;
			_timeBlockUnderEdit.TimeBlockType = (WorkType) ? TimeType.Task : TimeType.Break;
			_timecardUnderEdit.TimeBlocks.Add(_timeBlockUnderEdit);
			MakeDisplayDtos(_timecardUnderEdit.TimeBlocks);
			_timeBlockUnderEdit.CleanAll();
			_timecardUnderEdit.MakeDirty();
			//inNewBlockMode = false;
			//CreateTimeBlockCommand.RaiseCanExecuteChanged();
			//UpdateTimeBlockCommand.RaiseCanExecuteChanged();
			SaveCommand.RaiseCanExecuteChanged();
			OnClearFields();
		}

		bool CanCreateTimeBlock()
		{
			return inNewBlockMode && _timeBlockUnderEdit != null && _timeBlockUnderEdit.IsDirty 
				&& !string.IsNullOrEmpty(_startTime) && !string.IsNullOrEmpty(_endTime);
		}

		public RelayCommand ClearFieldsCommand { get; set; }

		void OnClearFields()
		{
			_timeBlockUnderEdit = new TimeBlock();
			WorkType = true;
			StartTime = "";
			EndTime = "";
			SelectedCompany = null;
			SelectedProject = null;
			SelectedPhase = null;
			SelectedTimeBlock = null;

			_timeBlockUnderEdit.CleanAll();
			inNewBlockMode = true;
			CreateTimeBlockCommand.RaiseCanExecuteChanged();
			UpdateTimeBlockCommand.RaiseCanExecuteChanged();
		}

		//public bool DirtyTimeBlock
		//{
		//	get { return _dirtyTimeBlock; }
		//	set
		//	{
		//		if (Set(() => DirtyTimeBlock, ref _dirtyTimeBlock, value, false))
		//		{
		//			UpdateTimeBlockCommand.RaiseCanExecuteChanged();
		//			CreateTimeBlockCommand.RaiseCanExecuteChanged();
		//		}
		//	}
		//}

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
						_timecardUnderEdit.TimeId = TimeCard.MakeTimeCardIdFromDate(newDate);
					}
					_timecardUnderEdit.MakeDirty();
					SaveCommand.RaiseCanExecuteChanged();
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
			set
			{
				if (Set(() => SelectedTimeBlock, ref _selectedTimeBlock, value, false))
				{
					if (_selectedTimeBlock != null)
					{
						_timeBlockUnderEdit = _selectedTimeBlock.TBlock.Clone() as TimeBlock;
						StartTime = _timeBlockUnderEdit.StartTime.ToString("T");
						EndTime = _timeBlockUnderEdit.EndTime.ToString("T");
						if (_timeBlockUnderEdit.TimeBlockType == TimeType.Task)
						{
							WorkType = true;
							int projectId = _selectedTimeBlock.TBlock.ProjectId;
							SelectedProject = (projectId > -1) ? _projects.Find(p => p.ProjectId == projectId) : null;
							if (_selectedProject != null)
							{
								Phases = new List<Phase>(_allPhases.FindAll(p => p.ProjectId == _selectedProject.ProjectId));
								int phaseId = _selectedTimeBlock.TBlock.PhaseId;
								SelectedPhase = (phaseId > -1) ? _phases.Find(p => p.PhaseId == phaseId) : null;
							}
							else
							{
								Phases = new List<Phase>();
								SelectedPhase = null;
								SelectedCompany = null;
							}
						}
						else
						{
							BreakType = true;
							SelectedProject = null;
							SelectedPhase = null;
							SelectedCompany = null;
						}
						_timeBlockUnderEdit.CleanAll();
					}

				}
				inNewBlockMode = _selectedTimeBlock == null;
				UpdateTimeBlockCommand.RaiseCanExecuteChanged();
				CreateTimeBlockCommand.RaiseCanExecuteChanged();
				RaisePropertyChanged(() => CalculatedHours);
			}
		}

		public List<Company> Companies
		{
			get
			{
				_companies = _companies ?? new List<Company>(_allCompanies);
				return _companies;
			}
			set { Set(() => Companies, ref _companies, value, false); }
		}

		public Company SelectedCompany
		{
			get { return _selectedCompany; }
			set
			{
				if (Set(() => SelectedCompany, ref _selectedCompany, value, false))
				{
					if (_selectedCompany != null)
					{
						Projects = _allProjects.FindAll(p => p.CompnayId == _selectedCompany.CompanyId);
						if (_selectedProject?.CompnayId != SelectedCompany?.CompanyId)
						{
							SelectedProject = null;
							SelectedPhase = null;
							_timeBlockUnderEdit.ProjectId = -1;
							_timeBlockUnderEdit.PhaseId = -1;						
						}
					}
					else
					{
						Projects = new List<Project>(_allProjects);
					}
					_timeBlockUnderEdit.MakeDirty();
					CreateTimeBlockCommand.RaiseCanExecuteChanged();
					UpdateTimeBlockCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public List<Project> Projects
		{
			get
			{
				_projects = _projects ?? new List<Project>(_allProjects);
				return _projects;
			}
			set { Set(() => Projects, ref _projects, value, false); }
		}

		public Project SelectedProject
		{
			get { return _selectedProject; }
			set
			{
				if (Set(() => SelectedProject, ref _selectedProject, value, false) )
				{
					_timeBlockUnderEdit.ProjectId = _selectedProject == null ? -1 : _selectedProject.ProjectId;
					if (_selectedProject != null)
					{
						if ((_selectedCompany == null) || (_selectedCompany != null && _selectedCompany.CompanyId != _selectedProject.CompnayId))
						{
							if (_selectedProject.CompnayId > -1)
							{
								SelectedCompany = _allCompanies.Find(c => c.CompanyId == _selectedProject.CompnayId);
							}
							else
							{
								SelectedCompany = null;
							}
						}
					}
					_timeBlockUnderEdit.MakeDirty();
					CreateTimeBlockCommand.RaiseCanExecuteChanged();
					UpdateTimeBlockCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public List<Phase> Phases
		{
			get
			{
				_phases = _phases ?? new List<Phase>(_allPhases);
				return _phases;
			}
			set { Set(() => Phases, ref _phases, value, false); }
		}

		public Phase SelectedPhase
		{
			get { return _selectedPhase; }
			set
			{
				if (Set(() => SelectedPhase, ref _selectedPhase, value, false))
				{
					_timeBlockUnderEdit.PhaseId  = _selectedPhase == null ? -1:_selectedPhase.PhaseId;
					_timeBlockUnderEdit.MakeDirty();
					CreateTimeBlockCommand.RaiseCanExecuteChanged();
					UpdateTimeBlockCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string StartTime
		{
			get
			{ return _startTime; }
			set
			{
				if (Set(() => StartTime, ref _startTime, value, false))
				{
					DateTime tempTime = DateTime.MinValue;
					DateTime tempDate = DateTime.MinValue;
					if (DateTime.TryParse(_startTime, out tempTime))
					{
						if (DateTime.TryParse(_timecardUnderEdit.TimeCardDate(), out tempDate))
						{
							_timeBlockUnderEdit.StartTime = tempDate.Add(tempTime.TimeOfDay);

						}
						else
						{
							StartTime = "";
						}
						
					}
					else
					{
						StartTime = "";
					}
					_timeBlockUnderEdit.MakeDirty();
					CreateTimeBlockCommand.RaiseCanExecuteChanged();
					UpdateTimeBlockCommand.RaiseCanExecuteChanged();
					RaisePropertyChanged(() => CalculatedHours);
				}
			}
		}

		public string EndTime
		{
			get { return _endTime; }
			set
			{
				DateTime tempTime = DateTime.MinValue;
				DateTime tempDate = DateTime.MinValue;
				if (Set(() => EndTime, ref _endTime, value, false))
				{
					if (DateTime.TryParse(_endTime, out tempTime))
					{
						if (DateTime.TryParse(_timecardUnderEdit.TimeCardDate(), out tempDate))
						{
							_timeBlockUnderEdit.EndTime = tempDate.Add(tempTime.TimeOfDay);

						}
						else
						{
							EndTime = "";
						}
					}
					else
					{
						EndTime = "";
					}
				_timeBlockUnderEdit.MakeDirty();
					CreateTimeBlockCommand.RaiseCanExecuteChanged();
					UpdateTimeBlockCommand.RaiseCanExecuteChanged();
					RaisePropertyChanged(() => CalculatedHours);
				}
			}
		}

		public string CalculatedHours
		{
			get
			{
				if (SelectedTimeBlock == null)
					return "";
				TimeSpan ts = _timeBlockUnderEdit.EndTime - _timeBlockUnderEdit.StartTime;
				if (ts.Hours <= 0 && ts.Minutes <= 0)
					return "0";
				return $"{ts.Hours:D}:{ts.Minutes:d2}";
			}
		}


		public bool WorkType
		{
			get { return _workType; }
			set
			{
				if (Set(() => WorkType, ref _workType, value, false))
				{
					_timeBlockUnderEdit.MakeDirty();
					CreateTimeBlockCommand.RaiseCanExecuteChanged();
					UpdateTimeBlockCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public bool BreakType
		{
			get { return _breakType; }
			set
			{
				if (Set(() => BreakType, ref _breakType, value, false))
				{
					_timeBlockUnderEdit.ProjectId = -1;
					if (_selectedProject != null)
						SelectedProject = null;
					_timeBlockUnderEdit.PhaseId = -1;
					if (_selectedPhase != null)
						SelectedPhase = null;
					if (_selectedCompany != null)
						SelectedCompany = null;
					_timeBlockUnderEdit.MakeDirty();
					CreateTimeBlockCommand.RaiseCanExecuteChanged();
					UpdateTimeBlockCommand.RaiseCanExecuteChanged();
				}
			}
		}

		private void MakeDisplayDtos(List<TimeBlock> timeBlocks)
		{
			List<TimeBlockDTO> DTOs = new List<TimeBlockDTO>();
			List<TimeBlock> sortedList =  timeBlocks.OrderBy(t => t.StartTime).ToList();
			foreach (TimeBlock _timeBlock in sortedList)
			{
				DTOs.Add(new TimeBlockDTO(_timeBlock, timecardMain.TimeCardMainProjects.ToList(),
					timecardMain.TimeCardMainPhases.ToList()));
			}
			DisplayTimeBlocks = new ObservableCollection<TimeBlockDTO>(DTOs);
		}

		public ObservableCollection<TimeBlockDTO> DisplayTimeBlocks
		{
			get { return _displayTimeBlocks; }
			set
			{
				if (Set(() => DisplayTimeBlocks, ref _displayTimeBlocks, value, false))
				{
					TimeSpan breaktime = new TimeSpan();
					TimeSpan tasktime = new TimeSpan();
					foreach (TimeBlockDTO _displayTimeBlock in _displayTimeBlocks)
					{
						if (_displayTimeBlock.TBlock.TimeBlockType == TimeType.Break)
						{
							breaktime = breaktime.Add(_displayTimeBlock.TBlock.EndTime - _displayTimeBlock.TBlock.StartTime);
						}
						else
						{
							tasktime = tasktime.Add(_displayTimeBlock.TBlock.EndTime - _displayTimeBlock.TBlock.StartTime);
						}
					}
					TotalBreakTime = $"{breaktime.Hours:D0}:{breaktime.Minutes:D0}";
					TotalTaskTime = $"{tasktime.Hours:D0}:{tasktime.Minutes:D0}";
				}
			}
		}

		public string TotalBreakTime
		{
			get { return _totalBreakTime; }
			set { Set(() => TotalBreakTime, ref _totalBreakTime, value, false); }
		}

		public string TotalTaskTime
		{
			get { return _totalTaskTime; }
			set {Set(() => TotalTaskTime, ref _totalTaskTime, value, false); }
		}

		protected override void OnViewLoaded()
		{
			timecardMain = ClientEntityBase.Container.GetExportedValue<TimeCardMainViewModel>();
			dashboardViewModel = ClientEntityBase.Container.GetExportedValue<DashboardViewModel>();
			_allCompanies = timecardMain.companyRepository.Get().EntitySet.ToList() ?? new List<Company>();
			_allPhases = timecardMain.phaseRepository.Get().EntitySet.ToList() ?? new List<Phase>();
			_allProjects = timecardMain.projectRepository.Get().EntitySet.ToList() ?? new List<Project>();
			WorkType = true;
			if (_timecardUnderEdit.TimeId != -1)
			{
				//TimeCardDate = _timecardUnderEdit.TimeCardDate();

				MakeDisplayDtos(_timecardUnderEdit.TimeBlocks);
			}
		}
	}
}
