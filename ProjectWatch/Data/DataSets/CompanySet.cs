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
	[Export(typeof(EntitySetBase<Company>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class CompanySet : EntitySetBase<Company>, IEntitySet<Company>
	{
		public CompanySet()
		{
			IsDirty = false;
		}
		public override IEnumerable<Company> GetEntitySet<Company>()
		{
			return null;
		}
		public override string PathName => "Company";
	}
}
