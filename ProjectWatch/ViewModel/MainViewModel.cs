using System.Collections.Generic;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using Core.Common.Core;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.Shared)]
    public class MainViewModel : ViewModelCommon
	{
		#region Fields
		private DashboardViewModel _dashboardVm;
		#endregion

		#region Properties

		[Import]
		public BillingMainViewModel BillingMainViewModel
		{
			get { return ClientEntityBase.Container.GetExportedValue<BillingMainViewModel>(); }
		}
		[Import]
		public CompanyMainViewModel CompanyMainViewModel 
		{
			get { return ClientEntityBase.Container.GetExportedValue<CompanyMainViewModel>(); }
		}

		public DashboardViewModel DashboardVM
		{
			get { return _dashboardVm; }
			set { _dashboardVm = value; }
		}
		[Import]
		public ProjectMainViewModel ProjectMainViewModel
		{
			get { return ClientEntityBase.Container.GetExportedValue<ProjectMainViewModel>(); }
		}

		[Import]
		public TimeCardMainViewModel TimeCardMainViewModel
		{
			get { return ClientEntityBase.Container.GetExportedValue<TimeCardMainViewModel>(); }
		}
		#endregion

		#region Constructors
		/// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
			ClosingCommand = new RelayCommand<RoutedEventArgs>(OnClosing);
        }
		#endregion

		#region Commands
		public RelayCommand<RoutedEventArgs> ClosingCommand { get; set; }
		#endregion

		#region Methods
		void OnClosing(RoutedEventArgs args)
		{
			_dashboardVm.IsPropertiesShowing = false;
		}
		#endregion
		#region Overrides
		protected override void OnViewLoaded()
		{
			_dashboardVm = ClientEntityBase.Container.GetExportedValue<DashboardViewModel>();
		}
		#endregion
	}
}