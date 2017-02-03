using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using ProjectWatch.Entities;

namespace ProjectWatch.Contracts.RepositoryInterfaces
{
	public interface ICustomerRepository : IDataRepository<Customer>
	{
	}
}
