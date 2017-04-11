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
		#region Fields
		public ICompanyRepository companyRepository;
		public IPhaseRepository phaseRepository;
		public IProjectRepository projectRepository;
		private ViewModelCommon _currentViewModel;
		private TimecardViewModel _timeCardViewModel ;
		#endregion

		#region Constructors
		public TimeCardMainViewModel()
		{
			
			_timeCardViewModel = new TimecardViewModel();
			CurrentViewModel = _timeCardViewModel;
			EditTimeCardCommand = new RelayCommand<TimeCardDTO>(OnEditTimeCard);
			AddTimeCardCommand = new RelayCommand(OnAddTimeCard);
			TimecardViewCommand = new RelayCommand(OnTimecardView);
		}
		#endregion

		#region Properties
		public ViewModelCommon CurrentViewModel
		{
			get { return _currentViewModel; }
			set {Set(() => CurrentViewModel, ref _currentViewModel, value, false); }
		}
		public ObservableCollection<Company> TimeCardMainCompanies { get; set; }

		public ObservableCollection<Project> TimeCardMainProjects { get; set; }

		public ObservableCollection<Phase> TimeCardMainPhases { get; set; }
		#endregion

		#region Overrides



		protected override void OnViewLoaded()
		{
			companyRepository = ClientEntityBase.Container.GetExportedValue<ICompanyRepository>();
			phaseRepository = ClientEntityBase.Container.GetExportedValue<IPhaseRepository>();
			projectRepository = ClientEntityBase.Container.GetExportedValue<IProjectRepository>();
			TimeCardMainCompanies = new ObservableCollection<Company>(companyRepository.Get().EntitySet);
			TimeCardMainProjects = new ObservableCollection<Project>(projectRepository.Get().EntitySet);
			TimeCardMainPhases = new ObservableCollection<Phase>(phaseRepository.Get().EntitySet);
		}
		public override string ViewTitle
		{
			get { return "TimeCard"; }
		}
		#endregion


		#region Commands
		public RelayCommand TimecardViewCommand { get; set; }
		public RelayCommand AddTimeCardCommand { get; set; }
		public RelayCommand<TimeCardDTO> EditTimeCardCommand { get; set; }
		#endregion

		#region Methods
		void OnTimecardView()
		{
			CurrentViewModel = new TimecardViewModel();
		}
		void OnAddTimeCard()
		{
			CurrentViewModel = new TimecardEditViewModel();
		}
		void OnEditTimeCard(TimeCardDTO dto)
		{
			CurrentViewModel = new TimecardEditViewModel(dto.TCard);
		}
		#endregion
	}
}
