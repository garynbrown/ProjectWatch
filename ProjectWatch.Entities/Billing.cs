using System;
using System.Runtime.Serialization;
using Core.Common.Core;
using Newtonsoft.Json;
using Core.Common.Contracts;

namespace ProjectWatch.Entities
{
	[DataContract]
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class Billing : ClientEntityBase, IIdentifiableEntity, ICloneable
	{
		#region Factory Method

		/// <summary>
		/// Create a new Billing object.
		/// </summary>
		/// <param name="billing_ID">Initial value of the Billing_ID property.</param>
		/// <param name="project_ID">Initial value of the Project_ID property.</param>
		public static Billing CreateBilling(int billing_ID, int project_ID)
		{
			Billing billing = new Billing();
			billing.BillingId = billing_ID;
			billing.ProjectId = project_ID;
			return billing;
		}
		public Billing()
		{
			BillingId = -1;
		}
		public Billing(int billingId, int projectId)
		{
			BillingId = billingId;
		}
		#endregion

		#region Primitive Properties

		[DataMember]
		public int BillingId { get; set; }

		[DataMember]
		public int ProjectId { get; set; }

		[DataMember]
		public int PhaseId { get; set; }

		[DataMember]
		public DateTime DateBilled { get; set; }

		[DataMember]
		public double AmountBilled { get; set; }

		[DataMember]
		public double Hours_Billed { get; set; }

		[DataMember]
		public string FileLocation { get; set; }
		[DataMember]
		public bool PhaseDetail { get; set; }
		[DataMember]
		public bool ByCompany { get; set; }
		[DataMember]
		public DateTime FromDate { get; set; }

		[DataMember]
		public DateTime ToDate { get; set; }
		[DataMember]
		public string Note { get; set; }

		[JsonIgnore]
		public override int EntityId
		{
			get
			{
				return BillingId;
			}

			set
			{
				BillingId = value;
			}
		}

		[JsonIgnore]
		public string PathName  => "Billing";

		#endregion

		#region Contract_Implementations
		public object Clone()
		{
			Billing b = new Billing();

			b.AmountBilled = AmountBilled;
			b.PhaseDetail = PhaseDetail;
			b.ByCompany = ByCompany;
			b.FileLocation = FileLocation;
			b.BillingId = BillingId;
			b.DateBilled = DateBilled;
			b.Hours_Billed = Hours_Billed;
			b.FromDate = FromDate;
			b.ToDate = ToDate;
			b.Note = Note;
			b.PhaseId = PhaseId;
			b.ProjectId = ProjectId;

			return b;
		}
		#endregion
	}
}
