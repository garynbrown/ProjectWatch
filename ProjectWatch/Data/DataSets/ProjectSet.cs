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
	public class ProjectSet : EntitySetBase<Project>, IEntitySet<Project>
	{
		public ProjectSet()
		{
			IsDirty = false;
		}
		public override IEnumerable<Project> GetEntitySet<Project>()
		{
			return null;
		}

		public override string PathName => "Project";

		//public override  EntitySetBase<Project> DeserializeSet(string JsonString)
		//{
		//	return  JsonConvert.DeserializeObject<ProjectSet>(JsonString);
		//}

	}
}
