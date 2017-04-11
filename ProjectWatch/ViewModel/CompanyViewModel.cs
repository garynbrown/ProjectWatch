using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class CompanyViewModel : ViewModelCommon
	{
		#region Fields
		private string _companyName;
		private string _totalEmployees;
		protected List<Company> _companies;
		private List<Contact> _employees = new List<Contact>();
		private Company _selectedCompany;
		private Contact _selectedContact;
		private CompanyMainViewModel _companyMain;
		private bool _canAddEmployee;
		private string _contactName;
		private string _note;
		#endregion

		#region Constructors
		public CompanyViewModel()
		{
			DeleteCompanyCommand = new RelayCommand<Company>(OnDeleteCompany);
			DeleteEmployeeCommand = new RelayCommand<Contact>(OnDeleteEmployee);
			_canAddEmployee = false;
		}
		#endregion

		#region Commands
		public RelayCommand<Company> DeleteCompanyCommand { get; set; }

		public RelayCommand<Contact> DeleteEmployeeCommand { get; set; }
		#endregion

		#region Methods
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
		}

		void OnDeleteEmployee(Contact contact)
		{
			_employees.Remove(contact);
			_companyMain.contactRepository.Remove(contact);
			Employees = new List<Contact>(_employees); 
		}
		#endregion
		#region Overrides
		protected override void OnViewLoaded()
		{
			_companyMain = ClientEntityBase.Container.GetExportedValue<CompanyMainViewModel>();
			Companies = _companyMain.companyRepository.Get().EntitySet.ToList();
		}
		public override string ViewTitle
		{
			get { return "Company"; }
		}
		#endregion

		#region Properties


		public bool CanAddEmployee
		{
			get { return _canAddEmployee; }
			set { Set(() => CanAddEmployee, ref _canAddEmployee,value,false); }
		}

		public List<Company> Companies
		{
			get { return _companies; }
			private set { Set(() => Companies, ref _companies, value, false); }
		}

		public string CompanyName
		{
			get { return _companyName; }
			set
			{
				Set(() => CompanyName, ref _companyName, value, false);
			}
		}
		public string ContactName
		{
			get { return _contactName; }
			set
			{
				Set(() => ContactName, ref _contactName, value, false);
			}
		}

		public List<Contact> Employees
		{
			get { return _employees; }
			set
			{
				if (Set(() => Employees, ref _employees, value, false))
				{
					TotalEmployees = _employees.Count.ToString();
				}
			}
		}

		public string Note
		{
			get { return _note; }
			set { Set(() => Note, ref _note, value, false); }
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

		public string TotalEmployees
		{
			get
			{
				return _totalEmployees;
			}
			private set { Set(() => TotalEmployees, ref _totalEmployees, value, false); }
		}
		#endregion
	}
}
