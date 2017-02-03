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
	}
}
