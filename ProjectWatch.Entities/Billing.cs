using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;
using Core.Common.Core;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Core.Common.Contracts;

namespace ProjectWatch.Entities
{
	[DataContract]
	public class Billing : ClientEntityBase, IIdentifiableEntity
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
			billing._billingId = billing_ID;
			billing.ProjectId = project_ID;
			return billing;
		}
		public Billing()
		{

		}
		public Billing(int billingId, int projectId)
		{
			_billingId = billingId;
		}
		#endregion

		#region Primitive Properties

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		//[EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
		[JsonProperty("BillingId")]
		[DataMember]
		public int BillingID
		{
			get
			{
				return _billingId;
			}
			set
			{
				if (_billingId != value)
				{
					_billingId = value;
					RaisePropertyChanged(() => BillingID);
				}
			}
		}
		private int _billingId;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		//[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
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
				RaisePropertyChanged(() => _projectId);
			}
		}
		private int _projectId;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		//[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMember]
		public int PhaseId
		{
			get
			{
				return _phaseId;
			}
			set
			{
				_phaseId = value;
				RaisePropertyChanged(() => _phaseId);
			}
		}
		private int _phaseId;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		//[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMember]
		public DateTime DateBilled
		{
			get
			{
				return _dateBilled;
			}
			set
			{
				_dateBilled = value;
				RaisePropertyChanged(() => _dateBilled);
			}
		}
		private DateTime _dateBilled;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		//[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMember]
		public double AmountBilled
		{
			get
			{
				return _amountBilled;
			}
			set
			{
				_amountBilled = value;
				RaisePropertyChanged(() => _amountBilled);
			}
		}
		private double _amountBilled;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public double Hours_Billed
		{
			get
			{
				return _hoursBilled;
			}
			set
			{
				_hoursBilled = value;
				RaisePropertyChanged(() => _hoursBilled);
			}
		}
		private double _hoursBilled;

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
				RaisePropertyChanged(() => _note);
			}
		}

		public int EntityId
		{
			get
			{
				return BillingID;
			}

			set
			{
				BillingID = value;
			}
		}

		private string _note;

		#endregion


	}
}
