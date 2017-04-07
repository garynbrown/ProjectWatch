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
			ProjectId = projectId;
			PhaseId = phaseId;
		}
		#endregion

		#region Primitive Properties

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public int PhaseId { get; set; }

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public int ProjectId { get; set; }

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public string PhaseName { get; set; }

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public bool IsBillable { get; set; }

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public DateTime DueDate { get; set; }

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public double HourQuote { get; set; }

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public double TimeQuote { get; set; }

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public double Rate { get; set; }

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public string Note { get; set; }

		public bool HasChild { get; set; }
		public bool HasParent { get; set; }

		//[DataMember]
		//public int BillingContactId { get; set; }

		//[DataMember]
		//public int ManagementContactId { get; set; }

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
			p.IsBillable = IsBillable;
			p.HasChild = HasChild;
			p.HasParent = HasParent;
			p.DueDate = DueDate;
			p.HourQuote = HourQuote;
			p.Note = Note;
			p.PhaseId = PhaseId;
			p.PhaseName = PhaseName;
			p.ProjectId = ProjectId;
			p.Rate = Rate;
			p.TimeQuote = TimeQuote;
			//p.BillingContactId = BillingContactId;
			//p.ManagementContactId = ManagementContactId;

			return p;
		}

		public override string ToString()
		{
			return PhaseName;
		}
	}
}
