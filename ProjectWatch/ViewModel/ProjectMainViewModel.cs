using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class ProjectMainViewModel : ViewModelCommon
	{
		public ProjectMainViewModel()
		{
			EditProjectCommand = new RelayCommand<Project>(OnEditProject);
			ProjectViewCommand = new RelayCommand(OnViewProject);
			AddProjectCommand = new RelayCommand(OnAddProject);
			AddPhaseCommand = new RelayCommand<Project>(OnAddPhase);
			EditPhaseCommand = new RelayCommand<Phase>(OnEditPhase);
		}
		private ProjectViewModel _projectViewModel = null;
		protected ProjectEditViewModel _projectEditViewModel = null;
		private ViewModelCommon _currentViewModel;
		public RelayCommand<Project> EditProjectCommand { get; private set; }

		public RelayCommand ProjectViewCommand { get; set; }

		void OnViewProject()
		{
			//dashboardViewModel = ClientEntityBase.Container.GetExportedValue<DashboardViewModel>();
			CurrentViewModel = new ProjectViewModel();
		}
		void OnEditProject(Project project)
		{
			
			CurrentViewModel = new ProjectEditViewModel(project);
		}
		public RelayCommand AddProjectCommand { get; set; }

		void OnAddProject()
		{
			CurrentViewModel = new ProjectEditViewModel();
		}
		public RelayCommand<Project> AddPhaseCommand { get; set; }

		void OnAddPhase(Project project)
		{
			CurrentViewModel = new PhaseEditViewModel(project);
		}
		public RelayCommand<Phase> EditPhaseCommand { get; set; }

		void OnEditPhase(Phase phase)
		{
			CurrentViewModel = new PhaseEditViewModel(phase);
		}

		public ViewModelCommon CurrentViewModel
		{
			get { return _currentViewModel ; }
			set { Set(() => CurrentViewModel, ref _currentViewModel, value, false); }
		}
		public override string ViewTitle
		{
			get { return "Projects"; }
		}
		//private IProjectRepository projectRepository;
		//private IPhaseRepository phaseRepository;
		private DashboardViewModel dashboardViewModel;
		//private ProjectMainViewModel projectMainViewModel;
		//private ObservableCollection<Project> _projectMainProjects;
		public List<Project> ProjectMainProjects
		{
			get { return dashboardViewModel.Projects; }
			set { dashboardViewModel.Projects = value; }
		}

		protected override void OnViewLoaded()
		{
			//projectRepository = ClientEntityBase.Container.GetExportedValue<IProjectRepository>();
			//phaseRepository = ClientEntityBase.Container.GetExportedValue<IPhaseRepository>();
			dashboardViewModel = ClientEntityBase.Container.GetExportedValue<DashboardViewModel>();
			//projectMainViewModel = ClientEntityBase.Container.GetExportedValue<ProjectMainViewModel>();
			//ProjectMainProjects = dashboardViewModel.Projects;
			_projectViewModel = new ProjectViewModel();
			//_projectEditViewModel = new ProjectEditViewModel();
			_currentViewModel = _projectViewModel;
		}
	}
}
