using System.Linq;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.CommandWpf;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	public class CompanyEditViewModel : ViewModelCommon
	{
		#region Fields
		Company _companyUnderEdit = new Company();
		private string _companyName;
		private CompanyMainViewModel companyMainViewModel;
		private string _note;
		#endregion

		#region Constructors
		public CompanyEditViewModel() : this(new Company(-1))
		{
			
		}
		public CompanyEditViewModel(Company company)
		{
			_companyUnderEdit = company.Clone() as Company;
			SaveCommand  = new RelayCommand(OnSave, CanSave);
		}
		#endregion

		#region Properties
		public string CompanyName
		{
			get
			{
				_companyName = string.IsNullOrEmpty(_companyName) ? _companyUnderEdit.CompanyName : _companyName;
				return _companyName;
			}
			set
			{
				if (Set(() => CompanyName, ref _companyName, value, false))
				{
					_companyUnderEdit.MakeDirty();
					_companyUnderEdit.CompanyName = _companyName;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string Note
		{
			get { return _note; }
			set
			{
				if (Set(() => Note, ref _note, value, false))
				{
					_companyUnderEdit.MakeDirty();
					_companyUnderEdit.Note = _note;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}
		#endregion

		#region Methods
		bool CanSave()
		{
			return _companyUnderEdit.IsDirty && !string.IsNullOrEmpty(_companyName);
		}

		void OnSave()
		{
			if (_companyUnderEdit.CompanyId == -1)
			{
				companyMainViewModel.companyRepository.Add(_companyUnderEdit);
			}
			else
			{

				companyMainViewModel.companyRepository.Update(_companyUnderEdit);
			}
			_companyUnderEdit.CleanAll();
			companyMainViewModel.CompanyMainCompanies = companyMainViewModel.companyRepository.Get().EntitySet.ToList();
		}
		#endregion
		#region Commands
		public RelayCommand SaveCommand { get; set; }
		#endregion

		#region Overrides
		protected override void OnViewLoaded()
		{
			companyMainViewModel = ClientEntityBase.Container.GetExportedValue<CompanyMainViewModel>();
		}
		#endregion
	}
}
