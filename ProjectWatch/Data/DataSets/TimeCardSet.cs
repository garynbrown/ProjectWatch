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
	[Export(typeof(EntitySetBase<TimeCard>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class TimeCardSet : EntitySetBase<TimeCard>, IEntitySet<TimeCard>
	{
		#region Constructors
		public TimeCardSet()
		{
			IsDirty = false;
		}
		#endregion
		#region Contract_Implementations
		public override IEnumerable<Project> GetEntitySet<Project>()
		{
			return null;
		}
		public override string PathName => "Timecard";
		#endregion
	}
}
