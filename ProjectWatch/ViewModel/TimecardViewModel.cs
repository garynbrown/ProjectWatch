using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Core;
using Core.Common.UI;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Data.DataRepositories;
using ProjectWatch.Data.DTO;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class TimecardViewModel : ViewModelCommon
	{
		private TimeCardMainViewModel timecardMain = null;
		List<Project> _projects = new List<Project>();
		List<Phase> _phases = new List<Phase>();
		List<Company> _companies = new List<Company>();
		public TimecardViewModel()
		{
		}

		public override string ViewTitle
		{
			get { return "Time Card Tools"; }
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TimecardViewModel"/> class.
		/// </summary>
		//public TimecardViewModel(TimeCard timeCard)
		//{
		//	_currentTimeCard = timeCard;
		//}
		// Fields...
		private TimeCard _currentTimeCard;
		private List<TimeCard> _timeCards;
		private TimeCard _selectedTimeCard;
		private ITimeCardRepository _timeCardRepository = null;
		private ObservableCollection<TimeCardDTO> _displayTimeCards;
		private bool _today;
		private bool _thisWeek;
		private bool _twoWeeks;
		private bool _twoMonths;
		private bool _customRange;
		private string _fromDate;
		private string _toDate;

		public TimeCard CurrentTimeCard
		{
			get { return _currentTimeCard; }
			set
			{
				_currentTimeCard = value;
			}
		}

		public ObservableCollection<TimeCardDTO> DisplayTimeCards
		{
			get { return _displayTimeCards; }
			set { Set(() => DisplayTimeCards, ref _displayTimeCards, value, false); }
		}

		public List<TimeCard> TimeCards
		{
			get { return _timeCards ?? (_timeCards = new List<TimeCard>()); }
			set
			{
				Set(() => TimeCards, ref _timeCards, value, false);
				//if (Set(() => TimeCards, ref _timeCards, value, false))
				//{
				//	MakeDisplayDtos(_timeCards);
				//}
			}
		}

		private void MakeDisplayDtos(List<TimeCard> timeCards )
		{
			List<TimeCardDTO> DTOs = new List<TimeCardDTO>();
			foreach (TimeCard _timeCard in timeCards)
			{
				DTOs.Add(new TimeCardDTO(_timeCard, timecardMain.TimeCardMainProjects.ToList(),
					timecardMain.TimeCardMainCompanies.ToList(), timecardMain.TimeCardMainPhases.ToList()));
			}
			DisplayTimeCards = new ObservableCollection<TimeCardDTO>(DTOs);
		}

		public TimeCard SelectedTimeCard
		{
			get { return _selectedTimeCard; }
			set
			{
				bool timeCardChanged = Set(() => SelectedTimeCard, ref _selectedTimeCard,value,false);
				if (timeCardChanged)
				{
					CurrentTimeCard = value;
				}

			}
		}

		public bool Today
		{
			get { return _today; }
			set
			{
				if (Set(() => Today, ref _today, value, false) && value)
				{
					TimeCards.Clear();
					_timeCards.Add( _timeCardRepository.GetTodaysTimeCard());
					MakeDisplayDtos(_timeCards);
				}
			}
		}

		public bool ThisWeek
		{
			get { return _thisWeek; }
			set
			{
				if (Set(() => ThisWeek, ref _thisWeek, value, false) && value)
				{
					_timeCards.Clear();
					DateTime from;
					DateTime to;
					int day = Convert.ToInt32( DateTime.Now.DayOfWeek);
					if (day == 0 || day == 6)
					{
						from = day == 0 ? DateTime.Today.AddDays(-6) : DateTime.Today.AddDays(-5);
						to = day == 0 ? DateTime.Today.AddDays(-2) : DateTime.Today.AddDays(-1);
					}
					else
					{
						from = DateTime.Today.AddDays(-1*(day-1));
						to = DateTime.Today;
					}
					_timeCards.AddRange(_timeCardRepository.GetRangeTimeCards(from,to));
					MakeDisplayDtos(_timeCards);
				}
			}
		}

		public bool TwoWeeks
		{
			get { return _twoWeeks; }
			set
			{
				if (Set(() => TwoWeeks, ref _twoWeeks, value, false) && value)
				{
					_timeCards.Clear();
					DateTime from;
					DateTime to;
					int day = Convert.ToInt32(DateTime.Now.DayOfWeek);
					if (day == 0 || day == 6)
					{
						from = day == 0 ? DateTime.Today.AddDays(-13) : DateTime.Today.AddDays(-12);
						to = day == 0 ? DateTime.Today.AddDays(-2) : DateTime.Today.AddDays(-1);
					}
					else
					{
						from = DateTime.Today.AddDays(-1 * (6+day));
						to = DateTime.Today;
					}
					TimeCards.AddRange(_timeCardRepository.GetRangeTimeCards(from, to));
					MakeDisplayDtos(_timeCards);
				}
			}
		}

		public bool TwoMonths
		{
			get { return _twoMonths; }
			set
			{
				if (Set(() => TwoMonths, ref _twoMonths, value, false) && value)
				{
					_timeCards.Clear();
					DateTime from = DateTime.Today.AddMonths(-2);
					DateTime to = DateTime.Today;
					
					_timeCards.AddRange(_timeCardRepository.GetRangeTimeCards(from, to));
					MakeDisplayDtos(_timeCards);
				}
			}
		}
		// todo move the backing fields of the radio buttons to the timecardMainViewModel.  So that when the user returns from editing, the same set of time cards are in the list box
		public bool CustomRange
		{
			get { return _customRange; }
			set
			{
				if (Set(() => CustomRange, ref _customRange, value, false) && _customRange)
				{
					if (!string.IsNullOrEmpty(_fromDate) && !string.IsNullOrEmpty(_toDate))
					{
						MakeDisplayDtos(GetCustomRange());
					}
				}
			}
		}

		public string FromDate
		{
			get { return _fromDate; }
			set
			{
				if (Set(() => FromDate, ref _fromDate, value, false) )
				{
					DateTime tempTime;
					if (!DateTime.TryParse(_fromDate, out tempTime))
					{
						FromDate = "";
					}
					else
					{
						MakeDisplayDtos(GetCustomRange());
					}
				}
			}
		}

		List<TimeCard> GetCustomRange()
		{
			_timeCards.Clear();
			if (CustomRange && !string.IsNullOrEmpty(_fromDate) && !string.IsNullOrEmpty(_toDate))
			{
				DateTime _toDateTime;
				DateTime _fromDateTime;
				DateTime.TryParse(_fromDate, out _fromDateTime);
				DateTime.TryParse(_toDate, out _toDateTime);
				if (_toDateTime < _fromDateTime)
					return _timeCards;
				_timeCards.Clear();
				_timeCards.AddRange(_timeCardRepository.GetRangeTimeCards(_fromDateTime, _toDateTime));
			}
			return _timeCards ;
		}

		public string ToDate
		{
			get { return _toDate; }
			set
			{
				if (Set(() => ToDate, ref _toDate, value, false))
				{
					DateTime tempTime;
					if (!DateTime.TryParse(_toDate, out tempTime))
					{
						ToDate = "";
					}
					else
					{
						MakeDisplayDtos(GetCustomRange());
					}
				}
			}
		}

		public List<Project> Projects
		{
			get { return _projects; }
			set
			{
				if (Set(() => Projects, ref _projects, value, false))
				{ }
			}
		}

		public List<Phase> Phases
		{
			get { return _phases; }
			set
			{
				if ( Set(() => Phases, ref _phases, value, false))
				{ }
			}
		}

		public List<Company> Companies
		{
			get { return _companies; }
			set
			{
				if (Set(() => Companies, ref _companies, value, false))
				{ }
			}
		}

		protected override void OnViewLoaded()
		{
			timecardMain = ClientEntityBase.Container.GetExportedValue<TimeCardMainViewModel>();
			Projects = timecardMain.projectRepository.Get().EntitySet as List<Project>;
			Phases = timecardMain.phaseRepository.Get().EntitySet as List<Phase>;
			Companies = timecardMain.companyRepository.Get().EntitySet as List<Company>;
			_timeCardRepository = ClientEntityBase.Container.GetExportedValue<ITimeCardRepository>();
			Today = true;
		}
	}
}
