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
using ProjectWatch.Data.DataRepositories;
using ProjectWatch.Data.DataSets;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class CompanyMainViewModel : ViewModelCommon
	{
		#region Fields
		public ICompanyRepository companyRepository;
		public IContactRepository contactRepository;
		private List<Company> _companyMainCompanies;
		private ViewModelCommon _currentViewModel;
		private CompanyViewModel _companyViewModel = null;
		#endregion

		#region Constructors
		public CompanyMainViewModel()
		{
			EditCompanyCommand = new RelayCommand<Company>(OnEditCompany);
			EditContactCommand = new RelayCommand<Contact>(OnEditContact);
			AddCompanyCommand = new RelayCommand(OnAddCompany);
			AddContactCommand = new RelayCommand<Company>(OnAddContact);
			CompanyViewCommand = new RelayCommand(OnCompanyView);
			_companyViewModel = new CompanyViewModel();
			_currentViewModel = _companyViewModel;
		}
		#endregion

		#region Properties
		public List<Company> CompanyMainCompanies
		{
			get
			{
				return _companyMainCompanies;
			}
			set
			{
				Set(() => CompanyMainCompanies, ref _companyMainCompanies, value, false); 
				
			}
		}

		public List<Contact> CompanyMainEmployees { get; set; }

		public ViewModelCommon CurrentViewModel
		{
			get { return _currentViewModel; }
			set {Set(() => CurrentViewModel, ref _currentViewModel,value,false); }
		}
		#endregion

		#region Commands

		public RelayCommand AddCompanyCommand { get; set; }

		public RelayCommand<Company> AddContactCommand { get; set; }
		public RelayCommand CompanyViewCommand { get; set; }
		public RelayCommand<Company> EditCompanyCommand { get; set; }

		public RelayCommand<Contact> EditContactCommand { get; set; }
		#endregion

		#region Methods

		void OnAddCompany()
		{
			CurrentViewModel = new CompanyEditViewModel();
		}

		void OnAddContact( Company company)
		{
			CurrentViewModel = new ContactEditViewModel(company);
		}
		void OnCompanyView()
		{
			CurrentViewModel = new CompanyViewModel();
		}

		public void OnEditCompany(Company company)
		{
			CurrentViewModel = new CompanyEditViewModel(company);

		}

		void OnEditContact(Contact contact)
		{
			CurrentViewModel = new ContactEditViewModel(contact);

		}
		#endregion

		#region Overrides

		protected override void OnViewLoaded()
		{
			companyRepository = ClientEntityBase.Container.GetExportedValue<ICompanyRepository>();
			contactRepository = ClientEntityBase.Container.GetExportedValue<IContactRepository>();
			CompanyMainCompanies = companyRepository.Get().EntitySet.ToList();
			CompanyMainEmployees = contactRepository.Get().EntitySet.ToList();
		}
		public override string ViewTitle
		{
			get { return "Company"; }
		}
		#endregion
	}
}
