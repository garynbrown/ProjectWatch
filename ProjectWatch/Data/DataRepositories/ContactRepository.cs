using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Data;
using ProjectWatch.Contracts.RepositoryInterfaces;
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
	}
}
