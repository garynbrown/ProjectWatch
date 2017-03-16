﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
//using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Data.DataRepositories;
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
		//private string _StopAnim = "InActive";
		//private string _PauseAnim = "InActive";
		//private string _StartAnim = "InActive";
		private string _typeOfTime = "Not Started";
		private string _startButtonContent = "Start";
		private ObservableCollection<Phase> _phases;
		private ObservableCollection<Project> _projects;
		private Timer _timer;
		private DateTime workStartTime;
		private DateTime workStopTime;
		private DateTime breakStartTime;
		private DateTime breakStopTime;
		//private Timer _breakTimer;
		//private Timer _workTimer;


		public string TypeOfTime
		{
			get { return _typeOfTime; }
			set
			{
				Set(() => TypeOfTime, ref _typeOfTime, value, false);
//				RaisePropertyChanged(() => TypeOfTime);
			}
		}

		//public string StartAnim
		//{
		//	get { return _StartAnim; }
		//	set
		//	{
		//		Set(() => StartAnim, ref _StartAnim, value, false);
		//	}
		//}

		//public string PauseAnim
		//{
		//	get { return _PauseAnim; }
		//	set
		//	{
		//		Set(() => PauseAnim, ref _PauseAnim, value, false);
		//	}
		//}

		//public string StopAnim
		//{
		//	get { return _StopAnim; }
		//	set
		//	{
		//		Set(() => StopAnim, ref _StopAnim, value, false);
		//	}
		//}
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
			//	_breakTimer = new Timer(500);
			//	_workTimer = new Timer(500);
			_timer = new Timer(200);
			_timer.Elapsed += new ElapsedEventHandler(TimerUpdate);
			//_breakTimer.Elapsed += new ElapsedEventHandler(BreakTimerUpdate);
			//_workTimer.Elapsed += new ElapsedEventHandler(WorkTimerUpdate);
			_timer.Enabled = true;
			//_breakTimer.Enabled = false;
			//_workTimer.Enabled = false;
			ShowPropertiesCommand = new RelayCommand(OnShowProperties);
//			StartCommand = new RelayCommand(OnStart, OnStartCanExecute);
			StartCommand = new RelayCommand(OnStart);
//			StopCommand = new RelayCommand(OnStop, OnStopCanExecute);
			StopCommand = new RelayCommand(OnStop);
