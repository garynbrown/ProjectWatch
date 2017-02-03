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
	[Export(typeof(EntitySetBase<Customer>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class CustomerSet : EntitySetBase<Project>, IEntitySet<Customer>
	{
		public CustomerSet()
		{
			IsDirty = false;
		}
		public override IEnumerable<Customer> GetEntitySet<Customer>()
		{
			return null;
		}

		public override string PathName => "Customer";

	}
}
