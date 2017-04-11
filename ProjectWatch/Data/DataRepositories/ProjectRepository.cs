using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Core.Common.Core;
using Core.Common.Data;
using Core.Common.Utils;
using Newtonsoft.Json;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Data.DataSets;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DataRepositories
{
	[Export(typeof(IProjectRepository))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class ProjectRepository : DataRepositoryBase<Project>, IProjectRepository
	{
		#region Constructors
		public ProjectRepository()
		{
			
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Project>>();
			base.ArchiveEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Project>>();
			
		}
		// Dependancy injecting constructor for testing
		public ProjectRepository(EntitySetBase<Project> targetEntitySet) : base(targetEntitySet)
		{
			
		}
		#endregion
		#region Overrides
		protected override void DeserializeEntitySet(bool archive = false)
		{
			string path;
			if (archive)
			{
				path = Path.Combine("Archive", TargetEntitySet.PathName);
			}
			else
			{
				path = TargetEntitySet.PathName;
			}
			string jString = JsonFileSupport.JsonReadFile(path);
			var ts = JsonConvert.DeserializeObject<ProjectSet>(jString);
			if (ts?.EntitySet == null)
			{
				if (!archive)
				{
					TargetEntitySet.EntitySet = new List<Project>();
				}
				else
				{
					ArchiveEntitySet.EntitySet = new List<Project>();
				}
			}
			else
			{
				if (!archive)
				{
					TargetEntitySet = ts;
				}
				else
				{
					ArchiveEntitySet = ts;
				}
			}
			if (!archive)
			{
				TargetEntitySet.IsDirty = false;
			}
			else
			{
				ArchiveEntitySet.IsDirty = false;
			}
		}

		protected override async void DeserializeEntitySetAsync(bool archive = false)
		{
			string path;
			if (archive)
			{
				path = Path.Combine("Archive", TargetEntitySet.PathName);
			}
			else
			{
				path = TargetEntitySet.PathName;
			}
			string jString = JsonFileSupport.JsonReadFile(path);
			var ts = JsonConvert.DeserializeObject<ProjectSet>(jString);
			if (ts?.EntitySet == null)
			{
				if (!archive)
				{
					TargetEntitySet.EntitySet = new List<Project>();
				}
				else
				{
					ArchiveEntitySet.EntitySet = new List<Project>();
				}
			}
			else
			{
				if (!archive)
				{
					TargetEntitySet = ts;
				}
				else
				{
					ArchiveEntitySet = ts;
				}
			}
			if (!archive)
			{
				TargetEntitySet.IsDirty = false;
			}
			else
			{
				ArchiveEntitySet.IsDirty = false;
			}
		}
		#endregion
	}
}
