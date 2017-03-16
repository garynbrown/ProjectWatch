using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Data;
using Newtonsoft.Json;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DataSets
{
	[Export(typeof(EntitySetBase<Contact>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class ContactSet : EntitySetBase<Contact>, IEntitySet<Contact>
	{
		#region Constructors
		public ContactSet()
		{
			IsDirty = false;
			EntitySet = new List<Contact>();
		}
		#endregion
		#region Overrides
		public override IEnumerable<Contact> GetEntitySet<Contact>()
		{
			return null;
			//DataRepositoryFactory<Project>;
		}
		[JsonIgnore]
		public override string PathName => "Contact";
		#endregion
	}
}
