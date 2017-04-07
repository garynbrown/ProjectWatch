using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Core;
using Core.Common.UI;
using GalaSoft.MvvmLight.Command;
using ProjectWatch.Entities;

namespace ProjectWatch.ViewModel
{
	public class PhaseEditViewModel : ViewModelCommon
	{
		private Phase _phaseUnderEdit;
		private DashboardViewModel dashboardViewModel = null;
		private string _dueDate;
		private string _hourQuote;
		private string _note;
		private int _phaseId;
		private string _phaseName;
		private string _projectName;
		private string _rate;
		private string _timeQuote;
		private bool _billable;
		private Contact _billingContact;
		private Contact _managementContact;

		private PhaseEditViewModel()
		{
		}

		public PhaseEditViewModel(Project project) 
			: this(new Phase(project.ProjectId,-1))
		{		
		}
		public Phase PhaseUnderEdit
		{
			get { return _phaseUnderEdit; }
			set { _phaseUnderEdit = value; }
		}


		public PhaseEditViewModel( Phase phase)
		{
			_phaseUnderEdit = phase.Clone() as Phase;
			_billable = _phaseUnderEdit.IsBillable;
			dashboardViewModel = ClientEntityBase.Container.GetExportedValue<DashboardViewModel>();
			SaveCommand = new RelayCommand(OnSave, CanSave);
		}


		bool CanSave()
		{
			return _phaseUnderEdit.IsDirty && !string.IsNullOrEmpty(_phaseName);
		}
		public RelayCommand SaveCommand { get; set; }

		void OnSave()
		{
			if (PhaseUnderEdit.PhaseId == -1)
			{
				dashboardViewModel.phaseRepository.Add(PhaseUnderEdit);
			}
			else
			{
				dashboardViewModel.phaseRepository.Update(PhaseUnderEdit);
			}
			int indx = dashboardViewModel.Projects.FindIndex(p => p.ProjectId == _phaseUnderEdit.ProjectId);
			if (indx > -1)
			{
				Project projectToEdit = indx > -1 ? dashboardViewModel.Projects[indx] : null;
				if (projectToEdit != null)
				{
					projectToEdit.HasChild = true;
				}
				dashboardViewModel.projectRepository.Update(projectToEdit);
			}
			_phaseUnderEdit.CleanAll();
			SaveCommand.RaiseCanExecuteChanged();
			dashboardViewModel.AllPhases = dashboardViewModel.phaseRepository.Get().EntitySet.ToList();
		}

		public bool Billable
		{
			get
			{
				return _billable;
			}
			set
			{
				if (Set(() => Billable, ref _billable, value, false))
				{
					PhaseUnderEdit.MakeDirty();
					PhaseUnderEdit.IsBillable = _billable;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string DueDate
		{
			get
			{
				_dueDate =  !string.IsNullOrEmpty(_dueDate) ? _dueDate :
					(PhaseUnderEdit.DueDate == DateTime.MinValue ? "" : PhaseUnderEdit.DueDate.ToShortDateString());
				return _dueDate;
			}
			set
			{
				if (Set(() => DueDate, ref _dueDate, value, false))
				{
					PhaseUnderEdit.MakeDirty();
					DateTime tempDate;
					if (DateTime.TryParse(_dueDate, out tempDate))
					{
						PhaseUnderEdit.DueDate = tempDate;
					}
					else
					{
						DueDate = "";
					}
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string HourQuote
		{
			get
			{
				_hourQuote =  !string.IsNullOrEmpty(_hourQuote) ? _hourQuote : PhaseUnderEdit.HourQuote.ToString();
				return _hourQuote;
			}
			set
			{
				if (Set(() => HourQuote, ref _hourQuote, value, false))
				{
					PhaseUnderEdit.MakeDirty();
					double tempd = -1.0d;
					if (double.TryParse(_hourQuote, out tempd))
					{
						PhaseUnderEdit.HourQuote = tempd;
					}
					else
					{
						HourQuote = "0.0";
					}
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string Note
		{
			get
			{
				_note =  !string.IsNullOrEmpty(_note) ? _note : PhaseUnderEdit.Note;
				return _note;
			}
			set
			{
				if (Set(() => Note, ref _note, value, false))
				{
					PhaseUnderEdit.MakeDirty();
					PhaseUnderEdit.Note = _note;
				}
				SaveCommand.RaiseCanExecuteChanged();
			}
		}

		public int PhaseId
		{
			get { return _phaseId; }
			set { _phaseId = value; }
		}

		public string PhaseName
		{
			get
			{
				_phaseName =  !string.IsNullOrEmpty(_phaseName) ? _phaseName : PhaseUnderEdit.PhaseName;
				return _phaseName;
			}
			set
			{
				if (Set(() => PhaseName, ref _phaseName, value, false))
				{
					PhaseUnderEdit.MakeDirty();
					PhaseUnderEdit.PhaseName = _phaseName;
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string ProjectName
		{
			get
			{
				if (string.IsNullOrEmpty(_projectName))
				{
					List<Project> temProjects = dashboardViewModel.Projects.ToList<Project>();
					_projectName = temProjects.Find(p => p.ProjectId == _phaseUnderEdit.ProjectId).Name;
				}
				return _projectName;
			}
			private set { _projectName = value; }
		}

		public string Rate
		{
			get
			{
				_rate =  string.IsNullOrEmpty(_rate) ? PhaseUnderEdit.Rate.ToString() : _rate;
				return _rate;
			}
			set
			{
				if (Set(() => Rate, ref _rate, value, false))
				{
					double tempd = -1.0d;
					if (double.TryParse(_rate, out tempd))
					{
						PhaseUnderEdit.Rate = tempd;
					}
					else
					{
						Rate = "0.0";
					}
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public string TimeQuote
		{
			get
			{
				_timeQuote =  !string.IsNullOrEmpty(_timeQuote) ? _timeQuote : PhaseUnderEdit.TimeQuote.ToString();
				return _timeQuote;
			}
			set
			{
				if (Set(() => TimeQuote, ref _timeQuote, value, false))
				{
					double tempd = -1.0d;
					if (double.TryParse(_timeQuote, out tempd))
					{
						PhaseUnderEdit.TimeQuote = tempd;
					}
					else
					{
						TimeQuote = "0.0";
					}
					SaveCommand.RaiseCanExecuteChanged();
				}

			}
		}

		public Contact BillingContact
		{
			get { return _billingContact; }
			set { _billingContact = value; }
		}

		public Contact ManagementContact
		{
			get { return _managementContact; }
			set { _managementContact = value; }
		}

		protected override void OnViewLoaded()
		{
			base.OnViewLoaded();
		}
	}
}
