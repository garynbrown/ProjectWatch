using System.ComponentModel.Composition;
using Core.Common.UI;

namespace ProjectWatch.ViewModel
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class AdministationViewModel : ViewModelCommon
	{

		private ViewModelCommon _currentViewModel;

		public ViewModelCommon CurrentViewModel
		{
			get{return _currentViewModel;}
			set { Set(() => CurrentViewModel, ref _currentViewModel, value); }
		}

		public override string ViewTitle
		{
			get { return "Administration Tools"; }
		}

	}
}
