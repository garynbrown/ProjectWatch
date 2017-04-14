using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Data.DataSets;
using ProjectWatch.Support;
using ProjectWatch.Views;
using ProjectWatch.Entities;
using Timer = System.Timers.Timer;


namespace ProjectWatch.ViewModel
{
	public enum State
	{
		Stopped = 0,
		Running = 1,
		Paused = 3
	}
	[Export(typeof(DashboardViewModel))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class DashboardViewModel : ViewModelCommon
	{
		#region Fields
		TimeSpan _SumOfWorkComplete = TimeSpan.Zero;

		public DateTime CurrentTime => DateTime.Now;
		public TimeSpan TotalWorkTime = TimeSpan.Zero;
		public TimeSpan TotalBreakTime = TimeSpan.Zero;
		public TimeSpan tempBreakTime = TimeSpan.Zero;
		private Project _selectedProject;
		private Phase _selectedPhase;
		private bool _isPropertiesShowing;
		private TimeCard _currentTimeCard;
		private string _hoursWorked = "0:0";
		State _timerState = State.Stopped;
		private string _typeOfTime = "Not Started";
		private string _startButtonContent = "Start";
		private List<Phase> _allPhases;
		private List<Phase> _phases = new List<Phase>();
		private List<Project> _projects = new List<Project>();
		private Timer _timer;
		private DateTime workStartTime;
		private DateTime workStopTime;
		private DateTime breakStartTime;
		private DateTime breakStopTime;
		public IProjectRepository projectRepository;
		public IPhaseRepository phaseRepository;
		public ITimeCardRepository timeCardRepository;
		public ICompanyRepository companyRepository;
		public IContactRepository contactRepository;
		public IBillingRepository billingRepository;
		public ProjectSet DashboardProjectSet = new ProjectSet();
		public PhaseSet DashboardPhaseSet = new PhaseSet();
		public Project currentProject = new Project(-1);
		public Phase currentPhase = new Phase(-1);
		public CompanySet DashboardCompanySet = new CompanySet();
		public ContactSet DashboardContactSet = new ContactSet();
		public BillingSet DashboardBillingSet = new BillingSet();
		private TimecardViewModel CurrentTimeCardViewModel;
		private PreferenceSettings _pSettings;
		#endregion


		#region Constructors
		public DashboardViewModel()
		{
			if (IsInDesignModeStatic)
			{
				StartButtonContent = "Start";
			}
			projectRepository = ClientEntityBase.Container.GetExportedValue<IProjectRepository>();
			phaseRepository = ClientEntityBase.Container.GetExportedValue<IPhaseRepository>();
			timeCardRepository = ClientEntityBase.Container.GetExportedValue<ITimeCardRepository>();
			companyRepository = ClientEntityBase.Container.GetExportedValue<ICompanyRepository>();
			contactRepository = ClientEntityBase.Container.GetExportedValue<IContactRepository>();
			billingRepository = ClientEntityBase.Container.GetExportedValue<IBillingRepository>();
			_timer = new Timer(200);
			_timer.Elapsed += new ElapsedEventHandler(TimerUpdate);
			_timer.Enabled = true;
			ShowPropertiesCommand = new RelayCommand(OnShowProperties, CanShowProperties);
			StartCommand = new RelayCommand(OnStart, OnStartCanExecute);
			StopCommand = new RelayCommand(OnStop, OnStopCanExecute);
			PauseCommand = new RelayCommand(OnBreak, OnBreakCanExecute);
			ShowHelpCommand = new RelayCommand(OnShowHelp);
		}
		#endregion

		#region Properties

		public List<Phase> AllPhases
		{
			get { return _allPhases; }
			set
			{
				if (Set(() => AllPhases, ref _allPhases, value, false))
				{
					Phases = _allPhases.FindAll(p => p.ProjectId == currentProject.ProjectId);
				}
			}
		}

		public List<Billing> Billings { get; set; }

		public List<Company> Companies { get; set; }

		public List<Contact> Contacts { get; set; }
		public TimeCard CurrentTimeCard
		{
			get { return _currentTimeCard ?? (_currentTimeCard = new TimeCard(DateTime.Now)); }
			set { _currentTimeCard = value; }
		}

		public string HoursWorked
		{
			get { return _hoursWorked; }
			set { Set(() => HoursWorked, ref _hoursWorked, value, false); }
		}

		public bool IsPropertiesShowing
		{
			get { return _isPropertiesShowing; }
			set
			{
				if (Set(() => IsPropertiesShowing, ref _isPropertiesShowing, value, false))
				{
					ShowPropertiesCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public List<Phase> Phases
		{
			get { return _phases; }
			set
			{
				Set(() => Phases, ref _phases, value, false);
			}
		}

		public List<Project> Projects
		{
			get { return _projects; }
			set { Set(() => Projects, ref _projects, value, false); }
		}

		public Phase SelectedPhase
		{
			get { return _selectedPhase; }
			set
			{
				bool firstPhase = (_selectedPhase == null || currentPhase.PhaseId == -1);
				if (Set(() => SelectedPhase, ref _selectedPhase, value, false))
				{
					if (_timerState != State.Stopped && !firstPhase && _selectedPhase != null)
					{
						workStopTime = CurrentTime;
						CurrentTimeCard.AddWorkBlock(TimeBlock.CreateWorkBlock(workStartTime, workStopTime, currentProject.ProjectId, currentPhase.PhaseId));
						_SumOfWorkComplete = CalculateSumOfWorkBlocks();
						workStartTime = CurrentTime;
					}
					currentPhase = _selectedPhase;
					_pSettings.LastPhase = _selectedPhase.PhaseId;
				}
			}
		}

		public Project SelectedProject
		{
			get { return _selectedProject; }
			set
			{
				bool firstProject = (_selectedProject == null || currentProject.ProjectId == -1);
				if (Set(() => SelectedProject, ref _selectedProject, value, false))
				{
					if (_timerState != State.Stopped && !firstProject)
					{
						workStopTime = CurrentTime;
						CurrentTimeCard.AddWorkBlock(TimeBlock.CreateWorkBlock(workStartTime, workStopTime, currentProject.ProjectId, currentPhase.PhaseId));
						_SumOfWorkComplete = CalculateSumOfWorkBlocks();
						workStartTime = CurrentTime;
					}
					currentProject = _selectedProject;
					_pSettings.LastProject = _selectedProject.ProjectId;
					Phases = _allPhases.FindAll(p => p.ProjectId == currentProject.ProjectId);
					SelectedPhase = null;
					currentPhase = new Phase(-1);
				}
			}
		}

		public string StartButtonContent
		{
			get { return _startButtonContent; }
			set { Set(() => StartButtonContent, ref _startButtonContent, value, false); }
		}

		public State TimerState
		{
			get { return _timerState; }
			set
			{
				Set(() => TimerState, ref _timerState, value, false);
			}
		}

		public string TypeOfTime
		{
			get { return _typeOfTime; }
			set
			{
				Set(() => TypeOfTime, ref _typeOfTime, value, false);
			}
		}
		#endregion

		#region Methods

		TimeSpan CalculateSumOfWorkBlocks()
		{
			TimeSpan calculatedTimeSpan = TimeSpan.Zero;
			foreach (TimeBlock _timeBlock in CurrentTimeCard.TimeBlocks)
			{
				if (_timeBlock.TimeBlockType == TimeType.Break)
					continue;
				calculatedTimeSpan += _timeBlock.GetTimeSpan();
			}
			return calculatedTimeSpan;
		}
		bool CanShowProperties()
		{
			return !IsPropertiesShowing;
		}

		private void OnBreak()
		{
			if (_timerState != State.Running) return;
			TimerState = State.Paused;
			workStopTime = CurrentTime;
			CurrentTimeCard.AddWorkBlock(TimeBlock.CreateWorkBlock(workStartTime, workStopTime, currentProject.ProjectId, currentPhase.PhaseId));
			_SumOfWorkComplete = CalculateSumOfWorkBlocks();
			breakStartTime = CurrentTime;
			StartButtonContent = "Start";
			TypeOfTime = "On Break";
			StopCommand.RaiseCanExecuteChanged();
			StartCommand.RaiseCanExecuteChanged();
			PauseCommand.RaiseCanExecuteChanged();
		}

		void OnShowHelp()
		{
			AboutView about = new AboutView();
			about.Show();
		}
		private bool OnBreakCanExecute()
		{
			return _timerState == State.Running;
		}
		private void OnShowProperties()
		{
			PropertyWindow TabbedProperties = new PropertyWindow();
			IsPropertiesShowing = true;
			ShowPropertiesCommand.RaiseCanExecuteChanged();
			TabbedProperties.Show();
		}

		void OnStart()
		{
			if (_timerState == State.Paused)
			{
				breakStopTime = CurrentTime;
				TotalBreakTime += tempBreakTime;
				tempBreakTime = TimeSpan.Zero;

				CurrentTimeCard?.AddBreakBlock(TimeBlock.CreateBreakBlock(breakStartTime, breakStopTime));
			}
			else
			{
				if (CurrentTimeCard == null)
				{
					CurrentTimeCard = new TimeCard();
				}
			}
			workStartTime = CurrentTime;
			TimerState = State.Running;
			if (currentProject?.StartDate == DateTime.MinValue)
				currentProject.StartDate = DateTime.Today;
			StartButtonContent = "Working";
			TypeOfTime = "Time at Work";
			StopCommand.RaiseCanExecuteChanged();
			PauseCommand.RaiseCanExecuteChanged();
			StartCommand.RaiseCanExecuteChanged();
		}

		bool OnStartCanExecute()
		{
			return _timerState != State.Running;
		}

		void OnStop()
		{
			if (_timerState == State.Paused)
			{
				breakStopTime = DateTime.Now;
				CurrentTimeCard.AddBreakBlock(TimeBlock.CreateBreakBlock(breakStartTime, breakStopTime));
			}
			else
			{
				ValidateModel();
				if (IsValid)
				{
					workStopTime = CurrentTime;
					CurrentTimeCard.AddWorkBlock(TimeBlock.CreateWorkBlock(workStartTime, workStopTime, currentProject.ProjectId,
						currentPhase.PhaseId));
					_SumOfWorkComplete = CalculateSumOfWorkBlocks();
				}
				else
				{
					TimeCardCreated?.Invoke(this, new TimeCardEventArgs(CurrentTimeCard, true, true));
					workStopTime = CurrentTime;
					CurrentTimeCard.AddWorkBlock(TimeBlock.CreateWorkBlock(workStartTime, workStopTime, currentProject.ProjectId,
						currentPhase.PhaseId));
					_SumOfWorkComplete = CalculateSumOfWorkBlocks();
				}
			}
			timeCardRepository.Update(CurrentTimeCard);
			PreferenceManager.saveConfiguration(_pSettings);

			TimerState = State.Stopped;
			StartButtonContent = "Start";
			TypeOfTime = "Not Working";
			TotalBreakTime = TimeSpan.Zero;
			StartCommand.RaiseCanExecuteChanged();
			PauseCommand.RaiseCanExecuteChanged();
			StopCommand.RaiseCanExecuteChanged();
		}

		bool OnStopCanExecute()
		{
			return _timerState != State.Stopped;
		}

		void TimerUpdate(object Sender, ElapsedEventArgs e)
		{
			TimeSpan totalTime;
			RaisePropertyChanged(() => CurrentTime);
			if (TimerState == State.Running)
			{
				TotalWorkTime = CurrentTime - workStartTime + _SumOfWorkComplete;
				totalTime = TotalWorkTime;
			}
			else if (TimerState == State.Paused)
			{
				tempBreakTime = CurrentTime - breakStartTime;
				totalTime = tempBreakTime;
			}
			else
			{
				totalTime = TimeSpan.Zero;
			}
			HoursWorked = $"{totalTime.Hours:D2}:{totalTime.Minutes:D2}:{totalTime.Seconds:D2}";
		}
#endregion
#region Commands

		public RelayCommand PauseCommand { get; set; }
		public RelayCommand ShowPropertiesCommand { get; set; }
		public RelayCommand ShowHelpCommand { get; set; }

		public RelayCommand StartCommand { get; private set; }

		public RelayCommand StopCommand { get; private set; }
#endregion

#region Overrides

		protected override void OnViewLoaded()
		{
			CurrentTimeCard = timeCardRepository.GetOrCreateTodaysTimeCard();
			DashboardProjectSet.EntitySet = projectRepository.Get().EntitySet;
			DashboardPhaseSet.EntitySet = phaseRepository.Get().EntitySet;
			DashboardCompanySet.EntitySet = companyRepository.Get().EntitySet;
			DashboardContactSet.EntitySet = contactRepository.Get().EntitySet;
			DashboardBillingSet.EntitySet = billingRepository.Get().EntitySet;
			_projects = DashboardProjectSet.EntitySet.ToList();
			AllPhases = DashboardPhaseSet.EntitySet.ToList();
			Companies = DashboardCompanySet.EntitySet.ToList();
			Contacts = DashboardContactSet.EntitySet.ToList();
			Billings = DashboardBillingSet.EntitySet.ToList();
			PreferenceManager.LoadConfiguration(ref _pSettings);
			if (_pSettings.LastProject > -1)
			{
				SelectedProject = _projects.Find(p => p.ProjectId == _pSettings.LastProject);
				if (_pSettings.LastPhase > -1)
				{
					SelectedPhase = _phases.Find(p => p.PhaseId == _pSettings.LastPhase);
				}
			}
			_SumOfWorkComplete = CalculateSumOfWorkBlocks();
		}
		public override string ViewTitle
		{
			get { return $"Dashboard"; }
		}
#endregion

#region Events_Delegates
		public event EventHandler<TimeCardEventArgs> TimeCardCreated;
#endregion

	}
}