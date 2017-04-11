using System.Collections.Generic;
using System.ComponentModel.Composition;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class ProjectMainViewModel : ViewModelCommon
	{
		#region Constructors
		public ProjectMainViewModel()
		{
			EditProjectCommand = new RelayCommand<Project>(OnEditProject);
			ProjectViewCommand = new RelayCommand(OnViewProject);
			AddProjectCommand = new RelayCommand(OnAddProject);
			AddPhaseCommand = new RelayCommand<Project>(OnAddPhase);
			EditPhaseCommand = new RelayCommand<Phase>(OnEditPhase);
		}
		#endregion
		#region Fields
		private ProjectViewModel _projectViewModel = null;
		protected ProjectEditViewModel _projectEditViewModel = null;
		private ViewModelCommon _currentViewModel;
		private DashboardViewModel dashboardViewModel;
		#endregion
		#region Commands
		public RelayCommand<Project> EditProjectCommand { get; private set; }
		public RelayCommand ProjectViewCommand { get; set; }
		public RelayCommand AddProjectCommand { get; set; }
		public RelayCommand<Project> AddPhaseCommand { get; set; }
		public RelayCommand<Phase> EditPhaseCommand { get; set; }
		#endregion


		#region Methods
		void OnViewProject()
		{
			CurrentViewModel = new ProjectViewModel();
		}
		void OnEditProject(Project project)
		{
			CurrentViewModel = new ProjectEditViewModel(project);
		}
		void OnAddProject()
		{
			CurrentViewModel = new ProjectEditViewModel();
		}
		void OnAddPhase(Project project)
		{
			CurrentViewModel = new PhaseEditViewModel(project);
		}
		void OnEditPhase(Phase phase)
		{
			CurrentViewModel = new PhaseEditViewModel(phase);
		}
		#endregion

		#region Properties
		public ViewModelCommon CurrentViewModel
		{
			get { return _currentViewModel ; }
			set { Set(() => CurrentViewModel, ref _currentViewModel, value, false); }
		}
		public override string ViewTitle
		{
			get { return "Projects"; }
		}
		public List<Project> ProjectMainProjects
		{
			get { return dashboardViewModel.Projects; }
			set { dashboardViewModel.Projects = value; }
		}
		#endregion

		#region Overrides
		protected override void OnViewLoaded()
		{
			dashboardViewModel = ClientEntityBase.Container.GetExportedValue<DashboardViewModel>();
			_projectViewModel = new ProjectViewModel();
			_currentViewModel = _projectViewModel;
		}
		#endregion
	}
}
