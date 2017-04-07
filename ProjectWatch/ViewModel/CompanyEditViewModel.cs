using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.CommandWpf;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	public class CompanyEditViewModel : ViewModelCommon
	{
		Company _companyUnderEdit = new Company();
		private string _companyName;
		private CompanyMainViewModel companyMainViewModel;
		private string _note;

		public CompanyEditViewModel() : this(new Company(-1))
		{
			
		}
		public CompanyEditViewModel(Company company)
		{
			_companyUnderEdit = company.Clone() as Company;
			SaveCommand  = new RelayCommand(OnSave, CanSave);
		}

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

		bool CanSave()
		{
			return _companyUnderEdit.IsDirty && !string.IsNullOrEmpty(_companyName);
		}
		public RelayCommand SaveCommand { get; set; }

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

		//private MainViewModel _mainViewModel;
		protected override void OnViewLoaded()
		{
			companyMainViewModel = ClientEntityBase.Container.GetExportedValue<CompanyMainViewModel>();
		}
	}
}
