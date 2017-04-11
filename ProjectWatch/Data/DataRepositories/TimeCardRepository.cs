using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Common.Core;
using Core.Common.Data;
using Core.Common.Utils;
using Newtonsoft.Json;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DataRepositories
{
	[Export(typeof(ITimeCardRepository))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class TimeCardRepository : DataRepositoryBase<TimeCard>, ITimeCardRepository
	{
		#region Constructors
		public TimeCardRepository()
		{
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<TimeCard>>();
			base.TargetEntitySet.EntitySet = new List<TimeCard>();
		}

		public TimeCardRepository(EntitySetBase<TimeCard> targetEntitySet) : base(targetEntitySet)
		{
		}
		#endregion

		#region Overrides

		public override async Task<TimeCard> AddAsync(TimeCard timeCard)
		{
			int tcId = (timeCard.EntityId > 20160000) ? timeCard.EntityId :  TimeCard.MakeTimeCardIdFromDate(DateTime.Now);
			string tcPath = MakePathFromId(tcId);
			TimeCard retEntity = null;
			if (TargetEntitySet == null)
				return null;
			if (TargetEntitySet.EntitySet == null)
			{
				TargetEntitySet.EntitySet = new List<TimeCard>();
			}
			List<TimeCard> Entities = (TargetEntitySet.EntitySet as List<TimeCard>) ;
			int indx = Entities.FindIndex(e => e.TimeId == timeCard.TimeId);
			if (timeCard.TimeId > 0 && indx >= 0)
			{
				return await UpdateAsync(timeCard);
			}
			else
			{
				timeCard.EntityId = tcId;
				Entities.Add(timeCard);
				string jsonString = JsonConvert.SerializeObject(timeCard, Formatting.Indented);
				bool isGood = await JsonFileSupport.WriteFileAsync(tcPath, jsonString);
				TargetEntitySet.IsDirty = !isGood;
			}
			return timeCard;
		}
		public override TimeCard Add(TimeCard timeCard)
		{
			int tcId = (timeCard.EntityId > 20160000) ? timeCard.EntityId : TimeCard.MakeTimeCardIdFromDate(DateTime.Now);
			string tcPath = MakePathFromId(tcId);
			TimeCard retEntity = null;
			if (TargetEntitySet == null)
				return null;
			if (TargetEntitySet.EntitySet == null)
			{
				TargetEntitySet.EntitySet = new List<TimeCard>();
			}
			List<TimeCard> Entities = (TargetEntitySet.EntitySet as List<TimeCard>);
			int indx = Entities.FindIndex(e => e.TimeId == timeCard.TimeId);
			if (timeCard.TimeId > 0 && indx >= 0)
			{
				return Update(timeCard);
			}
			else
			{
				timeCard.EntityId = tcId;
				Entities.Add(timeCard);
				string jsonString = JsonConvert.SerializeObject(timeCard, Formatting.Indented);
				bool isGood = JsonFileSupport.WriteFile(tcPath, jsonString);
				TargetEntitySet.IsDirty = !isGood;
			}
			return timeCard;
		}

		public override async Task<TimeCard> GetAsync(int id)
		{
			string location = Path.Combine(JsonFileSupport.DataPath, MakePathFromId(id) + ".json");
			if (!File.Exists(location))
			{
				return null;
			}

			return await DeserializeTimeCardAsync(id);
		}
		public override  TimeCard Get(int id)
		{
			string location = Path.Combine(JsonFileSupport.DataPath, MakePathFromId(id) + ".json");
			if (!File.Exists(location))
			{
				return null;
			}
			return  DeserializeTimeCard(id);
		}

		public override EntitySetBase<TimeCard> Get()
		{
			if (TargetEntitySet.EntitySet == null || !TargetEntitySet.EntitySet.Any())
			{
				TargetEntitySet.EntitySet = GetDefaultRangeTimeCards();
			}
			return TargetEntitySet;
		}
		public override async Task< EntitySetBase<TimeCard>> GetAsync()
		{
			if (TargetEntitySet.EntitySet == null || !TargetEntitySet.EntitySet.Any())
			{
				TargetEntitySet.EntitySet = GetDefaultRangeTimeCards();
			}
			return  TargetEntitySet;
		}

		//public override async void RemoveAsync(int id)
		//{
		//	string location = Path.Combine(JsonFileSupport.DataPath, MakePathFromId(id) + ".json");
		//	if (TargetEntitySet?.EntitySet == null)
		//		return;
		//	List<TimeCard> Entities = (TargetEntitySet.EntitySet as List<TimeCard>) ?? new List<TimeCard>();
		//	int indx = Entities.FindIndex(e => e.EntityId == id);
		//	if (indx > -1)
		//	{
		//		Entities.RemoveAt(indx);
		//		if (File.Exists(location))
		//		{
		//			File.Delete(location);
		//		}
		//	}
		//}
		public override async void RemoveAsync(TimeCard timeCard)
		{
				string location = Path.Combine(JsonFileSupport.DataPath, MakePathFromId(timeCard.TimeId) + ".json");
				if (TargetEntitySet?.EntitySet == null)
					return;
				List<TimeCard> Entities = (TargetEntitySet.EntitySet as List<TimeCard>) ?? new List<TimeCard>();
				int indx = Entities.FindIndex(e => e.EntityId == timeCard.TimeId);
				if (indx > -1)
				{
					Entities.RemoveAt(indx);
					if (File.Exists(location))
					{
						File.Delete(location);
					}
				}
		}
		public override  void Remove(TimeCard timeCard)
		{
			string location = Path.Combine(JsonFileSupport.DataPath, MakePathFromId(timeCard.TimeId) + ".json");
			if (TargetEntitySet?.EntitySet == null)
				return;
			List<TimeCard> Entities = (TargetEntitySet.EntitySet as List<TimeCard>) ?? new List<TimeCard>();
			int indx = Entities.FindIndex(e => e.EntityId == timeCard.TimeId);
			if (indx > -1)
			{
				Entities.RemoveAt(indx);
				if (File.Exists(location))
				{
					File.Delete(location);
				}
			}
		}
		public override async Task<TimeCard> UpdateAsync(TimeCard timeCard)
		{
			TimeCard retEntity = null;
			if (TargetEntitySet?.EntitySet == null)
				return null;
			List<TimeCard> Entities = (TargetEntitySet.EntitySet as List<TimeCard>) ?? new List<TimeCard>();
			int indx = Entities.FindIndex(e => e.EntityId == timeCard.EntityId);
			if (indx > -1)
			{
				string tcPath = MakePathFromId(timeCard.EntityId);
				Entities.RemoveAt(indx);
				Entities.Add(timeCard);
				string jsonString = JsonConvert.SerializeObject(timeCard, Formatting.Indented);
				bool isGood = await JsonFileSupport.WriteFileAsync(tcPath, jsonString);
				TargetEntitySet.IsDirty = !isGood;
			}
			else
			{
				retEntity = await AddAsync(timeCard);			
			}
			return retEntity;
		}
		public override TimeCard Update(TimeCard timeCard)
		{
			TimeCard retEntity = null;
			if (TargetEntitySet?.EntitySet == null)
				return null;
			List<TimeCard> Entities = (TargetEntitySet.EntitySet as List<TimeCard>) ?? new List<TimeCard>();
			int indx = Entities.FindIndex(e => e.EntityId == timeCard.EntityId);
			if (indx > -1)
			{
				string tcPath = MakePathFromId(timeCard.EntityId);
				Entities.RemoveAt(indx);
				Entities.Add(timeCard);
				string jsonString = JsonConvert.SerializeObject(timeCard, Formatting.Indented);
				bool isGood =  JsonFileSupport.WriteFile(tcPath, jsonString);
				TargetEntitySet.IsDirty = !isGood;
			}
			else
			{
				retEntity = Add(timeCard);
			}
			return retEntity;
		}

		protected override void DeserializeEntitySet(bool archive = false)
		{
			TargetEntitySet.EntitySet = new List<TimeCard>(GetDefaultRangeTimeCards());
		}

		protected override async void DeserializeEntitySetAsync(bool archive = false)
		{
			TargetEntitySet.EntitySet = new List<TimeCard>(await GetDefaultRangeTimeCardsAsync());
		}

		#endregion

		#region Contract_Implementations

		public IEnumerable<TimeCard> GetDefaultRangeTimeCards()
		{
			List<TimeCard> timeCards = new List<TimeCard>();
			timeCards.AddRange( GetLastMonthsTimeCards());
			timeCards.AddRange( GetThisMonthsTimeCards());
			return timeCards;
		}
		public async Task<IEnumerable<TimeCard>> GetDefaultRangeTimeCardsAsync()
		{
			List<TimeCard> timeCards = new List<TimeCard>();
			timeCards.AddRange(await GetLastMonthsTimeCardsAsync());
			timeCards.AddRange(await GetThisMonthsTimeCardsAsync());
			return timeCards;
		}
		public IEnumerable<TimeCard> GetLastMonthsTimeCards()
		{
			List<TimeCard> timeCards = new List<TimeCard>();
			int BaseId = DateTime.Now.Year*10000 + (DateTime.Now.Month - 1)*100;
			for (int i = BaseId+1; i < BaseId+31; i++)
			{
				TimeCard newTimeCard =  DeserializeTimeCard(i);
				if (newTimeCard != null)
				{
					timeCards.Add(newTimeCard);
				}
			}
			return timeCards;
		}
		public async Task<IEnumerable<TimeCard>> GetLastMonthsTimeCardsAsync()
		{
			List<TimeCard> timeCards = new List<TimeCard>();
			int BaseId = DateTime.Now.Year * 10000 + (DateTime.Now.Month - 1) * 100;
			for (int i = BaseId + 1; i < BaseId + 31; i++)
			{
				TimeCard newTimeCard = await DeserializeTimeCardAsync(i);
				if (newTimeCard != null)
				{
					timeCards.Add(newTimeCard);
				}
			}
			return timeCards;
		}
		public IEnumerable<TimeCard> GetRangeTimeCards(DateTime startDate, DateTime endDate)
		{
			int today = TimeCard.MakeTimeCardIdFromDate(DateTime.Now);
			if (startDate == DateTime.MinValue)
				startDate = new DateTime(2017,1,1);

			int startId = TimeCard.MakeTimeCardIdFromDate(startDate);
			int endId = TimeCard.MakeTimeCardIdFromDate(endDate);
			if (startId > endId)
			{
				int tId = startId;
				startId = endId;
				endId = tId;
			}
			endId = (endId > today) ? today : endId;
			List<TimeCard> timeCards = new List<TimeCard>();
			for (int i = startId; i <= endId; i++)
			{
				TimeCard t = DeserializeTimeCard(i);
				if (t == null)
					continue;
				timeCards.Add(t);
			}
			return timeCards;
		}
		public async Task<IEnumerable<TimeCard>> GetRangeTimeCardsAsync(DateTime startDate, DateTime endDate)
		{
			int today = TimeCard.MakeTimeCardIdFromDate(DateTime.Now);
			if (startDate == DateTime.MinValue)
				startDate = new DateTime(2017, 1, 1);
			int startId = TimeCard.MakeTimeCardIdFromDate(startDate);
			int endId = TimeCard.MakeTimeCardIdFromDate(endDate);
			if (startId > endId)
			{
				int tId = startId;
				startId = endId;
				endId = tId;
			}
			endId = (endId > today) ? today : endId;
			List<TimeCard> timeCards = new List<TimeCard>();
			for (int i = startId; i <= endId; i++)
			{
				timeCards.Add(await GetAsync(i));
			}
			return timeCards;
		}
		public async Task<IEnumerable<TimeCard>> GetThisMonthsTimeCardsAsync()
		{
			List<TimeCard> timeCards = new List<TimeCard>();
			int BaseId = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100;
			for (int i = BaseId + 1; i < BaseId + 31; i++)
			{
				TimeCard newTimeCard = await DeserializeTimeCardAsync(i);
				if (newTimeCard != null)
				{
					timeCards.Add(newTimeCard);
				}
			}
			return timeCards;
		}
		public IEnumerable<TimeCard> GetThisMonthsTimeCards()
		{
			List<TimeCard> timeCards = new List<TimeCard>();
			int BaseId = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100;
			for (int i = BaseId + 1; i < BaseId + 31; i++)
			{
				TimeCard newTimeCard = DeserializeTimeCard(i);
				if (newTimeCard != null)
				{
					timeCards.Add(newTimeCard);
				}
			}
			return timeCards;
		}
		public TimeCard GetTodaysTimeCard()
		{
			List<TimeCard> timeCards;
			TimeCard _timeCard = new TimeCard();
			int id = TimeCard.MakeTimeCardIdFromDate(DateTime.Now);
			if (TargetEntitySet == null)
				return null;
			if (TargetEntitySet.EntitySet == null)
			{
				TargetEntitySet.EntitySet = new List<TimeCard>();
			}
			List<TimeCard> _timeCards = TargetEntitySet.EntitySet as List<TimeCard>;
			int indx = _timeCards.FindIndex(i => i.EntityId == id);
			if (indx > -1)
			{
				return _timeCards[indx];
			}
			else
			{
				_timeCard =  DeserializeTimeCard(id);
				_timeCards.Add(_timeCard);
			}
			return _timeCard;
		}

		public TimeCard GetOrCreateTodaysTimeCard()
		{
			List<TimeCard> timeCards;
			TimeCard _timeCard = new TimeCard();
			int id = TimeCard.MakeTimeCardIdFromDate(DateTime.Now);
			if (TargetEntitySet == null)
				return null;
			if (TargetEntitySet.EntitySet == null)
			{
				TargetEntitySet.EntitySet = new List<TimeCard>();
			}
			List<TimeCard> _timeCards = TargetEntitySet.EntitySet as List<TimeCard>;
			int indx = _timeCards.FindIndex(i => i.EntityId == id);
			if (indx > -1)
			{
				return _timeCards[indx];
			}
			else
			{
				_timeCard = DeserializeTimeCard(id);
				if (_timeCard == null)
				{
					_timeCard = TimeCard.CreateTimeCard(id);
					_timeCard = Add(_timeCard);
				}
				else
				{
					_timeCards.Add(_timeCard);
				}
			}
			return _timeCard;
		}

		#endregion

		#region Methods

		private TimeCard DeserializeTimeCard(int id)
		{
			TimeCard retTimeCard = TimeCardFromCache(id);
			if (retTimeCard == null)
			{
				string jString = JsonFileSupport.JsonReadFile(MakePathFromId(id));
				if (jString == string.Empty)
				{
					return null;
				}
				retTimeCard = JsonConvert.DeserializeObject<TimeCard>(jString);
			}
			return retTimeCard;
		}

		TimeCard TimeCardFromCache(int id )
		{
			if (TargetEntitySet.EntitySet == null)
			{
				TargetEntitySet.EntitySet = new List<TimeCard>();
			}
			List<TimeCard> _timeCards = TargetEntitySet.EntitySet as List<TimeCard>;
			int indx = _timeCards.FindIndex(i => i.EntityId == id);
			if (indx > -1)
			{
				return _timeCards[indx];
			}
			else
			{
				return null;
			}

		}
		private async Task<TimeCard> DeserializeTimeCardAsync(int id)
		{
			TimeCard retTimeCard = TimeCardFromCache(id);
			if (retTimeCard == null)
			{

				string jString = await JsonFileSupport.JsonReadFileAsync(MakePathFromId(id));
				if (jString == string.Empty)
				{
					return null;
				}
				retTimeCard = JsonConvert.DeserializeObject<TimeCard>(jString);
			}
			return retTimeCard;
		}
		private string MakePathFromId(int id)
		{
			int year = id/10000;
			int month = (id - year*10000)/100;
			return $@"{TargetEntitySet.PathName}\{year}\{month}\{id}";
		}
		#endregion
	}
}
