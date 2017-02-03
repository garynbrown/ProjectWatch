using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
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
	[Export(typeof(ITimeCardRepository))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class TimeCardRepository : DataRepositoryBase<TimeCard>, ITimeCardRepository
	{
		#region Constructors
		public TimeCardRepository()
		{
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<TimeCard>>();
		}

		public TimeCardRepository(EntitySetBase<TimeCard> targetEntitySet) : base(targetEntitySet)
		{
		}
		#endregion

		#region Overrides

		public override async Task<TimeCard> AddAsync(TimeCard timeCard)
		{
			int tcId = MakeIdFromDate(DateTime.Now);
			string tcPath = MakePathFromId(tcId);
			TimeCard retEntity = null;
			if (TargetEntitySet?.EntitySet == null)
				return null;
			List<TimeCard> Entities = (TargetEntitySet.EntitySet as List<TimeCard>) ?? new List<TimeCard>();
			int indx = Entities.FindIndex(e => e.TimeId == timeCard.TimeId);
			if (timeCard.TimeId > 0 && indx >= 0)
			{
				return await UpdateAsync(timeCard);
			}
			else
			{
				timeCard.EntityId = tcId;
				Entities.Add(timeCard);
				string jsonString = JsonConvert.SerializeObject(TargetEntitySet, Formatting.Indented);
				bool isGood = await JsonFileSupport.WriteFileAsync(tcPath, jsonString);
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

			return await DeserializeTimeCard(id);
		}
		public override async void RemoveAsync(int id)
		{
			string location = Path.Combine(JsonFileSupport.DataPath, MakePathFromId(id) + ".json");
			if (TargetEntitySet?.EntitySet == null)
				return;
			List<TimeCard> Entities = (TargetEntitySet.EntitySet as List<TimeCard>) ?? new List<TimeCard>();
			int indx = Entities.FindIndex(e => e.EntityId == id);
			if (indx > -1)
			{
				Entities.RemoveAt(indx);
				if (File.Exists(location))
				{
					File.Delete(location);
				}
			}
		}
		public override async void RemoveAsync(TimeCard timeCard)
		{
			RemoveAsync(timeCard.EntityId);
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
				Entities.RemoveAt(indx);
				Entities.Add(timeCard);
			}
			retEntity = await AddAsync(timeCard);
			return retEntity;
		}
		#endregion

		#region Contract_Implementations

		public async Task<IEnumerable<TimeCard>> GetDefaultRangeTimeCards()
		{
			List<TimeCard> timeCards = new List<TimeCard>();
			timeCards.AddRange(await  GetLastMonthsTimeCards());
			timeCards.AddRange(await GetThisMonthsTimeCards());
			return timeCards;
		}
		public async Task<IEnumerable<TimeCard>> GetLastMonthsTimeCards()
		{
			List<TimeCard> timeCards = new List<TimeCard>();
			int BaseId = DateTime.Now.Year*10000 + (DateTime.Now.Month - 1)*100;
			for (int i = BaseId+1; i < BaseId+31; i++)
			{
				TimeCard newTimeCard = await DeserializeTimeCard(i);
				if (newTimeCard != null)
				{
					timeCards.Add(newTimeCard);
				}
			}
			return timeCards;
		}
		public async Task<IEnumerable<TimeCard>> GetRangeTimeCards(DateTime startDate, DateTime endDate)
		{
			int today = MakeIdFromDate(DateTime.Now);

			int startId = MakeIdFromDate(startDate);
			int endId = MakeIdFromDate(endDate);
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
				timeCards.Add( await GetAsync(i));
			}
			return timeCards;
		}
		public async Task<IEnumerable<TimeCard>> GetThisMonthsTimeCards()
		{
			List<TimeCard> timeCards = new List<TimeCard>();
			int BaseId = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100;
			for (int i = BaseId + 1; i < BaseId + 31; i++)
			{
				TimeCard newTimeCard = await DeserializeTimeCard(i);
				if (newTimeCard != null)
				{
					timeCards.Add(newTimeCard);
				}
			}
			return timeCards;
		}
		public TimeCard GetTodaysTimeCard()
		{
			return DeserializeTimeCard(MakeIdFromDate(DateTime.Now)).Result;
		}
		#endregion

		#region Methods

		private async Task<TimeCard> DeserializeTimeCard(int id)
		{
			string jString = await JsonFileSupport.JsonReadFileAsync(MakePathFromId(id));
			if (jString == null)
				return null;
			return JsonConvert.DeserializeObject<TimeCard>(jString);
		}

		int MakeIdFromDate(DateTime date)
		{
			return date.Year * 10000 + date.Month * 100 + date.Day;

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
