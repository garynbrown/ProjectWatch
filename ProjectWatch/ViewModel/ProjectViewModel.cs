using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.UI.Core;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Entities;
using System.Collections;

namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class ProjectViewModel : ViewModelCommon
	{
		// Fields...
		private string _currentCompanyName;
private Project _currentProject;
		private List<Project> _projects = new List<Project>();
		private List<Phase> _phases;

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

		public string CurrentCompanyName => _currentCompanyName;

		public List<Project> Projects
		{
			get { return _projects; }
			set { _projects = value; }
		}

		public List<Phase> Phases
		{
			get { return _phases; }
			set { _phases = value; }
		}

		public Project SelectedProject
		{
			get { return _selectedProject; }
			set
			{
				if (Set(() => SelectedProject, ref _selectedProject, value, false))
				{
					CurrentProject = value;
				}
			}
		}

		private Project _selectedProject;

		//public RelayCommand ProjectSelectionChangeCommand { get; set; }

		//void onProjectSelectionChange()
		//{
		//	return;
		//}
		public override string ViewTitle
		{
			get { return "Projects"; }
		}

		public ProjectViewModel()
		{
			if (!IsInDesignMode)
			{
				_projects.Add(new Project() {Name = "Autometrix",Note = "PatternSmith"});
				_projects.Add(new Project() { Name = "Project Watch", Note = "Win 7"});
				_projects.Add(new Project() { Name = "TrustNet", Note = "Client Win 7"});
			}
			else
			{
				_projects.Add(new Project() { Name = "Autometrix" });
				_projects.Add(new Project() { Name = "Project Watch" });
				_projects.Add(new Project() { Name = "TrustNet" });
			}
			//ProjectSelectionChangeCommand = new RelayCommand(onProjectSelectionChange);
		}
	}
}
