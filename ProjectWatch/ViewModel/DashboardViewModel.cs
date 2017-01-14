using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using ProjectWatch.Support;
using ProjectWatch.Views;
using ProjectWatch.Entities;
using Timer = System.Timers.Timer;


namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
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
//				_typeOfTime = value;
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
		
		public TimecardViewModel CurrentTimeCardViewModel;

		public DashboardViewModel()
		{
			if (IsInDesignModeStatic)
			{
				StartButtonContent = "Start";
			}
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
		}

		// Fields...
//		private RelayCommand _pauseCommand;
//		private RelayCommand _startCommand;
//		private RelayCommand _stopCommand;
		private TimeCard _currentTimeCard;
		private string _hoursWorked = "0:0";
		private bool _isStarted = false;
		private bool _isStoped = true;
		private bool _isOnBreak = false;


		public bool IsStarted
		{
			get { return _isStarted; }
			set
			{
				_isStarted = value;
				RaisePropertyChanged(() => IsStarted);
				//StopCommand.RaiseCanExecuteChanged();
				//StartCommand.RaiseCanExecuteChanged();
				//PauseCommand.RaiseCanExecuteChanged();
			}
		}

		public bool IsStoped
		{
			get { return _isStoped; }
			set
			{
				_isStoped = value;
				//StopCommand.RaiseCanExecuteChanged();
				//StartCommand.RaiseCanExecuteChanged();
				//PauseCommand.RaiseCanExecuteChanged();
			}
		}

		public bool IsOnBreak
		{
			get { return _isOnBreak; }
			set
			{
				_isOnBreak = value;
				//StopCommand.RaiseCanExecuteChanged();
				//StartCommand.RaiseCanExecuteChanged();
				//PauseCommand.RaiseCanExecuteChanged();
			}
		}

		public ObservableCollection<Project> Projects
		{
			get { return _projects; }
			set { _projects = value; }
		}


		public ObservableCollection<Phase> Phases
		{
			get { return _phases; }
			set { _phases = value; }
		}


		public TimeCard CurrentTimeCard
		{
			get { return _currentTimeCard ?? (_currentTimeCard = new TimeCard()); }
			set { _currentTimeCard = value; }
		}

		public TimeSpan TotalWorkTime = TimeSpan.Zero;
		public TimeSpan TotalBreakTime = TimeSpan.Zero;
		public TimeSpan tempBreakTime = TimeSpan.Zero;

		public string HoursWorked
		{
			get { return _hoursWorked; }
			set { Set(() => HoursWorked, ref _hoursWorked, value, false); }
		}

		public DateTime CurrentTime
		{
			get { return DateTime.Now; }
		}


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
				CurrentTimeCard.AddWorkBlock(workStartTime, workStopTime);
			}
			else
			{
				if (TimeCardCreated != null)
				{
					CurrentTimeCardViewModel = new TimecardViewModel(CurrentTimeCard);

					TimeCardCreated(this, new TimeCardEventArgs(CurrentTimeCard, true, true));
				}
				workStopTime = DateTime.Now;
				CurrentTimeCard.AddWorkBlock(workStartTime, workStopTime);
			}
			// save timecard


			//}
			//}
			IsStoped = true;
			IsStarted = false;
			IsOnBreak = false;
			StartButtonContent = "Start";
			TypeOfTime = "Not Working";
			TotalBreakTime = TimeSpan.Zero;
			//StartAnim = "Inactive";
			//PauseAnim = "Inactive";
			//StopAnim = "Active";
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
			if (IsOnBreak)
			{
				// Stop break timer
				_isOnBreak = false;
				breakStopTime = DateTime.Now;
				TotalBreakTime += tempBreakTime;
				tempBreakTime = TimeSpan.Zero;

				if (CurrentTimeCard != null)
				{
					CurrentTimeCard.AddBreakBlock(breakStartTime, breakStopTime);
				}
			}
			else
			{
				IsStoped = false;
				// is current time card null
				if (CurrentTimeCard == null)
				{
					CurrentTimeCard = new TimeCard();
				}
				workStartTime = DateTime.Now;
			}

			IsStarted = true;
			StartButtonContent = "Working";
			TypeOfTime = "Time at Work";
			//StartAnim = "Active";
			//PauseAnim = "Inactive";
			//StopAnim = "Inactive";
			// Start button pushed			
		}

		public RelayCommand PauseCommand { get; set; }

		private void OnBreak()
		{
			if (_isStoped || _isOnBreak) return;

			IsOnBreak = true;
			IsStoped = false;
			IsStarted = false;
			breakStartTime = CurrentTime;
			StartButtonContent = "Start";
			TypeOfTime = "On Break";
			//StartAnim = "Inactive";
			//PauseAnim = "Active";
			//StopAnim = "Inactive";
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
			if (_isStarted)
			{
				TotalWorkTime = CurrentTime - workStartTime - TotalBreakTime; // TODO: remove break times
				totalTime = TotalWorkTime;
			}
			else if (_isOnBreak)
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
			HoursWorked = string.Format("{0}:{1}:{2}", showHours, showMinutes, showSeconds);
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