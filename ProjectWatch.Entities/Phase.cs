using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;
using Newtonsoft.Json;

namespace ProjectWatch.Entities
{
//	OnPropertyChanged(() => _billingId);
	[DataContract]
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class Phase : ClientEntityBase, IIdentifiableEntity, ICloneable
	{
		#region Factory Method

		/// <summary>
		/// Create a new Phase object.
		/// </summary>
		/// <param name="phase_ID">Initial value of the Phase_ID property.</param>
		/// <param name="project_ID">Initial value of the Project_ID property.</param>
		/// <param name="phaseName">Initial value of the PhaseName property.</param>
		/// <param name="billable">Initial value of the Billable property.</param>
		//public static Phase CreatePhase(int phaseId, int projectId, string phaseName, bool billable)
		//{
		//	Phase phase = new Phase( phaseId,  projectId,  phaseName,  billable);
		//	return phase;
		//}

		//public Phase(int phaseId, int projectId, string phaseName, bool billable)
		//{
		//	_phaseId = phaseId;
		//	_projectId = projectId;
		//	_phaseName = phaseName;
		//	Billable = billable;
		//}

		public Phase()
		{
			
		}

		public Phase(int projectId) 
			:this(projectId,-1)
		{
		}

		public Phase(int projectId, int phaseId)
		{
			_projectId = projectId;
			_phaseId = phaseId;
		}
		#endregion

		#region Primitive Properties

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public int PhaseId
		{
			get
			{
				return _phaseId;
			}
			set
			{
				if (_phaseId != value)
				{
					_phaseId =(value);
					RaisePropertyChanged(() => PhaseId);
				}
			}
		}
		private int _phaseId;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public int ProjectId
		{
			get
			{
				return _projectId;
			}
			set
			{
				_projectId = value;
				RaisePropertyChanged(() => ProjectId);
			}
		}
		private int _projectId;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public string PhaseName
		{
			get
			{
				return _phaseName;
			}
			set
			{
				_phaseName =value;
				RaisePropertyChanged(() => PhaseName);
			}
		}
		private string _phaseName;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public bool Billable
		{
			get
			{
				return _billable;
			}
			set
			{
				_billable = value;
				RaisePropertyChanged(() => Billable);
			}
		}
		private bool _billable;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public DateTime DueDate
		{
			get
			{
				return _dueDate;
			}
			set
			{
				_dueDate = value;
				RaisePropertyChanged(() => DueDate);
			}
		}
		private DateTime _dueDate;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public double HourQuote
		{
			get
			{
				return _hourQuote;
			}
			set
			{
				_hourQuote = value;
				RaisePropertyChanged(() => HourQuote);
			}
		}
		private double _hourQuote;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public double TimeQuote
		{
			get
			{
				return _timeQuote;
			}
			set
			{
				_timeQuote = value;
				RaisePropertyChanged(() => TimeQuote);
			}
		}
		private double _timeQuote;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public double Rate
		{
			get
			{
				return _rate;
			}
			set
			{
				_rate = value;
				RaisePropertyChanged(() => Rate);
			}
		}
		private double _rate;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public string Note
		{
			get
			{
				return _note;
			}
			set
			{
				_note = value;
				RaisePropertyChanged(() => Note);
			}
		}
		[DataMember]
		public int BillingContactId { get; set; }

		[DataMember]
		public int ManagementContactId { get; set; }
		private string _note;

		#endregion

		#region Contract_Implementations
		[JsonIgnore]
		public override int EntityId
		{
			get { return PhaseId; }
			set { PhaseId = value; }
		}
		#endregion

		public object Clone()
		{
			Phase p = new Phase();
			p.Billable = _billable;
			p.DueDate = _dueDate;
			p.HourQuote = _hourQuote;
			p.Note = _note;
			p.PhaseId = _phaseId;
			p.PhaseName = _phaseName;
			p.ProjectId = _projectId;
			p.Rate = _rate;
			p.TimeQuote = _timeQuote;
			p.BillingContactId = BillingContactId;
			p.ManagementContactId = ManagementContactId;

			return p;
		}

		public override string ToString()
		{
			return _phaseName;
		}
	}
}
