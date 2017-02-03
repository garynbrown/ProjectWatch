﻿using System;
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
	[Export(typeof(ICustomerRepository))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
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
	}
}