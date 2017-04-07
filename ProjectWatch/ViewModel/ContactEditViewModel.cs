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
	public class ContactEditViewModel : ViewModelCommon
	{
		private string _note;
		private string _zip;
		private string _state;
		private string _city;
		private string _street2;
		private string _street1;
		private string _phone2;
		private string _phone1;
		private string _companyName;
		private string _email;
		private string _lastName;
		private string _firstName;
		Contact _contactUnderEdit = new Contact();
		private CompanyMainViewModel companyMainViewModel;

		public ContactEditViewModel()
		{
			
		}
		public ContactEditViewModel(Contact contact)
		{
			_contactUnderEdit = contact.Clone() as Contact;
			SaveCommand = new RelayCommand(OnSave, CanSave);
		}

		public ContactEditViewModel(Company company)  : this(new Contact( company.CompanyId,-1))
		{
		}

		public string FirstName
		{
			get
			{
				_firstName = string.IsNullOrEmpty(_firstName) ? _contactUnderEdit.FirstName : _firstName;
				return _firstName;
			}
			set
			{
				if (Set(() => FirstName, ref _firstName, value, false))
				{
					_contactUnderEdit.MakeDirty();
					_contactUnderEdit.FirstName = _firstName;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string LastName
		{
			get
			{
				_lastName = string.IsNullOrEmpty(_lastName) ? _contactUnderEdit.LastName : _lastName;
				return _lastName;
			}
			set
			{
				if (Set(() => LastName, ref _lastName, value, false))
				{
					_contactUnderEdit.MakeDirty();
					_contactUnderEdit.LastName = _lastName;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string Email
		{
			get
			{
				_email = string.IsNullOrEmpty(_email) ? _contactUnderEdit.Email : _email;
				return _email;
			}
			set
			{
				if (Set(() => Email, ref _email, value, false))
				{
					_contactUnderEdit.MakeDirty();
					_contactUnderEdit.Email = _email;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string CompanyName
		{
			get { return _companyName; }
			private set
			{
				_companyName = value;
			}
		}

		public string Phone1
		{
			get
			{
				_phone1 = string.IsNullOrEmpty(_phone1) ? _contactUnderEdit.Phone1 : _phone1;
				return _phone1;
			}
			set
			{
				if (Set(() => Phone1, ref _phone1, value, false))
				{
					_contactUnderEdit.MakeDirty();
					_contactUnderEdit.Phone1 = _phone1;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string Phone2
		{
			get
			{
				_phone2 = string.IsNullOrEmpty(_phone2) ? _contactUnderEdit.Phone2 : _phone2;
				return _phone2;
			}
			set
			{
				if (Set(() => Phone2, ref _phone2, value, false))
				{
					_contactUnderEdit.MakeDirty();
					_contactUnderEdit.Phone2 = _phone2;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string Street1
		{
			get
			{
				_street1 = string.IsNullOrEmpty(_street1) ? _contactUnderEdit.Street1 : _street1;
				return _street1;
			}
			set
			{
				if (Set(() => Street1, ref _street1, value, false))
				{
					_contactUnderEdit.MakeDirty();
					_contactUnderEdit.Street1 = _street1;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string Street2
		{
			get
			{
				_street2 = string.IsNullOrEmpty(_street2) ? _contactUnderEdit.Street2 : _street2;
				return _street2;
			}
			set
			{
				if (Set(() => Street2, ref _street2, value, false))
				{
					_contactUnderEdit.MakeDirty();
					_contactUnderEdit.Street2 = _street2;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string City
		{
			get
			{
				_city = string.IsNullOrEmpty(_city) ? _contactUnderEdit.City : _city;
				return _city;
			}
			set
			{
				if (Set(() => City, ref _city, value, false))
				{
					_contactUnderEdit.MakeDirty();
					_contactUnderEdit.City = _city;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string State
		{
			get
			{
				_state = string.IsNullOrEmpty(_state) ? _contactUnderEdit.State : _state;
				return _state;
			}
			set
			{
				if (Set(() => State, ref _state, value, false))
				{
					_contactUnderEdit.MakeDirty();
					_contactUnderEdit.State = _state;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string Zip
		{
			get
			{
				_zip = string.IsNullOrEmpty(_zip) ? _contactUnderEdit.ZipCode : _zip;
				return _zip;
			}
			set
			{
				if (Set(() => Zip, ref _zip, value, false))
				{
					_contactUnderEdit.MakeDirty();
					_contactUnderEdit.ZipCode = _zip;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string Note
		{
			get
			{
				_note = string.IsNullOrEmpty(_note) ? _contactUnderEdit.Note : _note;
				return _note;
			}
			set
			{
				if (Set(() => Note, ref _note, value, false))
				{
					_contactUnderEdit.MakeDirty();
					_contactUnderEdit.Note = _note;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		bool CanSave()
		{
			return _contactUnderEdit.IsDirty  && (!string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName));
		}
		public RelayCommand SaveCommand { get; set; }

		void OnSave()
		{
			if (_contactUnderEdit.ContactId == -1)
			{
				companyMainViewModel.contactRepository.Add(_contactUnderEdit);
			}
			else
			{
				companyMainViewModel.contactRepository.Update(_contactUnderEdit);
			}
			companyMainViewModel.CompanyMainEmployees =companyMainViewModel.contactRepository.Get().EntitySet.ToList();
			_contactUnderEdit.CleanAll();
		}

		protected override void OnViewLoaded()
		{
			companyMainViewModel = ClientEntityBase.Container.GetExportedValue<CompanyMainViewModel>();
		}
	}
}
