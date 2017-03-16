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
	[Export(typeof(EntitySetBase<Project>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class ProjectSet : EntitySetBase<Project>, IEntitySet<Project>
	{
		#region Constructors
		public ProjectSet()
		{
			IsDirty = false;
		}
		#endregion
		#region Overrides
		public override IEnumerable<Project> GetEntitySet<Project>()
		{
			return null;
			EntitySet = new List<Entities.Project>();
		}

		[JsonIgnore]
		public override string PathName => "Project";
		#endregion

	}
}
