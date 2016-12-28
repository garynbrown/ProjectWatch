using GalaSoft.MvvmLight;
using Core.Common.Core;
using System.ComponentModel.Composition;
using Core.Common.UI.Core;

namespace ProjectWatch.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
    public class MainViewModel : ViewModelCommon
	{
		[Import]
		public DashboardViewModel DashboardViewModel { get; private set ; }
		[Import]
		public AdministationViewModel AdministationViewModel
		{
			get { return ClientEntityBase.Container.GetExportedValue<AdministationViewModel>(); }
		}

		[Import]
		public SettingsViewModel SettingsViewModel
		{
			get { return ClientEntityBase.Container.GetExportedValue<SettingsViewModel>(); }
		}
		[Import]
		public ProjectViewModel ProjectViewModel
		{
			get { return ClientEntityBase.Container.GetExportedValue<ProjectViewModel>(); }
		}
		[Import]
		public PhaseViewModel PhaseViewModel
		{
			get { return ClientEntityBase.Container.GetExportedValue<PhaseViewModel>(); }
		}


		[Import]
		public ReportsViewModel ReportsViewModel { get; private set; }
		[Import]
		public TimecardViewModel TimecardViewModel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }
    }
}