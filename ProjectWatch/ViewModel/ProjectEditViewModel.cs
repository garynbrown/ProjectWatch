using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	public class ProjectEditViewModel : ViewModelCommon
	{
		private ObservableCollection<Company> _companies = null;
		private ObservableCollection<Contact> _contacts = null;
		private ObservableCollection<Phase> _phases = null;
		private DashboardViewModel dashboardViewModel = null;
		public ProjectEditViewModel() : this(new Project(-1))
		{
		}

		private Project _projectUnderEdit = new Project();
		private Contact _projectContact;
		private string _projectName;
		private string _projectNote;
		private Company _projectCompany;
		private string _projectCostQuote;
		private string _projectTimeQuote;
		private Contact _billingContact;
		private Contact _managementContact;

		public ProjectEditViewModel(Project project)
		{
			_projectUnderEdit = project.Clone() as Project;
			SaveCommand = new RelayCommand(OnSave);
		}

		protected override void OnViewLoaded()
		{
			dashboardViewModel = ClientEntityBase.Container.GetExportedValue<DashboardViewModel>();
			Companies = dashboardViewModel.Companies;				// new ObservableCollection<Company>(dashboardViewModel.DashboardCompanySet.EntitySet);
			_phases = dashboardViewModel.Phases;
			Contacts = dashboardViewModel.Contacts;
		}

		public RelayCommand SaveCommand { get; set; }

		public void OnSave()
		{
			if (ProjectUnderEdit.ProjectId == -1)
			{
				dashboardViewModel.projectRepository.Add(ProjectUnderEdit);
			}
			else
			{
				dashboardViewModel.projectRepository.Update(ProjectUnderEdit);
			}
			dashboardViewModel.Projects = new ObservableCollection<Project>(dashboardViewModel.projectRepository.Get().EntitySet);
			//ProjectMainProjects = dashboardViewModel.Projects;
		}


		public Project ProjectUnderEdit
		{
			get { return _projectUnderEdit; }
			set
			{
				if (Set(() => ProjectUnderEdit, ref _projectUnderEdit, value, false))
				{
					ProjectUnderEdit.MakeDirty();
				}
				
			}
		}

		public string ProjectName
		{
			get
			{
				_projectName = _projectName ?? ProjectUnderEdit.Name;
				return _projectName;
			}
			set
			{
				if (Set(() => ProjectName, ref _projectName, value, false))
				{
					ProjectUnderEdit.MakeDirty();
					ProjectUnderEdit.Name = _projectName;
				}

			}
		}

		public string ProjectNote
		{
			get
			{
				_projectNote = _projectNote ?? ProjectUnderEdit.Note;
				return _projectNote;
			}
			set
			{
				if (Set(() => ProjectNote, ref _projectNote, value, false))
				{
					ProjectUnderEdit.MakeDirty();
					ProjectUnderEdit.Note = ProjectNote;
				}
			}
		}
// todo the project and the phase now have billing contacts and Manageing contacts, update combo boxes and properties
		public Contact ProjectContact
		{
			get { return _projectContact; }
			set { _projectContact = value; }
		}

		public ObservableCollection<Contact> Contacts
		{
			get { return _contacts; }
			set { _contacts = value; }
		}
// todo Companies arn't loading 
		public ObservableCollection<Company> Companies
		{
			get { return _companies; }
			set { _companies = value; }
		}
		// todo Companies arn't loading  the combo box

		public Company ProjectCompany
		{
			get { return _projectCompany; }
			set { _projectCompany = value; }
		}

		public string ProjectCostQuote
		{
			get
			{
				_projectCostQuote = _projectCostQuote ?? ProjectUnderEdit.CostQuote.ToString();
				return _projectCostQuote;
			}
			set
			{
				if (Set(() => ProjectCostQuote, ref _projectCostQuote, value, false))
				{
					ProjectUnderEdit.MakeDirty();
					double temp = -1.0d;
					if ( double.TryParse(_projectCostQuote, out temp)) 
					{
						ProjectUnderEdit.CostQuote = temp; 
					}
					else
					{
						ProjectCostQuote = "0.0";
					}
				}
			}
		}
		public Contact BillingContact
		{
			get { return _billingContact; }
			set { _billingContact = value; }
		}

		public Contact ManagementContact
		{
			get { return _managementContact; }
			set { _managementContact = value; }
		}

		public string ProjectTimeQuote
		{
			get
			{
				_projectTimeQuote = _projectTimeQuote ?? ProjectUnderEdit.TimeQuote.ToString();
				return _projectTimeQuote;
			}
			set
			{
				if (Set(() => ProjectTimeQuote, ref _projectTimeQuote, value, false))
				{
					ProjectUnderEdit.MakeDirty();
					double temp = -1.0d;
					int indx = _projectTimeQuote.IndexOf('$');
					if (indx > -1)
					{
						_projectTimeQuote = _projectTimeQuote.Substring(1);
					}
					if (double.TryParse(_projectTimeQuote, out temp))
					{
						ProjectUnderEdit.TimeQuote = temp;
					}
					else
					{
						ProjectTimeQuote = "0.00";
					}
				}
			}
		}
	}
}
