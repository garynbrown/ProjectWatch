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
	[Export(typeof(IBillingRepository))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	class BillingRepository :  DataRepositoryBase<Billing>, IBillingRepository
	{
		#region Constructors
		public BillingRepository()
		{
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Billing>>();
			base.ArchiveEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Billing>>();
		}

		public BillingRepository(EntitySetBase<Billing> targetEntitySet) : base(targetEntitySet)
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
			var ts = JsonConvert.DeserializeObject<BillingSet>(jString);
			if (ts?.EntitySet == null)
			{
				if (!archive)
				{
					TargetEntitySet.EntitySet = new List<Billing>();
				}
				else
				{
					ArchiveEntitySet.EntitySet = new List<Billing>();
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
			var ts = JsonConvert.DeserializeObject<BillingSet>(jString);
			if (ts?.EntitySet == null)
			{
				if (!archive)
				{
					TargetEntitySet.EntitySet = new List<Billing>();
				}
				else
				{
					ArchiveEntitySet.EntitySet = new List<Billing>();
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
