using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using ProjectWatch.Data.DTO;
using ProjectWatch.Entities;
using ProjectWatch.Support;

namespace ProjectWatch.ViewModel
{
	public struct TimeCardBlock
	{
		public int TimeCardId;
		public int TimeBlockId;
	}
	public class BillingEditViewModel : ViewModelCommon
	{
		#region Fields
		private Billing _billing;
		private BillingMainViewModel _billingMainViewModel;
		private string _billingForReview;
		private bool _billingChanged ;

		private List<TimeCardBlock> _timeBlockTacker;
		private StringBuilder sb = new StringBuilder();
		private DateTime _startTime = DateTime.Now;
		private DateTime _endTime = DateTime.MinValue;

		private List<TimeCard> _timeCards = new List<TimeCard>();
		#endregion

		#region Constructors
		public BillingEditViewModel(Billing billling) : this()
		{
			_billing = billling.Clone() as Billing;
		}

		public BillingEditViewModel(BillingDTO billingDto) : this()
		{
			_billing = billingDto.Bill;
		}

		public BillingEditViewModel()
		{
			SaveCommand = new RelayCommand(OnSave, CanSave);
			BillingChanged = false;
		}
		#endregion
		#region Commands
		public RelayCommand SaveCommand { get; set; }
		#endregion

		#region Methods


		private void FetchBill()
		{
			if (File.Exists(_billing.FileLocation))
			{
				BillingForReview = File.ReadAllText(_billing.FileLocation);
				BillingChanged = false;
			}
		}

		private void FillForm()
		{
			if (_billing.BillingId > -1 && !string.IsNullOrEmpty(_billing.FileLocation))
			{
				FetchBill();
			}
			else
			{
				MakeBill();
			}
			SaveCommand.RaiseCanExecuteChanged();
		}
		private bool CanSave()
		{
			return _billingChanged;
		}

