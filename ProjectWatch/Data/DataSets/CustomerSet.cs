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
	[Export(typeof(EntitySetBase<Customer>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class CustomerSet : EntitySetBase<Customer>, IEntitySet<Customer>
	{
		#region Constructors
		public CustomerSet()
		{
			IsDirty = false;
		}
		#endregion
		#region Overrides
		public override IEnumerable<Customer> GetEntitySet<Customer>()
		{
			return null;
		}

		[JsonIgnore]
		public override string PathName => "Customer";
		#endregion

	}
}
