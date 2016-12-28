using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWatch
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class MainWindowViewModel
	{

		#region FromMVVMLightCourse
#if Example

		// 
//  found at C:\VCPP\Plurasight\MVVMLight\3-mvvm-light-m3-core-exercise-files\after\viewmodelbase\WhyMvvm\ViewModel
		private Friend _selectedFriend;

		public Friend SelectedFriend
		{
			get
			{
				return _selectedFriend;
			}

			set
			{
				Set(() => SelectedFriend, ref _selectedFriend, value);  // The "Set" is part of the ViewModelBase It takes care of the guard condition and raise property change
			}
		}
		//  found at C:\VCPP\Plurasight\MVVMLight\2-mvvm-light-m2-refactoring-exercise-files\after\WhyMvvm 
		private RelayCommand _refreshCommand;

		public RelayCommand RefreshCommand
		{
			get
			{
				return _refreshCommand
					?? (_refreshCommand = new RelayCommand(
										  async () =>
										  {
											  await Refresh();
										  }));
			}
		}

		private async Task Refresh()
		{
			Friends.Clear();

			var friends = await _dataService.Refresh();

			foreach (var friend in friends)
			{
				Friends.Add(friend);
			}
		}
		private RelayCommand<Friend> _saveCommand;

		public RelayCommand<Friend> SaveCommand
		{
			get
			{
				return _saveCommand
					?? (_saveCommand = new RelayCommand<Friend>(
										  async friend =>
										  {
											  try
											  {
												  var service = _dataService;  // injected
												  var result = await service.Save(friend);

												  var id = int.Parse(result);

												  if (id > 0)
												  {
													  friend.Id = id;
												  }
												  else
												  {
													  _dialogService.ShowMessage("Error");
												  }
											  }
											  catch (Exception ex)
											  {
												  _dialogService.ShowMessage(ex.Message);
											  }
										  }));
			}
		}
#endif
#endregion

	}
}
