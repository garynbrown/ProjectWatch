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
	[Export(typeof(EntitySetBase<Phase>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class PhaseSet : EntitySetBase<Phase>, IEntitySet<Phase>
	{
		public PhaseSet()
		{
			IsDirty = false;
		}
		public override IEnumerable<Project> GetEntitySet<Project>()
		{
			return null;
		}
		public override string PathName => "Phase";
	}
}
