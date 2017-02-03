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
	[Export(typeof(ICompanyRepository))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	class CompanyRepository : DataRepositoryBase<Company>, ICompanyRepository
	{
		#region Constructors
		public CompanyRepository()
		{
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Company>>();
		}

		public CompanyRepository(EntitySetBase<Company> targetEntitySet ) : base(targetEntitySet)
		{
		}
		#endregion
	}
}
