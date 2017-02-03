using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using ProjectWatch.Entities;

namespace ProjectWatch.Contracts.RepositoryInterfaces
{
	public interface ITimeCardRepository : IDataRepository<TimeCard>
	{
		Task<IEnumerable<TimeCard>> GetDefaultRangeTimeCards();
		Task<IEnumerable<TimeCard>> GetRangeTimeCards(DateTime startDate, DateTime endDate);
		Task<IEnumerable<TimeCard>> GetLastMonthsTimeCards();
		Task<IEnumerable<TimeCard>> GetThisMonthsTimeCards();
		TimeCard GetTodaysTimeCard();


	}
}
