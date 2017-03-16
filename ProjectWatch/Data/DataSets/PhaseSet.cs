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
	[Export(typeof(EntitySetBase<Phase>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class PhaseSet : EntitySetBase<Phase>, IEntitySet<Phase>
	{
		#region Constructors
		public PhaseSet()
		{
			IsDirty = false;
			EntitySet = new List<Phase>();
		}
		#endregion
		#region Overrides
		public override IEnumerable<Project> GetEntitySet<Project>()
		{
			return null;
		}
		[JsonIgnore]
		public override string PathName => "Phase";
		#endregion
	}
}
