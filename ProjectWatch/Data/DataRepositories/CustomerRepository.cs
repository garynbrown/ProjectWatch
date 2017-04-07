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
	[Export(typeof(ICustomerRepository))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class CustomerRepository : DataRepositoryBase<Customer>, ICustomerRepository
	{
		#region Constructors
		public CustomerRepository()
		{
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Customer>>();
		}

		public CustomerRepository(EntitySetBase<Customer> targetEntitySet) : base(targetEntitySet)
		{
			
		}
		#endregion
		#region Overrides
		protected override void DeserializeEntitySet(bool archive = false)
		{
			string path;
			string jString;
			if (archive)
			{
				path = Path.Combine("Archive", TargetEntitySet.PathName);
				jString = JsonConvert.SerializeObject(ArchiveEntitySet, Formatting.Indented);
			}
			else
			{
				path = TargetEntitySet.PathName;
				jString = JsonConvert.SerializeObject(TargetEntitySet, Formatting.Indented);
			}
			var ts = JsonConvert.DeserializeObject<CustomerSet>(jString);
			if (ts?.EntitySet == null)
			{
				if (!archive)
				{
					TargetEntitySet.EntitySet = new List<Customer>();
				}
				else
				{
					ArchiveEntitySet.EntitySet = new List<Customer>();
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
			string jString;
			if (archive)
			{
				path = Path.Combine("Archive", TargetEntitySet.PathName);
				jString = JsonConvert.SerializeObject(ArchiveEntitySet, Formatting.Indented);
			}
			else
			{
				path = TargetEntitySet.PathName;
				jString = JsonConvert.SerializeObject(TargetEntitySet, Formatting.Indented);
			}
			var ts = JsonConvert.DeserializeObject<CustomerSet>(jString);
			if (ts?.EntitySet == null)
			{
				if (!archive)
				{
					TargetEntitySet.EntitySet = new List<Customer>();
				}
				else
				{
					ArchiveEntitySet.EntitySet = new List<Customer>();
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
