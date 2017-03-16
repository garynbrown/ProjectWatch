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
	[Export(typeof(EntitySetBase<Company>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class CompanySet : EntitySetBase<Company>, IEntitySet<Company>
	{
		#region Constructors
		public CompanySet()
		{
			IsDirty = false;
			EntitySet = new List<Company>();
		}
		#endregion
		#region Overrides
		public override IEnumerable<Company> GetEntitySet<Company>()
		{
			return null;
		}
		[JsonIgnore]
		public override string PathName => "Company";
		#endregion
	}
}
