using System.Collections.Generic;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using Core.Common.Core;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
	[Export]
	[PartCreationPolicy(CreationPolicy.Shared)]
    public class MainViewModel : ViewModelCommon
	{
		private DashboardViewModel _dashboardVm;
		//private List<Company> _companies;
		//private ICompanyRepository _companyRepo;
		//private List<Contact> _contacts;
		//private IContactRepository _contactRepo;

		//[Import]
		//public AdministationViewModel AdministationViewModel
		//{
		//	get { return ClientEntityBase.Container.GetExportedValue<AdministationViewModel>(); }
		//}

		//[Import]
		//public SettingsViewModel SettingsViewModel
		//{
		//	get { return ClientEntityBase.Container.GetExportedValue<SettingsViewModel>(); }
		//}
		[Import]
		public ProjectMainViewModel ProjectMainViewModel
		{
			get { return ClientEntityBase.Container.GetExportedValue<ProjectMainViewModel>(); }
		}
		[Import]
		public CompanyMainViewModel CompanyMainViewModel 
		{
			get { return ClientEntityBase.Container.GetExportedValue<CompanyMainViewModel>(); }
		}


		//[Import]
		//public ReportMainViewModel ReportsViewModel
		//{
		//	get { return ClientEntityBase.Container.GetExportedValue<ReportMainViewModel>(); }
		//}
		[Import]
		public TimeCardMainViewModel TimeCardMainViewModel
		{
			get { return ClientEntityBase.Container.GetExportedValue<TimeCardMainViewModel>(); }
		}

		[Import]
		public BillingMainViewModel BillingMainViewModel
		{
			get { return ClientEntityBase.Container.GetExportedValue<BillingMainViewModel>(); }
		}

		/// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
			ClosingCommand = new RelayCommand<RoutedEventArgs>(OnClosing);
        }

		public DashboardViewModel DashboardVM
		{
			get { return _dashboardVm; }
			set { _dashboardVm = value; }
		}

		//public List<Company> Companies
		//{
		//	get { return _companies; }
		//	set { Set(() => Companies, ref _companies, value, false); }
		//}

		//public ICompanyRepository CompanyRepo
		//{
		//	get { return _companyRepo; }
		//	set { _companyRepo = value; }
		//}

		//public List<Contact> Contacts
		//{
		//	get { return _contacts; }
		//	set { _contacts = value; }
		//}

		//public IContactRepository ContactRepo
		//{
		//	get { return _contactRepo; }
		//	set { _contactRepo = value; }
		//}
		public RelayCommand<RoutedEventArgs> ClosingCommand { get; set; }

		void OnClosing(RoutedEventArgs args)
		{
			_dashboardVm.IsPropertiesShowing = false;
		}
		protected override void OnViewLoaded()
		{
			_dashboardVm = ClientEntityBase.Container.GetExportedValue<DashboardViewModel>();
			//_companyRepo = ClientEntityBase.Container.GetExportedValue<ICompanyRepository>();
			//_companies = _companyRepo.Get().EntitySet.ToList();
			//_contactRepo = ClientEntityBase.Container.GetExportedValue<IContactRepository>();
			//_contacts = _contactRepo.Get().EntitySet.ToList();
		}
	}
}