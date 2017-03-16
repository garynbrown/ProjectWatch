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
using Newtonsoft.Json;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Data.DataSets;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DataRepositories
{
	[Export(typeof(IContactRepository))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class ContactRepository : DataRepositoryBase<Contact>, IContactRepository
	{
		#region Constructors
		public ContactRepository()
		{
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Contact>>();
		}

		public ContactRepository(EntitySetBase<Contact> targetEntitySet) : base(targetEntitySet)
		{
			
		}
		#endregion
		#region Overrides
		protected override void DeserializeEntitySet()
		{
			string jString =  JsonFileSupport.JsonReadFile(TargetEntitySet.PathName);
			var ts = JsonConvert.DeserializeObject<ContactSet>(jString);
			if (ts?.EntitySet == null)
			{
				TargetEntitySet.EntitySet = new List<Contact>();
			}
			else
			{
				TargetEntitySet = ts;
			}
			TargetEntitySet.IsDirty = false;
		}

		protected override async void DeserializeEntitySetAsync()
		{
			string jString = await JsonFileSupport.JsonReadFileAsync(TargetEntitySet.PathName);
			var ts = JsonConvert.DeserializeObject<ContactSet>(jString);
			if (ts?.EntitySet == null)
			{
				TargetEntitySet.EntitySet = new List<Contact>();
			}
			else
			{
				TargetEntitySet = ts;
			}
			TargetEntitySet.IsDirty = false;
		}
		#endregion
	}
}
