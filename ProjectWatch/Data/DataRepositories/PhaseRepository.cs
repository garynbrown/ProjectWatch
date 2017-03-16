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
	[Export(typeof(IPhaseRepository))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class PhaseRepository : DataRepositoryBase<Phase>, IPhaseRepository
	{
		#region Constructors
		public PhaseRepository()
		{
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Phase>>();
		}

		public PhaseRepository(EntitySetBase<Phase> targetEntitySet) : base(targetEntitySet)
		{
		}
		#endregion
		#region Overrides
		protected override void DeserializeEntitySet()
		{
			string jString =  JsonFileSupport.JsonReadFile(TargetEntitySet.PathName);
			var ts = JsonConvert.DeserializeObject<PhaseSet>(jString);
			if (ts?.EntitySet == null)
			{
				TargetEntitySet.EntitySet = new List<Phase>();
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
			var ts = JsonConvert.DeserializeObject<PhaseSet>(jString);
			if (ts?.EntitySet == null)
			{
				TargetEntitySet.EntitySet = new List<Phase>();
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
