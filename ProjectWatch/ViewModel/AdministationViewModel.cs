using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.UI.Core;

namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class AdministationViewModel : ViewModelCommon
	{
		private ProjectViewModel _projectViewModel = new ProjectViewModel();
		PhaseViewModel _phaseViewModel = new PhaseViewModel();
		CompanyViewModel _companyViewModel = new CompanyViewModel();
		ReportsViewModel _reportsViewModel = new ReportsViewModel();
		SettingsViewModel _settingsViewModel = new SettingsViewModel();

		private ViewModelCommon _currentViewModel;

		public ViewModelCommon CurrentViewModel
		{
			get{return _currentViewModel;}
			set { Set(() => CurrentViewModel, ref _currentViewModel, value); }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AdministationViewModel"/> class.
		/// </summary>
		public override string ViewTitle
		{
			get { return "Administration Tools"; }
		}

		private void OnNav(string destination)
		{
			switch (destination)
			{
				case "Project":
					CurrentViewModel = _projectViewModel;
					break;
				case "Phase":
					CurrentViewModel = _phaseViewModel;
					break;
				case "Settings":
					CurrentViewModel = _settingsViewModel;
					break;
				default:
					break;
			}
		}
	}
}
