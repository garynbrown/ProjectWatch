using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Entities;
using System.Collections;
using System.Collections.ObjectModel;
using Core.Common.Core;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Data.DataRepositories;

namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class ProjectViewModel : ViewModelCommon
	{
		// Fields...
		private string _currentCompanyName = String.Empty;
		private Project _currentProject;
		private List<Phase> _phases;
		private DashboardViewModel dashboardViewModel = null;
		private List<Company> _companies;


		public Project CurrentProject
		{
			get { return _currentProject; }
			set
			{
				if (Set(() => CurrentProject, ref _currentProject, value, false))
				{
					
				}
			}
		}

		public RelayCommand<Project> DeleteProjectCommand { get; set; }

		void OnDeleteProject(Project project)
		{
			List<Phase> _phases = dashboardViewModel.AllPhases.FindAll(p => p.ProjectId == project.ProjectId);
			foreach (Phase _phase in _phases)
			{
				dashboardViewModel.AllPhases.Remove(_phase);
				dashboardViewModel.phaseRepository.Remove(_phase);
			}
			dashboardViewModel.AllPhases = new List<Phase>(dashboardViewModel.AllPhases);
			Phases = new List<Phase>();
			dashboardViewModel.Projects.Remove(project);
			dashboardViewModel.Projects = new List<Project>(dashboardViewModel.Projects);
			dashboardViewModel.projectRepository.Remove(project);
			Projects = dashboardViewModel.Projects;
		}

		public List<Project> Projects
		{
			get { return _projects; }
			set { Set(() => Projects, ref _projects, value, false); }
		}

		public RelayCommand<Phase> DeletePhaseCommand { get; set; }

		void OnDeletePhase(Phase phase)
		{
			dashboardViewModel.AllPhases.Remove(phase);
			dashboardViewModel.AllPhases = new List<Phase>(dashboardViewModel.AllPhases);
			dashboardViewModel.phaseRepository.Remove(phase);
		}
		//public RelayCommand AddProjectCommand { get; set; }

		//void OnAddProject()
		//{
		//	
		//}
		public List<Phase> Phases
		{
			get
			{
				if (CanAddPhase)
				{
					_phases = _phases ?? dashboardViewModel.AllPhases.ToList().FindAll(p => p.ProjectId == _selectedProject.ProjectId);
				}
				return CanAddPhase ? _phases : null;
			}
			private set
			{
				Set(() => Phases, ref _phases, value, false);
			}
		}

		public Phase SelectedPhase
		{
			get { return _selectedPhase; }
			set
			{
				if (Set(() => SelectedPhase, ref _selectedPhase, value, false))
				{
					//if (_selectedPhase.ManagementContactId > -1 && _allContacts.Any())
					//{
					//	ContactName = _allContacts.Find(c => c.ContactId == _selectedPhase.ManagementContactId).ToString();
					//}
				}
			}
		}

		public Project SelectedProject
		{
			get { return _selectedProject; }
			set
			{
				CanAddPhase = true;
				if (Set(() => SelectedProject, ref _selectedProject, value, false))
				{
					CurrentProject = value;
					Phases = dashboardViewModel.AllPhases.ToList().FindAll(p => p.ProjectId == _selectedProject.ProjectId);
					if (_selectedProject.ManagementContactId > -1 && _allContacts.Any())
					{
						ContactName = _allContacts.Find(c => c.ContactId == _selectedProject.ManagementContactId).ToString();
					}
					else
					{
						ContactName = "";
					}
					if (_selectedProject.CompnayId > -1 && _companies.Any())
					{
						CurrentCompanyName = _companies.Find(c => c.CompanyId == _selectedProject.CompnayId).CompanyName;
					}
					else
					{
						CurrentCompanyName = "";
					}
					
				}
			}
		}

		private Project _selectedProject;
		private bool _canAddPhase;
		private string _contactName;
		private Phase _selectedPhase;
		private List<Contact> _contacts;
		private List<Contact> _allContacts;
		private List<Project> _projects;

		public bool CanAddPhase
		{
			get { return _canAddPhase; }
			set { Set(() => CanAddPhase, ref _canAddPhase, value, false); }
		}

		public override string ViewTitle
		{
			get { return "Projects"; }
		}

		public string CurrentCompanyName
		{
			get { return _currentCompanyName; }
			set { Set(() => CurrentCompanyName, ref _currentCompanyName, value, false); }
		}

		public ProjectViewModel()
		{
			dashboardViewModel = ClientEntityBase.Container.GetExportedValue<DashboardViewModel>();
			DeleteProjectCommand = new RelayCommand<Project>(OnDeleteProject);
			DeletePhaseCommand = new RelayCommand<Phase>(OnDeletePhase);
			CanAddPhase = false;
			//if (!IsInDesignMode)
			//{
			//	_projects.Add(new Project() {Name = "Autometrix",Note = "PatternSmith"});
			//	_projects.Add(new Project() { Name = "Project Watch", Note = "Win 7"});
			//	_projects.Add(new Project() { Name = "TrustNet", Note = "Client Win 7"});
			//}
			//NavCommand = new RelayCommand<string>(OnNav);
			//projectRepository = ClientEntityBase.Container.GetExportedValue<IProjectRepository>();
			//phaseRepository = ClientEntityBase.Container.GetExportedValue<IPhaseRepository>();
			//dashboardViewModel = ClientEntityBase.Container.GetExportedValue<DashboardViewModel>();
			//projectMainViewModel = ClientEntityBase.Container.GetExportedValue<ProjectMainViewModel>();
			//Projects = dashboardViewModel.Projects;
//			Projects = new ObservableCollection<Project>(projectRepository.Get().EntitySet);
//			CurrentProject = dashboardViewModel
			//ProjectSelectionChangeCommand = new RelayCommand(onProjectSelectionChange);
		}

		public string ContactName
		{
			get { return _contactName; }
			set { Set(() => ContactName, ref _contactName, value, false); }
		}

		public List<Contact> Contacts
		{
			get { return _contacts; }
			set { Set(() => Contacts, ref _contacts, value, false); }
		}

		protected override void OnViewLoaded()
		{
			_allContacts = dashboardViewModel.Contacts;
			_companies = dashboardViewModel.Companies;
			Projects = dashboardViewModel.Projects;
		}
	}
}
