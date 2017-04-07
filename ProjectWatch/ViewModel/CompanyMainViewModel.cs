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
		public ICompanyRepository companyRepository;
		public IContactRepository contactRepository;
		//protected ObservableCollection<Company> companies;
		//protected ObservableCollection<Contact> employees;
		private List<Company> _companyMainCompanies;
		private List<Contact> _companyMainEmployees;
		private ViewModelCommon _currentViewModel;
		private CompanyViewModel _companyViewModel = null;

		public CompanyMainViewModel()
		{
			//companies = new ObservableCollection<Company>( companyRepository.Get().EntitySet);
			//employees = new ObservableCollection<Contact>(contactRepository.Get().EntitySet);
			EditCompanyCommand = new RelayCommand<Company>(OnEditCompany);
			EditContactCommand = new RelayCommand<Contact>(OnEditContact);
			AddCompanyCommand = new RelayCommand(OnAddCompany);
			AddContactCommand = new RelayCommand<Company>(OnAddContact);
			CompanyViewCommand = new RelayCommand(OnCompanyView);
			_companyViewModel = new CompanyViewModel();
			_currentViewModel = _companyViewModel;
		}

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

		public List<Contact> CompanyMainEmployees
		{
			get { return _companyMainEmployees; }
			//set { Set(() => CompanyMainEmployees, ref _companyMainEmployees, value, false); }
			set { _companyMainEmployees= value; }
		}

		public RelayCommand CompanyViewCommand { get; set; }

		void OnCompanyView()
		{
			CurrentViewModel = new CompanyViewModel();
		}
		public RelayCommand<Company> EditCompanyCommand { get; set; }

		public void OnEditCompany(Company company)
		{
			CurrentViewModel = new CompanyEditViewModel(company);

		}

		public RelayCommand<Contact> EditContactCommand { get; set; }

		void OnEditContact(Contact contact)
		{
			CurrentViewModel = new ContactEditViewModel(contact);

		}

		public RelayCommand AddCompanyCommand { get; set; }

		void OnAddCompany()
		{
			CurrentViewModel = new CompanyEditViewModel();
			//Company tempCompany = new Company(companyName);
			//tempCompany = companyRepository.Add(tempCompany);
			//CompanyMainCompanies = companyRepository.Get().EntitySet.ToList();
			//List<Company> tempCompanies = _companyMainCompanies.ToList();
			//tempCompanies.Add(tempCompany);
			//_companyMainCompanies = new ObservableCollection<Company>(tempCompanies);
			//(CurrentViewModel as CompanyViewModel).CompanyToAdd = "";
		}

		public RelayCommand<Company> AddContactCommand { get; set; }

		void OnAddContact( Company company)
		{
			CurrentViewModel = new ContactEditViewModel(company);
		}
		public ViewModelCommon CurrentViewModel
		{
			get { return _currentViewModel; }
			set {Set(() => CurrentViewModel, ref _currentViewModel,value,false); }
		}

		public override string ViewTitle
		{
			get { return "Company"; }
		}

		protected override void OnViewLoaded()
		{
			companyRepository = ClientEntityBase.Container.GetExportedValue<ICompanyRepository>();
			contactRepository = ClientEntityBase.Container.GetExportedValue<IContactRepository>();
			CompanyMainCompanies = companyRepository.Get().EntitySet.ToList();
			CompanyMainEmployees = contactRepository.Get().EntitySet.ToList();
		}
	}
}
