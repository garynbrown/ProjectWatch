using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using ProjectWatch.Entities;

namespace ProjectWatch.Data
{
	[Export(typeof(EntitySetDataModel))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class EntitySetDataModel
	{
		public IEntitySet<Project> Projects;
	}
}
