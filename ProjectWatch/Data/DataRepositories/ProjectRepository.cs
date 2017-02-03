using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Data;
using Core.Common.Utils;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Data.DataSets;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DataRepositories
{
	[Export(typeof(IProjectRepository))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class ProjectRepository : DataRepositoryBase<Project>, IProjectRepository
	{
		#region Constructors
		public ProjectRepository()
		{
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Project>>();
			
		}
		// Dependancy injecting constructor for testing
		public ProjectRepository(EntitySetBase<Project> targetEntitySet) : base(targetEntitySet)
		{
			
		}
		#endregion
	}
}
