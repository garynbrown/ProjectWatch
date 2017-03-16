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
			dashboardViewModel.projectRepository.Remove(project);
		}

		public RelayCommand<Phase> DeletePhaseCommand { get; set; }

		void OnDeletePhase(Phase phase)
		{
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
					_phases = _phases ?? dashboardViewModel.Phases.ToList().FindAll(p => p.ProjectId == _selectedProject.ProjectId);
				}
				return CanAddPhase ? _phases : null;
			}
			private set
			{
				Set(() => Phases, ref _phases, value, false);
			}
		}

		public Phase SelectedPhase { get; set; }
		public Project SelectedProject
		{
			get { return _selectedProject; }
			set
			{
				CanAddPhase = true;
				if (Set(() => SelectedProject, ref _selectedProject, value, false))
				{
					CurrentProject = value;
					Phases = dashboardViewModel.Phases.ToList().FindAll(p => p.ProjectId == _selectedProject.ProjectId);
				}
			}
		}

		private Project _selectedProject;
		private bool _canAddPhase;

		public bool CanAddPhase
		{
			get { return _canAddPhase; }
			set { Set(() => CanAddPhase, ref _canAddPhase, value, false); }
		}

		public override string ViewTitle
		{
			get { return "Projects"; }
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

		//protected override void OnViewLoaded()
		//{
		//	base.OnViewLoaded();

		//}
	}
}
