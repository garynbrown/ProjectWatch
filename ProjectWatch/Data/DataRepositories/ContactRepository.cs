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
	[Export(typeof(IContactRepository))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class ContactRepository : DataRepositoryBase<Contact>, IContactRepository
	{
		#region Constructors
		public ContactRepository()
		{
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Contact>>();
			base.ArchiveEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Contact>>();
		}

		public ContactRepository(EntitySetBase<Contact> targetEntitySet) : base(targetEntitySet)
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
			var ts = JsonConvert.DeserializeObject<ContactSet>(jString);
			if (ts?.EntitySet == null)
			{
				if (!archive)
				{
					TargetEntitySet.EntitySet = new List<Contact>();
				}
				else
				{
					ArchiveEntitySet.EntitySet = new List<Contact>();
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
			var ts = JsonConvert.DeserializeObject<ContactSet>(jString);
			if (ts?.EntitySet == null)
			{
				if (!archive)
				{
					TargetEntitySet.EntitySet = new List<Contact>();
				}
				else
				{
					ArchiveEntitySet.EntitySet = new List<Contact>();
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
