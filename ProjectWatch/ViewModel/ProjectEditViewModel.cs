using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Data.DataSets;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	public class ProjectEditViewModel : ViewModelCommon
	{
		private List<Company> _companies = null;
		private List<Contact> _contacts = null;
		private List<Contact> _allContacts = null;
		//private List<Phase> _phases = null;
		private DashboardViewModel dashboardViewModel = null;
		public ProjectEditViewModel() : this(new Project(-1))
		{
		}

		private Project _projectUnderEdit = new Project();
		private Contact _selectedProjectContact;
		private string _projectName;
		private string _projectNote;
		private Company _selectedProjectCompany;
		private string _projectCostQuote;
		private string _projectTimeQuote;
		private Contact _selectedBillingContact;
		private Contact _selectedManagementContact;
		private bool _isBillable;
		private string _rate;
		private ICompanyRepository _companyRepo;
		private IContactRepository _contactRepo;

		public ProjectEditViewModel(Project project)
		{
			_projectUnderEdit = project.Clone() as Project;
			SaveCommand = new RelayCommand(OnSave, CanSave);
		}

		protected override void OnViewLoaded()
		{
			dashboardViewModel = ClientEntityBase.Container.GetExportedValue<DashboardViewModel>();
			_companyRepo = ClientEntityBase.Container.GetExportedValue<ICompanyRepository>();
			_companies = _companyRepo.Get().EntitySet.ToList();
			//_phases = dashboardViewModel.Phases;
			_contactRepo = ClientEntityBase.Container.GetExportedValue<IContactRepository>();
			_allContacts = _contactRepo.Get().EntitySet.ToList();
			FormInit();
		}

		void FormInit()
		{
			_projectName = _projectUnderEdit.Name;
			_projectNote = _projectUnderEdit.Note;
			_isBillable = _projectUnderEdit.IsBillable;
			if (_projectUnderEdit.CompnayId > -1 && _companies.Any())
			{
				SelectedProjectCompany = _companies.Find(c => c.CompanyId == _projectUnderEdit.CompnayId);
			}
			if ( _projectUnderEdit.ManagementContactId > -1 && Contacts.Any())
			{
				SelectedProjectContact = _contacts.Find(c => c.ContactId == _projectUnderEdit.ManagementContactId);
			}
			if ( _projectUnderEdit.BillingContactId > -1 && Contacts.Any())
			{
				_selectedBillingContact = _contacts.Find(c => c.ContactId == _projectUnderEdit.BillingContactId);
			}
			if (_projectUnderEdit.CompnayId > -1 && Contacts.Any())
			{
				_contacts = _allContacts.FindAll(c => c.CompanyId == _projectUnderEdit.CompnayId);
			}
			_projectCostQuote = _projectUnderEdit.CostQuote> 0d ? $"{_projectUnderEdit.CostQuote:0.##}" : "0.0";
			_projectTimeQuote = _projectUnderEdit.TimeQuote > 0d ? $"{_projectUnderEdit.TimeQuote:0.##}" : "0.0";
			_rate =( _projectUnderEdit.Rate > 0d && _projectUnderEdit.IsBillable) ? $"{ProjectUnderEdit.Rate:0.##}" : "0.0";
			_projectUnderEdit.CleanAll();
		}

		bool CanSave()
		{
			return _projectUnderEdit.IsDirty && !string.IsNullOrEmpty(_projectName);
		}
		public RelayCommand SaveCommand { get; set; }

		public void OnSave()
		{
			if (_projectUnderEdit.ProjectId == -1)
			{
				dashboardViewModel.projectRepository.Add(_projectUnderEdit);
			}
			else
			{
				dashboardViewModel.projectRepository.Update(_projectUnderEdit);
			}
			_projectUnderEdit.CleanAll();
			dashboardViewModel.Projects = dashboardViewModel.projectRepository.Get().EntitySet.ToList();
			SaveCommand.RaiseCanExecuteChanged();
			//ProjectMainProjects = dashboardViewModel.Projects;
		}

		public string Rate
		{
			get { return _rate; }
			set
			{
				if (Set(() => Rate, ref _rate, value, false))
				{
					_projectUnderEdit.MakeDirty();
					double temp = -1.0d;
					if (double.TryParse(_rate, out temp))
					{
						_projectUnderEdit.Rate = temp;
					}
					else
					{
						Rate = "0.0";
					}
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public Project ProjectUnderEdit
		{
			get { return _projectUnderEdit; }
			set
			{
				if (Set(() => ProjectUnderEdit, ref _projectUnderEdit, value, false))
				{
					_projectUnderEdit.MakeDirty();
				}
				
			}
		}

		public string ProjectName
		{
			get
			{return _projectName;}
			set
			{
				if (Set(() => ProjectName, ref _projectName, value, false))
				{
					_projectUnderEdit.MakeDirty();
					_projectUnderEdit.Name = _projectName;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string ProjectNote
		{
			get
			{return _projectNote;}
			set
			{
				if (Set(() => ProjectNote, ref _projectNote, value, false))
				{
					_projectUnderEdit.MakeDirty();
					_projectUnderEdit.Note = ProjectNote;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}
		public Contact SelectedProjectContact  // todo rename
		{
			get
			{ return _selectedProjectContact; }
			set
			{
				if (Set(() => SelectedProjectContact, ref _selectedProjectContact, value, false))
				{
					_projectUnderEdit.MakeDirty();
					_projectUnderEdit.ManagementContactId = _selectedProjectContact.ContactId;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		//public CompanySet CompanyEntitySet
		//{
		//	get { return _companyEntitySet; }
		//	set
		//	{
		//		if (Set(() => CompanyEntitySet, ref _companyEntitySet, value, false))
		//		{
		//			Companies = _companyEntitySet.EntitySet.ToList();
		//		}
		//	}
		//}

		public List<Contact> Contacts
		{
			get { return _contacts; }
			set { Set(() => Contacts, ref _contacts, value, false); }
		}
		public List<Company> Companies
		{
			get { return _companies; }
			set { Set(() => Companies, ref _companies, value, false); }
		}

		public Company SelectedProjectCompany
		{
			get
			{ return _selectedProjectCompany; }
			set
			{
				if (Set(() => SelectedProjectCompany, ref _selectedProjectCompany, value, false))
				{
					_projectUnderEdit.MakeDirty();
					_projectUnderEdit.CompnayId = _selectedProjectCompany.CompanyId;
					SelectedProjectContact = null;
					SelectedBillingContact = null;
					if (_selectedProjectCompany == null)
					{
						Contacts = new List<Contact>();
					}
					else
					{
						Contacts = _allContacts.FindAll(c => c.CompanyId == _selectedProjectCompany.CompanyId);
					}
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public bool IsBillable
		{
			get { return _isBillable; }
			set
			{
				if (Set(() => IsBillable, ref _isBillable, value, false))
				{
					_projectUnderEdit.IsBillable = _isBillable;
					_projectUnderEdit.MakeDirty();
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string ProjectCostQuote
		{
			get
			{ return _projectCostQuote; }
			set
			{
				if (Set(() => ProjectCostQuote, ref _projectCostQuote, value, false))
				{
					_projectUnderEdit.MakeDirty();
					double temp = -1.0d;
					if ( double.TryParse(_projectCostQuote, out temp)) 
					{
						_projectUnderEdit.CostQuote = temp; 
					}
					else
					{
						ProjectCostQuote = "0.0";
					}
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}
		public Contact SelectedBillingContact
		{
			get
			{ return _selectedBillingContact; }
			set
			{
				if (Set(() => SelectedBillingContact, ref _selectedBillingContact, value, false))
				{
					_projectUnderEdit.MakeDirty();
					_projectUnderEdit.BillingContactId = _selectedBillingContact.ContactId;
				}
				SaveCommand.RaiseCanExecuteChanged();
			}
		}

		//public Contact SelectedManagementContact
		//{
		//	get { return _selectedManagementContact; }
		//	set
		//	{
		//		if (Set(() => SelectedManagementContact, ref _selectedManagementContact, value, false))
		//		{
		//			_projectUnderEdit.MakeDirty();
		//			_projectUnderEdit.ManagementContactId = _selectedManagementContact.ContactId;
		//		}
		//	}
		//}

		public string ProjectTimeQuote
		{
			get
			{ return _projectTimeQuote; }
			set
			{
				if (Set(() => ProjectTimeQuote, ref _projectTimeQuote, value, false))
				{
					_projectUnderEdit.MakeDirty();
					double temp = -1.0d;
					int indx = _projectTimeQuote.IndexOf('$');
					if (indx > -1)
					{
						_projectTimeQuote = _projectTimeQuote.Substring(1);
					}
					if (double.TryParse(_projectTimeQuote, out temp))
					{
						_projectUnderEdit.TimeQuote = temp;
					}
					else
					{
						ProjectTimeQuote = "0.00";
					}
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}
	}
}