		private void MakeBill()
		{

			sb = new StringBuilder();
			_timeCards = _billingMainViewModel.TimeCardRepo.GetRangeTimeCards(_billing.FromDate,_billing.ToDate).ToList();
			Project project = _billingMainViewModel.Projects.Find(p => p.ProjectId == _billing.ProjectId);
			Company company = _billingMainViewModel.Companies.Find(p => p.CompanyId == project.CompnayId);
			string companyName = company.CompanyName;
			TimeSpan _billHours = new TimeSpan();
			TimeSpan _projectHours = new TimeSpan();
			TimeSpan _phaseHours = new TimeSpan();
			double _billTotal = 0.0d;
			double _projectTotal = 0.0d;
			double _phaseTotal = 0.0d;
			double hours = 0.0d;
			DateTime _timeCardDate;
			List<Project> projects  = new List<Project>();
			sb.Append($"Invoice Date: {DateTime.Today:d}\r\n");
			int indx = _billingMainViewModel.Contacts.FindIndex(c => c.ContactId == project.BillingContactId);
			sb.Append($"\r\nInvoice for {companyName}\r\nFrom <User Name Here>\r\n");
			if (indx > -1)
				sb.Append($"Attention: {_billingMainViewModel.Contacts[indx].ToString()}\r\n");
			StringBuilder sbBill = new StringBuilder();
			if (_billing.ByCompany)
			{
				projects.AddRange(_billingMainViewModel.Projects.FindAll(p => p.CompnayId == company.CompanyId)); 
			}
			else
			{
				projects.Add(project);
			}
			foreach (Project _project in projects)
			{
				_projectHours = new TimeSpan();
				_projectTotal = 0.0d;
				sbBill.Append($"Project:  {_project.Name}\r\n");
				List<Phase> _phases = new List<Phase>();
				_phases = _billingMainViewModel.Phases.FindAll(p => p.ProjectId == _project.ProjectId);
				if (_phases.Any())
				{
					foreach (var _phase in _phases)
					{
						hours = 0.0d;
						if (_phase.IsBillable)
						{
							TimeSpan workhours = new TimeSpan();
							foreach (var _timeCard in _timeCards)
							{
								_timeCardDate = _timeCard.TimeCardDateTime();
								foreach (var _timeBlock in _timeCard.TimeBlocks)
								{
									if (!_timeBlock.IsBilled && _timeBlock.TimeBlockType == TimeType.Task && _timeBlock.PhaseId == _phase.PhaseId)
									{
										workhours = workhours.Add(_timeBlock.EndTime - _timeBlock.StartTime);
										TimeBlockTacker.Add(new TimeCardBlock() {TimeBlockId = _timeBlock.EntityId, TimeCardId = _timeCard.EntityId});
										if ( _timeCardDate < _startTime)
											_startTime = _timeCardDate;
										if (_timeCardDate > _endTime)
											_endTime = _timeCardDate;
									}
								}
							}
							hours += workhours.Hours + workhours.Minutes/60.0d;
							_projectHours = _projectHours.Add(workhours);
							_billHours = _billHours.Add(workhours);
							if (_billing.PhaseDetail)
							{
								double amount = hours*_phase.Rate;
								sbBill.Append($"\tPhase: {_phase.PhaseName} \tHours: {workhours.Hours:D0}:{workhours.Minutes:D2}  ({hours:N2})\t\t{amount:C}\r\n");
								_projectTotal += hours*_phase.Rate;
								_billTotal += hours*_phase.Rate;
							}
						}
					}
				}
				else
				{
					if (_project.IsBillable)
					{
						hours = 0.0d;
						TimeSpan workhours = new TimeSpan();
						foreach (var _timeCard in _timeCards)
						{
							_timeCardDate = _timeCard.TimeCardDateTime();
							foreach (var _timeBlock in _timeCard.TimeBlocks)
							{
								if (!_timeBlock.IsBilled && _timeBlock.TimeBlockType == TimeType.Task &&
									_timeBlock.ProjectId == _project.ProjectId)
								{
									workhours = workhours.Add(_timeBlock.EndTime - _timeBlock.StartTime);
									TimeBlockTacker.Add(new TimeCardBlock() { TimeBlockId = _timeBlock.EntityId, TimeCardId = _timeCard.EntityId });
									if (_timeCardDate < _startTime)
										_startTime = _timeCardDate;
									if (_timeCardDate > _endTime)
										_endTime = _timeCardDate;
								}
							}
						}
						hours += workhours.Hours + workhours.Minutes / 60.0d;
						_projectTotal += hours*_project.Rate;
						_billTotal += hours*_project.Rate;
						_projectHours = _projectHours.Add(workhours);
						_billHours = _billHours.Add(workhours);
					}
				}
				hours = _projectHours.Hours + _projectHours.Minutes/60.0d;
				sbBill.Append($"\r\nProject Total: Hours: {_projectHours.Hours:N0}:{_projectHours.Minutes:D2}  ({hours:N2})\t\t{_projectTotal:C}\r\n");
			}
			hours = _billHours.Hours + _billHours.Minutes/60.0d;
			sbBill.Append($"\r\n\r\nTotal\tHours: {_billHours.Hours:N0}:{_billHours.Minutes:D2} ({hours:N2})\t\t{_billTotal:C}\r\n");
			sb.Append($"\r\nFrom: {_startTime:d}\t\tTo: {_endTime:d}\r\n");
			sb.Append(sbBill);
			_billing.AmountBilled = _billTotal;
			_billing.Hours_Billed = hours;
			_billing.FromDate = _startTime;
			_billing.ToDate = _endTime;
			if (sbBill.Length > 0)
			{
				BillingForReview = sb.ToString();
				BillingChanged = true;
				SaveCommand.RaiseCanExecuteChanged();
			}
			else
			{
				BillingForReview = "Could not find any Billable Timcards";
				BillingChanged = false;
				SaveCommand.RaiseCanExecuteChanged();
			}


		}
		private void OnSave()
		{
			if (!BillingChanged)
				return;
			string filename = TimeCard.MakeTimeCardIdFromDate(DateTime.Now).ToString();
			filename += "_Invoice.txt";
			PreferenceSettings _preferenceSettings = new PreferenceSettings();
			PreferenceManager.LoadConfiguration(ref _preferenceSettings);  
			filename = !string.IsNullOrEmpty(_preferenceSettings.InvoicePath)? Path.Combine( _preferenceSettings.InvoicePath  , filename): filename;
			SaveFileDialog dialog = new SaveFileDialog() {Filter = "Text Files(*.txt)|*.txt|All(*.*)|*", FileName = filename};
			if (dialog.ShowDialog() == true)
			{
				string direct = Path.GetDirectoryName( dialog.FileName);

				File.WriteAllText(dialog.FileName, sb.ToString());
				if (_preferenceSettings.InvoicePath != direct)
				{
					_preferenceSettings.InvoicePath = direct;
					PreferenceManager.saveConfiguration(_preferenceSettings);
				}
				BillingChanged = false;
				_billing.FileLocation = dialog.FileName;
				_billing.DateBilled = DateTime.Now;

				Billing SavedBill = _billingMainViewModel.BillingRepo.Add(_billing);
				foreach (TimeCard _timeCard in _timeCards)
				{
					bool hasChanged = false;
					foreach (TimeBlock _block in _timeCard.TimeBlocks)
					{
						if (_timeBlockTacker.FindIndex(t => t.TimeBlockId == _block.EntityId && t.TimeCardId == _timeCard.EntityId) > -1)
						{
							_block.IsBilled = true;
							_block.BillingId = SavedBill.BillingId;
							hasChanged = true;
						}
					}
					if (hasChanged)
					{
						_billingMainViewModel.TimeCardRepo.Update(_timeCard);
					}
				}
				SaveCommand.RaiseCanExecuteChanged();
				
			}
		}
		#endregion

		#region Properties

		public bool BillingChanged
		{
			get { return _billingChanged; }
			set { Set(() => BillingChanged, ref _billingChanged, value, false); }
		}
		public List<TimeCardBlock> TimeBlockTacker
		{
			get { return _timeBlockTacker; }
			set { _timeBlockTacker = value; }
		}

		public string BillingForReview
		{
			get { return _billingForReview; }
			set
			{
				if (Set(() => BillingForReview, ref _billingForReview, value, false))
				{
					BillingChanged = true;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}
		#endregion

		#region Overrides
		protected override void OnViewLoaded()
		{
			_billingMainViewModel = ClientEntityBase.Container.GetExportedValue<BillingMainViewModel>();
			_timeBlockTacker = new List<TimeCardBlock>();
			FillForm();

		}
		#endregion
	}

}
