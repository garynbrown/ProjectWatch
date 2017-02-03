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
	[Export(typeof(IBillingRepository))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	class BillingRepository :  DataRepositoryBase<Billing>, IBillingRepository
	{
		#region Constructors
		public BillingRepository()
		{
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Billing>>();
		}

		public BillingRepository(EntitySetBase<Billing> targetEntitySet) : base(targetEntitySet)
		{
		}
		#endregion
	}
}
