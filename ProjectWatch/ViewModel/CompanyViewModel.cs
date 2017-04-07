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
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class CompanyViewModel : ViewModelCommon
	{
		private string _companyName;
		private string _totalEmployees;
		//protected ICompanyRepository companyRepository;
		//protected IContactRepository contactRepository;
		protected List<Company> _companies;
		//protected ObservableCollection<Contact> employees;
//		private List<Company> CompanyList = new List<Company>();
		private List<Contact> _employees = new List<Contact>();
		private Company _selectedCompany;
		private Contact _selectedContact;
		private CompanyMainViewModel _companyMain;
		private MainViewModel _mainViewModel;
		private bool _canAddEmployee;
		private string _companyToAdd;
		private bool _canAddCompany;
		private string _contactName;
		private string _note;

		public CompanyViewModel()
		{
			DeleteCompanyCommand = new RelayCommand<Company>(OnDeleteCompany);
			DeleteEmployeeCommand = new RelayCommand<Contact>(OnDeleteEmployee);
			_canAddEmployee = false;
			_canAddCompany = false;
			//companyRepository = ClientEntityBase.Container.GetExportedValue<CompanyRepository>();
			//contactRepository = ClientEntityBase.Container.GetExportedValue<ContactRepository>();
		}

		public RelayCommand<Company> DeleteCompanyCommand { get; set; }

		public void OnDeleteCompany(Company company)
		{
			List<Contact> _localContacts = _companyMain.CompanyMainEmployees.FindAll(e => e.CompanyId == company.CompanyId);
			foreach (Contact _employee in _localContacts)
			{
				_companyMain.contactRepository.Remove(_employee);
			}
			_companyMain.companyRepository.Remove(company);
			_companies.Remove(company);
			Companies = new List<Company>(_companies);
			SelectedCompany = null;
			// todo Add an Archive bool to the entity classes, so that the entity is not removed from the persisted set, but not shown
			// todo delte form company list and observable collection and serialize set
		}

		public RelayCommand<Contact> DeleteEmployeeCommand { get; set; }

		void OnDeleteEmployee(Contact contact)
		{
			_employees.Remove(contact);
			_companyMain.contactRepository.Remove(contact);
			Employees = new List<Contact>(_employees); 
		}
		public override string ViewTitle
		{
			get { return "Company"; }
		}

		public string ContactName
		{
			get { return _contactName; }
			set
			{
				Set(() => ContactName, ref _contactName, value, false);
			}
		}

		public string CompanyName
		{
			get { return _companyName; }
			set
			{
				Set(() => CompanyName, ref _companyName, value, false);
			}
		}

		//public string CompanyToAdd
		//{
		//	get { return _companyToAdd; }
		//	set {
		//		if ( Set(() => CompanyToAdd, ref _companyToAdd, value, false))
		//		{
		//			CanAddCompany = !(string.IsNullOrEmpty(_companyToAdd) && string.IsNullOrWhiteSpace(_companyToAdd));
		//		}
		//	}
		//}

		public List<Company> Companies
		{
			get { return _companies; }
			private set { Set(() => Companies, ref _companies, value, false); }
		}

		public List<Contact> Employees
		{
			get { return _employees; }
			set
			{
				if (Set(() => Employees, ref _employees, value, false))
				{
					//Employees = _employees;
					TotalEmployees = _employees.Count.ToString();
				}
			}
		}


		public Company SelectedCompany
		{
			get { return _selectedCompany; }
			set
			{
				CanAddEmployee = true;
				if (Set(() => SelectedCompany, ref _selectedCompany, value, false))
				{
					CompanyName = _selectedCompany.CompanyName;
					Employees = _companyMain.CompanyMainEmployees.FindAll(e => e.CompanyId == SelectedCompany.CompanyId);
					Note = _selectedCompany.Note;
					TotalEmployees = Employees.Count.ToString();
				}
			}
		}

		public string Note
		{
			get { return _note; }
			set { Set(() => Note, ref _note, value, false); }
		}

		public Contact SelectedContact
		{
			get { return _selectedContact; }
			set
			{
				if (Set(() => SelectedContact, ref _selectedContact, value, false))
				{
					ContactName = _selectedContact.ToString();
				}
			}
		}

		//public bool CanAddCompany
		//{
		//	get { return _canAddCompany; }
		//	set { Set(() => CanAddCompany, ref _canAddCompany, value,false); }
		//}

		public bool CanAddEmployee
		{
			get { return _canAddEmployee; }
			set { Set(() => CanAddEmployee, ref _canAddEmployee,value,false); }
		}

		public string TotalEmployees
		{
			get
			{
				return _totalEmployees;
			}
			private set { Set(() => TotalEmployees, ref _totalEmployees, value, false); }
		}
		protected override void OnViewLoaded()
		{
			//_mainViewModel = ClientEntityBase.Container.GetExportedValue<MainViewModel>();
			_companyMain = ClientEntityBase.Container.GetExportedValue<CompanyMainViewModel>();
			Companies = _companyMain.companyRepository.Get().EntitySet.ToList();

		}
	}
}
