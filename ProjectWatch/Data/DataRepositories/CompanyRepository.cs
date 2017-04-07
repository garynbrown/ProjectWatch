using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Data;
using Core.Common.Utils;
using Newtonsoft.Json;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Data.DataSets;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DataRepositories
{
	[Export(typeof(ICompanyRepository))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class CompanyRepository : DataRepositoryBase<Company>, ICompanyRepository
	{
		#region Constructors
		public CompanyRepository()
		{
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Company>>();
			base.ArchiveEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Company>>();
		}

		public CompanyRepository(EntitySetBase<Company> targetEntitySet ) : base(targetEntitySet)
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
			var ts = JsonConvert.DeserializeObject<CompanySet>(jString);
			if (ts?.EntitySet == null)
			{
				if (!archive)
				{
					TargetEntitySet.EntitySet = new List<Company>();
				}
				else
				{
					ArchiveEntitySet.EntitySet = new List<Company>();
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
			var ts = JsonConvert.DeserializeObject<CompanySet>(jString);
			if (ts?.EntitySet == null)
			{
				if (!archive)
				{
					TargetEntitySet.EntitySet = new List<Company>();
				}
				else
				{
					ArchiveEntitySet.EntitySet = new List<Company>();
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
