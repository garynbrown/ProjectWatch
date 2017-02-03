using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Data;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DataSets
{
	[Export(typeof(EntitySetBase<Billing>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class BillingSet : EntitySetBase<Billing>, IEntitySet<Billing>
	{
		public BillingSet()
		{
			IsDirty = false;
		}
		public override IEnumerable<T> GetEntitySet<T>()
		{
			return null;
		}

		public override string PathName => "Billing";
	}
}
