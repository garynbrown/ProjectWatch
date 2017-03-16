using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Data;
using Newtonsoft.Json;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DataSets
{
	[Export(typeof(EntitySetBase<Billing>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class BillingSet : EntitySetBase<Billing>, IEntitySet<Billing>
	{
		#region Constructors
		public BillingSet()
		{
			IsDirty = false;
			EntitySet = new List<Billing>();
		}
		#endregion
		#region Overrides
		public override IEnumerable<T> GetEntitySet<T>()
		{
			return null;
		}

		[JsonIgnore]
		public override string PathName => "Billing";
		#endregion
	}
}
