using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Data.DTO;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class TimeCardMainViewModel : ViewModelCommon
	{
		public ICompanyRepository companyRepository;
		public IPhaseRepository phaseRepository;
		public IProjectRepository projectRepository;
		private ObservableCollection<Company> _timeCardMainCompanies;
		private ObservableCollection<Project> _timeCardMainProjects;
		private ObservableCollection<Phase> _timeCardMainPhases;
		private ViewModelCommon _currentViewModel;
		private TimecardViewModel _timeCardViewModel = null;

		public TimeCardMainViewModel()
		{
			
			_timeCardViewModel = new TimecardViewModel();
			CurrentViewModel = _timeCardViewModel;
			EditTimeCardCommand = new RelayCommand<TimeCardDTO>(OnEditTimeCard);
			AddTimeCardCommand = new RelayCommand(OnAddTimeCard);
			TimecardViewCommand = new RelayCommand(OnTimecardView);
		}

		public ViewModelCommon CurrentViewModel
		{
			get { return _currentViewModel; }
			set {Set(() => CurrentViewModel, ref _currentViewModel, value, false); }
		}

		public override string ViewTitle
		{
			get { return "TimeCard"; }
		}

		public ObservableCollection<Company> TimeCardMainCompanies
		{
			get { return _timeCardMainCompanies; }
			set { _timeCardMainCompanies = value; }
		}

		public ObservableCollection<Project> TimeCardMainProjects
		{
			get { return _timeCardMainProjects; }
			set { _timeCardMainProjects = value; }
		}

		public ObservableCollection<Phase> TimeCardMainPhases
		{
			get { return _timeCardMainPhases; }
			set { _timeCardMainPhases = value; }
		}

		public RelayCommand TimecardViewCommand { get; set; }

		void OnTimecardView()
		{
			CurrentViewModel = new TimecardViewModel();
		}
		public RelayCommand AddTimeCardCommand { get; set; }

		void OnAddTimeCard()
		{
			CurrentViewModel = new TimecardEditViewModel();
		}
		public RelayCommand<TimeCardDTO> EditTimeCardCommand { get; set; }

		void OnEditTimeCard(TimeCardDTO dto)
		{
			CurrentViewModel = new TimecardEditViewModel(dto.TCard);
		}

		protected override void OnViewLoaded()
		{
			companyRepository = ClientEntityBase.Container.GetExportedValue<ICompanyRepository>();
			phaseRepository = ClientEntityBase.Container.GetExportedValue<IPhaseRepository>();
			projectRepository = ClientEntityBase.Container.GetExportedValue<IProjectRepository>();
			TimeCardMainCompanies = new ObservableCollection<Company>(companyRepository.Get().EntitySet);
			TimeCardMainProjects = new ObservableCollection<Project>(projectRepository.Get().EntitySet);
			TimeCardMainPhases = new ObservableCollection<Phase>(phaseRepository.Get().EntitySet);
		}
	}
}
