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
	[Export(typeof(IPhaseRepository))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class PhaseRepository : DataRepositoryBase<Phase>, IPhaseRepository
	{
		#region Constructors
		public PhaseRepository()
		{
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Phase>>();
			base.ArchiveEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Phase>>();
		}

		public PhaseRepository(EntitySetBase<Phase> targetEntitySet) : base(targetEntitySet)
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
			var ts = JsonConvert.DeserializeObject<PhaseSet>(jString);
			if (ts?.EntitySet == null)
			{
				if (!archive)
				{
					TargetEntitySet.EntitySet = new List<Phase>();
				}
				else
				{
					ArchiveEntitySet.EntitySet = new List<Phase>();
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
			var ts = JsonConvert.DeserializeObject<PhaseSet>(jString);
			if (ts?.EntitySet == null)
			{
				if (!archive)
				{
					TargetEntitySet.EntitySet = new List<Phase>();
				}
				else
				{
					ArchiveEntitySet.EntitySet = new List<Phase>();
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
