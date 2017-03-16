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
		IEnumerable<TimeCard> GetDefaultRangeTimeCards();
		Task<IEnumerable<TimeCard>> GetDefaultRangeTimeCardsAsync();
		IEnumerable<TimeCard> GetRangeTimeCards(DateTime startDate, DateTime endDate);
		Task<IEnumerable<TimeCard>> GetRangeTimeCardsAsync(DateTime startDate, DateTime endDate);
		IEnumerable<TimeCard> GetLastMonthsTimeCards();
		Task<IEnumerable<TimeCard>> GetLastMonthsTimeCardsAsync();
		IEnumerable<TimeCard> GetThisMonthsTimeCards();
		Task<IEnumerable<TimeCard>> GetThisMonthsTimeCardsAsync();
		TimeCard GetTodaysTimeCard();
		TimeCard GetOrCreateTodaysTimeCard();


	}
}
