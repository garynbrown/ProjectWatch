using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Data;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DataSets
{
	[Export(typeof(EntitySetBase<Contact>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class ContactSet : EntitySetBase<Project>, IEntitySet<Project>
	{
		public ContactSet()
		{
			IsDirty = false;
		}
		public override IEnumerable<Contact> GetEntitySet<Contact>()
		{
			return null;
			//DataRepositoryFactory<Project>;
		}
		public override string PathName => "Contact";
	}
}
