using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
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
	public class BillingMainViewModel : ViewModelCommon
	{
		#region Constructors
		public BillingMainViewModel()
		{
			VeiwBillingCommand = new RelayCommand<Billing>(OnVeiwBilling);
			EditBillingCommand = new RelayCommand<BillingDTO>(OnEditBilling);
			AddBillingCommand = new RelayCommand<Billing>(OnAddBilling);
		}
		#endregion


		#region Fields
		private BillingViewModel _billingViewModel = null;
		private BillingEditViewModel _billingEditViewModel;
		private ViewModelCommon _currentViewModel;
		#endregion
		#region Commands
		public RelayCommand<Billing> AddBillingCommand { get; set; }
		public RelayCommand BillingViewCommand { get; set; }
		public RelayCommand<BillingDTO> EditBillingCommand { get; set; }
		public RelayCommand<Billing> VeiwBillingCommand { get; set; }
		#endregion

		#region Methods
		public void OnAddBilling(Billing billing)
		{
			CurrentViewModel = new BillingEditViewModel(billing);
		}

		void OnBillingView()
		{
			CurrentViewModel = new BillingViewModel();
		}
		void OnEditBilling(BillingDTO billingDto)
		{
			CurrentViewModel = new BillingEditViewModel(billingDto.Bill);
		}
		void OnVeiwBilling(Billing billing)
		{
			CurrentViewModel = new BillingViewModel();
		}
		#endregion

		#region Properties
		public IProjectRepository ProjectRepo { get; set; }

		public IPhaseRepository PhaseRepo { get; set; }

		public ITimeCardRepository TimeCardRepo { get; set; }

		public ICompanyRepository CompanyRepo { get; set; }

		public IContactRepository ContactRepo { get; set; }

		public IBillingRepository BillingRepo { get; set; }

		public List<Company> Companies { get; set; }
		public List<Project> Projects { get; set; }
		public List<Phase> Phases { get; set; }
		public List<Contact> Contacts { get; set; }
		public ViewModelCommon CurrentViewModel
		{
			get { return _currentViewModel; }
			set {Set(() => CurrentViewModel, ref _currentViewModel, value, false); }
		}
		#endregion

		#region Overrides

		protected override void OnViewLoaded()
		{
			ProjectRepo = ClientEntityBase.Container.GetExportedValue<IProjectRepository>();
			Projects = ProjectRepo.Get().EntitySet.ToList();
			PhaseRepo = ClientEntityBase.Container.GetExportedValue<IPhaseRepository>();
			Phases = PhaseRepo.Get().EntitySet.ToList();
			TimeCardRepo = ClientEntityBase.Container.GetExportedValue<ITimeCardRepository>();
			CompanyRepo = ClientEntityBase.Container.GetExportedValue<ICompanyRepository>();
			Companies = CompanyRepo.Get().EntitySet.ToList();
			ContactRepo = ClientEntityBase.Container.GetExportedValue<IContactRepository>();
			Contacts = ContactRepo.Get().EntitySet.ToList();
			BillingRepo = ClientEntityBase.Container.GetExportedValue<IBillingRepository>();
			_billingViewModel = new BillingViewModel();
			_currentViewModel = _billingViewModel;
		}
		public override string ViewTitle => "Billing";
		#endregion
	}
}