//			PauseCommand = new RelayCommand(OnBreak, OnBreakCanExecute);
			PauseCommand = new RelayCommand(OnBreak);
		}

		private void OnShowProperties()
		{
			PropertyWindow TabbedProperties = new PropertyWindow();
			TabbedProperties.Show();
		}

		public override string ViewTitle
		{
			get { return $"Dashboard"; }
		}

		protected override void OnViewLoaded()
		{
			base.OnViewLoaded();
			//TimeCardSet timeCardSet = new TimeCardSet();
			//timeCardSet.EntitySet =	timeCardRepository.GetDefaultRangeTimeCards().ToList();
			CurrentTimeCard = timeCardRepository.GetOrCreateTodaysTimeCard();
			DashboardProjectSet.EntitySet =  projectRepository.Get().EntitySet ;
			DashboardPhaseSet.EntitySet = phaseRepository.Get().EntitySet;
			DashboardCompanySet.EntitySet = companyRepository.Get().EntitySet;
			DashboardContactSet.EntitySet = contactRepository.Get().EntitySet;
			DashboardBillingSet.EntitySet = billingRepository.Get().EntitySet;
			_projects = new ObservableCollection<Project>(DashboardProjectSet.EntitySet);
			_phases =  new ObservableCollection<Phase>(DashboardPhaseSet.EntitySet);
			_companies = new ObservableCollection<Company>(DashboardCompanySet.EntitySet);
			_contacts = new ObservableCollection<Contact>(DashboardContactSet.EntitySet);
			_billings = new ObservableCollection<Billing>(DashboardBillingSet.EntitySet);
		}

		// Fields...
		//		private RelayCommand _pauseCommand;
		//		private RelayCommand _startCommand;
		//		private RelayCommand _stopCommand;
		private TimeCard _currentTimeCard;
		private string _hoursWorked = "0:0";
		//private bool _isStarted = false;
		//private bool _isStoped = true;
		//private bool _isOnBreak = false;
		State _timerState = State.Stopped;

		public Project SelectedProject
		{
			get { return _selectedProject; }
			set
			{
				bool firstProject = (_selectedProject == null || currentProject.ProjectId == -1);
				if (Set(() => SelectedProject, ref _selectedProject, value, false))
				{
					bool resume = false;
					if (_timerState != State.Stopped && !firstProject)
					{
						workStopTime = CurrentTime;
						CurrentTimeCard.AddWorkBlock(TimeBlock.CreateWorkBlock(workStartTime, workStopTime, currentProject.ProjectId, currentPhase.PhaseId));
						_SumOfWorkComplete = CalculateSumOfWorkBlocks();
						workStartTime = CurrentTime;
						//resume = true;
						//_timer.Stop();
						//OnStop();
					}
					//currentProject = DashboardProjectSet.EntitySet.ToList().Find(p => p.Name == value);
					currentProject = _selectedProject;
					Phases = new ObservableCollection<Phase>( DashboardPhaseSet.EntitySet.ToList().FindAll(p => p.ProjectId == currentProject.ProjectId));
					SelectedPhase = null;
					currentPhase = new Phase(-1);
					//if (resume)
					//{
					//	OnStart();
					//	_timer.Start();
					//}
				}
			}
		}

		public State TimerState
		{
			get { return _timerState; }
			set
			{
				Set(() => TimerState, ref _timerState, value, false);
			}
		}
		//public bool IsStarted
		//{
		//	get { return _isStarted; }
		//	set
		//	{
		//		_isStarted = value;
		//		RaisePropertyChanged(() => IsStarted);
		//		//StopCommand.RaiseCanExecuteChanged();
		//		//StartCommand.RaiseCanExecuteChanged();
		//		//PauseCommand.RaiseCanExecuteChanged();
		//	}
		//}

		//public bool IsStoped
		//{
		//	get { return _isStoped; }
		//	set
		//	{
		//		_isStoped = value;
		//		//StopCommand.RaiseCanExecuteChanged();
		//		//StartCommand.RaiseCanExecuteChanged();
		//		//PauseCommand.RaiseCanExecuteChanged();
		//	}
		//}

		//public bool IsOnBreak
		//{
		//	get { return _isOnBreak; }
		//	set
		//	{
		//		_isOnBreak = value;
		//		//StopCommand.RaiseCanExecuteChanged();
		//		//StartCommand.RaiseCanExecuteChanged();
		//		//PauseCommand.RaiseCanExecuteChanged();
		//	}
		//}

		public ObservableCollection<Company> Companies
		{
			get { return _companies; }
			set { _companies = value; }
		}

		public ObservableCollection<Contact> Contacts
		{
			get { return _contacts; }
			set { _contacts = value; }
		}

		public ObservableCollection<Billing> Billings
		{
			get { return _billings; }
			set { _billings = value; }
		}

		public ObservableCollection<Project> Projects
		{
			get { return _projects; }
			set { _projects = value; }
		}


		public ObservableCollection<Phase> Phases
		{
			get { return _phases; }
			set
			{
				Set(() => Phases, ref _phases, value, false);
			}
		}

		public Phase SelectedPhase
		{
			get { return _selectedPhase; }
			set
			{
				bool firstPhase = (_selectedPhase == null || currentPhase.PhaseId == -1);
				if (Set(() => SelectedPhase, ref _selectedPhase, value, false))
				{
					//bool resume = false;
					if (_timerState != State.Stopped && !firstPhase && _selectedPhase != null)
					{
						workStopTime = CurrentTime;
						CurrentTimeCard.AddWorkBlock(TimeBlock.CreateWorkBlock(workStartTime, workStopTime, currentProject.ProjectId, currentPhase.PhaseId));
						_SumOfWorkComplete = CalculateSumOfWorkBlocks();
						workStartTime = CurrentTime;
						//resume = true;
						//_timer.Stop();
						//OnStop();
					}
					currentPhase = _selectedPhase;
					//if (resume)
					//{
					//	OnStart();
					//	_timer.Start();
					//}
				}
			}
		}

		TimeSpan CalculateSumOfWorkBlocks()
		{
			TimeSpan calculatedTimeSpan = TimeSpan.Zero;
			foreach (TimeBlock _timeBlock in CurrentTimeCard.WorkBlocks)
			{
				calculatedTimeSpan += _timeBlock.GetTimeSpan();
			}
			return calculatedTimeSpan;
		}
		public TimeCard CurrentTimeCard
		{
			get { return _currentTimeCard ?? (_currentTimeCard = new TimeCard(DateTime.Now)); }
			set { _currentTimeCard = value; }
		}
		TimeSpan _SumOfWorkComplete = TimeSpan.Zero;
		public TimeSpan TotalWorkTime = TimeSpan.Zero;
		public TimeSpan TotalBreakTime = TimeSpan.Zero;
		public TimeSpan tempBreakTime = TimeSpan.Zero;
		private Project _selectedProject;
		private Phase _selectedPhase;
		private ObservableCollection<Company> _companies;
		private ObservableCollection<Contact> _contacts;
		private ObservableCollection<Billing> _billings;

		public string HoursWorked
		{
			get { return _hoursWorked; }
			set { Set(() => HoursWorked, ref _hoursWorked, value, false); }
		}

		public DateTime CurrentTime => DateTime.Now;


		public string StartButtonContent
		{
			get { return _startButtonContent; }
			set { Set(() => StartButtonContent, ref _startButtonContent, value, false); }
		}

		public RelayCommand ShowPropertiesCommand { get; set; }

		public RelayCommand StopCommand { get; private set; }

		public event EventHandler<TimeCardEventArgs> TimeCardCreated;

		void OnStop()
		{
			//if (_isOnBreak)
			//{
			//	IsOnBreak = false;
			//	breakStopTime = DateTime.Now;
			//	CurrentTimeCard.AddBreakBlock(breakStartTime,breakStopTime);
			//	// stop break timmer and add it to total break time
			//	// start or restart on task timmer
			//	IsStarted = false;
			//	workStartTime = DateTime.Now;
			//}
			//else
			//{
			//if (_isStarted)
			//{
			//	IsStarted = false;

			// stop on task timer and 
			ValidateModel();
			if (IsValid)
			{
				workStopTime = CurrentTime;
				CurrentTimeCard.AddWorkBlock(TimeBlock.CreateWorkBlock(workStartTime, workStopTime, currentProject.ProjectId, currentPhase.PhaseId));
				_SumOfWorkComplete = CalculateSumOfWorkBlocks();
			}
			else
			{
				if (TimeCardCreated != null)
				{
					//CurrentTimeCardViewModel = new TimecardViewModel(CurrentTimeCard);

					TimeCardCreated(this, new TimeCardEventArgs(CurrentTimeCard, true, true));
				}
				workStopTime = CurrentTime;
				CurrentTimeCard.AddWorkBlock(TimeBlock.CreateWorkBlock( workStartTime, workStopTime, currentProject.ProjectId,currentPhase.PhaseId));
				_SumOfWorkComplete = CalculateSumOfWorkBlocks();
			}
			// save timecard
			timeCardRepository.Update(CurrentTimeCard);

			//}
			//}
			TimerState = State.Stopped;
			//IsStoped = true;
			//IsStarted = false;
			//IsOnBreak = false;
			StartButtonContent = "Start";
			TypeOfTime = "Not Working";
			TotalBreakTime = TimeSpan.Zero;
		}

		//bool OnStopCanExecute()
		//{
		//	return !IsStoped;
		//}

		public RelayCommand StartCommand { get; private set; }

		//bool OnStartCanExecute()
		//{
		//	return IsOnBreak || IsStoped;
		//}

		void OnStart()
		{
			if (_timerState == State.Paused)
			{
				// Stop break timer
				// _isOnBreak = false;
				breakStopTime = CurrentTime;
				TotalBreakTime += tempBreakTime;
				tempBreakTime = TimeSpan.Zero;

				CurrentTimeCard?.AddBreakBlock(TimeBlock.CreateBreakBlock(breakStartTime, breakStopTime));
			}
			else
			{
				// IsStoped = false;
				// is current time card null
				if (CurrentTimeCard == null)
				{
					CurrentTimeCard = new TimeCard();
				}
			}
			workStartTime = CurrentTime;
			TimerState = State.Running;

			// IsStarted = true;
			StartButtonContent = "Working";
			TypeOfTime = "Time at Work";
		}

		public RelayCommand PauseCommand { get; set; }


		private void OnBreak()
		{
			if (_timerState != State.Running) return;
			TimerState = State.Paused;
			//IsOnBreak = true;
			//IsStoped = false;
			//IsStarted = false;
			workStopTime = CurrentTime;
			CurrentTimeCard.AddWorkBlock(TimeBlock.CreateWorkBlock(workStartTime, workStopTime, currentProject.ProjectId, currentPhase.PhaseId));
			_SumOfWorkComplete = CalculateSumOfWorkBlocks();
			breakStartTime = CurrentTime;
			StartButtonContent = "Start";
			TypeOfTime = "On Break";
		}

		//private bool OnBreakCanExecute()
		//{
		//	return ! IsStoped;
		//}

		void TimerUpdate(object Sender, ElapsedEventArgs e)
		{
			TimeSpan totalTime;
			string showHours;
			string showMinutes;
			string showSeconds;
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
			showHours = totalTime.Hours.ToString();
			showMinutes = (totalTime.Minutes < 10 ? "0" : "") + totalTime.Minutes.ToString();
			showSeconds = (totalTime.Seconds < 10 ? "0" : "") + totalTime.Seconds.ToString();
			HoursWorked = $"{showHours}:{showMinutes}:{showSeconds}";
		}

		//void BreakTimerUpdate(object Sender, ElapsedEventArgs e)
		//{
		//	RaisePropertyChanged(() => CurrentTime);

		//}
		//void WorkTimerUpdate(object Sender, ElapsedEventArgs e)
		//{
		//	RaisePropertyChanged(() => CurrentTime);

		//}
	}
}