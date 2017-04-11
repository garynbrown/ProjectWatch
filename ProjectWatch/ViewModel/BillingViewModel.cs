using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Data.DTO;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	public class BillingViewModel : ViewModelCommon
	{
		#region Constructors
		public BillingViewModel()
		{
			DeleteBillingCommand = new RelayCommand<Billing>(OnDeleteBilling);
			CreateBillingCommand = new RelayCommand(OnCreateBilling,CanCreateBilling);
			_billingRepository = ClientEntityBase.Container.GetExportedValue<IBillingRepository>();
		}
		#endregion

		#region Fields
		private readonly IBillingRepository _billingRepository;
		private List<BillingDTO> _displayBillings;
		private List<Company> _companies;
		private List<Project> _projects;
		private BillingMainViewModel _billingMainViewModel;
		private bool _fromLastBilling;
		private bool _customDateRange;
		private bool _phaseDetail;
		private bool _byCompany;
		private bool _byProject;
		private string _fromDate;
		private string _toDate;
		private Project _selectedProject;
		private Company _selectedCompany;
		#endregion
		#region Commands

		public RelayCommand CreateBillingCommand { get; set; }
		public RelayCommand<Billing> DeleteBillingCommand { get; set; }
		#endregion

		#region Methods

		bool CanCreateBilling()
		{
			bool canExecute = ((IsValidCustomDaterange() || FromLastBilling) &&
								((SelectedCompany != null && ByCompany) ||
								(SelectedProject != null && ByProject)));
			return canExecute;
		}

		bool IsValidCustomDaterange()
		{
			bool _isVaildDateRange = false;
			if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
			{
				DateTime toTemp;
				DateTime fromTemp;
				DateTime.TryParse(ToDate, out toTemp);
				DateTime.TryParse(FromDate, out fromTemp);
				_isVaildDateRange = toTemp > fromTemp;
			}
			return _customDateRange && _isVaildDateRange;
		}

		void MakeDisplsyDtos(List<Billing> billings)
		{
			List<BillingDTO> billDtos = new List<BillingDTO>();
			List<Billing> sortedList = billings.OrderByDescending(b =>b.DateBilled ).ToList();
			foreach (Billing _billing in sortedList)
			{
				billDtos.Add( new BillingDTO(_billing,Companies,Projects, Phases));
			}
			DisplayBillings =billDtos;
		}

		void OnCreateBilling()
		{
			Billing _billing = new Billing();
			_billing.PhaseDetail = _phaseDetail;
			_billing.ByCompany = _byCompany;
			DateTime tempDate;
			if (_customDateRange)
			{
				_billing.ToDate = (DateTime.TryParse(ToDate, out tempDate)) ? tempDate : DateTime.MinValue;
				_billing.FromDate = (DateTime.TryParse(FromDate, out tempDate)) ? tempDate : DateTime.MinValue;
			}
			else
			{
				List<Billing> TempBilling = new List<Billing>();
				if (_byCompany)
				{
					List<Project> tempProjects = _projects.FindAll(p => p.CompnayId == _selectedCompany.CompanyId);
					_billing.ProjectId = tempProjects.Any() ? tempProjects.First().ProjectId : -1;
					foreach (Project _tempProject in tempProjects)
					{
						TempBilling.AddRange( Billings.FindAll(v => v.ProjectId == _tempProject.ProjectId));
					}
				}
				else
				{
					TempBilling = Billings.FindAll(v => v.ProjectId == SelectedProject.ProjectId);
					_billing.ProjectId = SelectedProject.ProjectId;
				}
				TempBilling = TempBilling.OrderByDescending(v => v.ToDate).ToList();
				_billing.FromDate = (TempBilling.Any()) ? TempBilling.First().ToDate.AddMinutes(10) : SelectedProject.StartDate;
				_billing.ToDate = DateTime.Now;
			}
			_billingMainViewModel.OnAddBilling(_billing);
		}
		void OnDeleteBilling(Billing billing)
		{
			List<TimeCard> timeCards = _billingMainViewModel.TimeCardRepo.GetRangeTimeCards(billing.FromDate.AddDays(-1),
				billing.ToDate.AddDays(1)).ToList();
			bool changed = false;
			foreach (TimeCard _timeCard in timeCards)
			{
				changed = false;
				foreach (TimeBlock _block in _timeCard.TimeBlocks)
				{
					if (_block.BillingId == billing.BillingId)
					{
						_block.IsBilled = false;
						_block.BillingId = -1;
						changed = true;
					}
				}
				if (changed)
				{
					_billingMainViewModel.TimeCardRepo.Update(_timeCard);
				}
			}
			// todo reset project information
			if (!string.IsNullOrEmpty(billing.FileLocation) && File.Exists(billing.FileLocation))
			{
				File.Delete(billing.FileLocation);
			}
			_billingRepository.Remove(billing);
		}
		#endregion

		#region Properties

		public List<Billing> Billings { get; set; }

		public bool ByCompany
		{
			get { return _byCompany; }
			set
			{
				if (Set(() => ByCompany, ref _byCompany, value, false))
				{
					
					CreateBillingCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public bool ByProject
		{
			get { return _byProject; }
			set
			{
				if (Set(() => ByProject, ref _byProject, value, false))
				{
					CreateBillingCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public List<Company> Companies
		{
			get { return _companies; }
			set { Set(() => Companies, ref _companies, value, false); }
		}

		public bool CustomDateRange
		{
			get { return _customDateRange; }
			set
			{
				if (Set(() => CustomDateRange, ref _customDateRange, value, false))
				{
					CreateBillingCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public List<BillingDTO> DisplayBillings
		{
			get { return _displayBillings; }
			set { Set(() => DisplayBillings, ref _displayBillings, value, false); }
		}

		public string FromDate
		{
			get { return _fromDate; }
			set
			{
				if (Set(() => FromDate, ref _fromDate, value, false))
				{
					DateTime tempTime;
					if (!DateTime.TryParse(_fromDate, out tempTime))
					{
						FromDate = "";
					}
					CreateBillingCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public bool FromLastBilling
		{
			get { return _fromLastBilling; }
			set
			{
				if (Set(() => FromLastBilling, ref _fromLastBilling, value, false) )
				{
					CreateBillingCommand.RaiseCanExecuteChanged();
				}
			}
		}
		public bool PhaseDetail
		{
			get { return _phaseDetail; }
			set { Set(() => PhaseDetail, ref _phaseDetail, value, false); }
		}

		public List<Phase> Phases { get; set; }

		public List<Project> Projects
		{
			get { return _projects; }
			set {Set(() => Projects, ref _projects, value, false); }
		}

		public Company SelectedCompany
		{
			get { return _selectedCompany; }
			set
			{
				if (Set(() => SelectedCompany, ref _selectedCompany, value, false))
				{
					CreateBillingCommand.RaiseCanExecuteChanged();
				}
				
			}
		}
		public Project SelectedProject
		{
			get { return _selectedProject; }
			set
			{
				if (Set(() => SelectedProject, ref _selectedProject, value, false))
				{
					CreateBillingCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string ToDate
		{
			get { return _toDate; }
			set
			{
				if (Set(() => ToDate, ref _toDate, value, false))
				{
					DateTime tempTime;
					if (!DateTime.TryParse(_toDate, out tempTime))
					{
						ToDate = "";
					}
					CreateBillingCommand.RaiseCanExecuteChanged();
				}
			}
		}
		#endregion

		#region Overrides
		protected override void OnViewLoaded()
		{
			_billingMainViewModel = ClientEntityBase.Container.GetExportedValue<BillingMainViewModel>();
			Companies = _billingMainViewModel.Companies;
			Projects = _billingMainViewModel.Projects;
			Phases = _billingMainViewModel.Phases;
			Billings = _billingMainViewModel.BillingRepo.Get().EntitySet.ToList();
			MakeDisplsyDtos(Billings);
			_fromLastBilling = true;
			_phaseDetail = false;
			_byProject = true;
		}
		#endregion
	}
}
