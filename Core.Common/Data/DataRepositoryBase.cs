using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Utils;
using Newtonsoft.Json;
using System.Collections;


namespace Core.Common.Data
{
	public abstract class DataRepositoryBase<T>  : IDataRepository<T>
		where T : ClientEntityBase, new()
	{
		#region Properties
		protected EntitySetBase<T> TargetEntitySet = null;
		#endregion

		#region Constructors
		public DataRepositoryBase()
		{
			ClientEntityType = typeof(T);
			//TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<T>>();
		}

		public Type ClientEntityType { get; }

		public DataRepositoryBase(EntitySetBase<T> targetEntitySet)
		{
			TargetEntitySet = targetEntitySet;
		}
		#endregion
			
		#region Contract_Implementations
		public virtual async Task<T> AddAsync(T entity)
		{
			T retEntity = null;
			if (TargetEntitySet?.EntitySet == null)
				return null;
			List<T> Entities = (TargetEntitySet.EntitySet as List<T>) ?? new List<T>();
			if (Entities.Contains(entity))
			{
				return await UpdateAsync(entity);
			}
			else
			{
				entity.EntityId = Entities.Any() ? Entities.Max(p => p.EntityId) + 1 :0;
				Entities.Add(entity);
				SerializeEntitySetAsync();
			}
			return entity;
		}

		public virtual T Add(T entity)
		{
			T retEntity = null;
			if (TargetEntitySet?.EntitySet == null)
				return null;
			List<T> Entities = (TargetEntitySet.EntitySet as List<T>) ?? new List<T>();
			if (Entities.Contains(entity))
			{
				return  Update(entity);
			}
			else
			{
				entity.EntityId = Entities.Any() ? Entities.Max(p => p.EntityId) + 1: 0;
				Entities.Add(entity);
				SerializeEntitySet();
			}
			return entity;
		}

		public virtual async  Task<EntitySetBase<T>> GetAsync()
		{
			if (TargetEntitySet.EntitySet == null || !TargetEntitySet.EntitySet.Any())
			{
				DeserializeEntitySetAsync();
			}
			if (TargetEntitySet.IsDirty)
			{
				SerializeEntitySetAsync();
			}
			return TargetEntitySet;
			//EntitySetBase<T> retVal = await GetEntitySetAsync();
			//return  retVal;
		}

		public EntitySetBase<T> Get()
		{
			if (TargetEntitySet.EntitySet == null || !TargetEntitySet.EntitySet.Any())
			{
				DeserializeEntitySet();
			}
			if (TargetEntitySet.IsDirty)
			{
				SerializeEntitySet();
			}
			return TargetEntitySet;
		}

		public virtual async Task<T> GetAsync(int id)
		{
			if (id < 0)
			{
				return null;
			}

			if (TargetEntitySet.EntitySet == null)
			{
				var TS = await GetAsync();
				if (TS?.EntitySet == null)
				{
					TargetEntitySet.EntitySet = new List<T>();
				}
				else
				{
					TargetEntitySet = TS;
				}
				TargetEntitySet.IsDirty =false;
			}

			List<T> Entities = null;
			int indx = -1;
			if ( TargetEntitySet.IsDirty)
			{
				SerializeEntitySetAsync();
			}
			Entities = (TargetEntitySet.EntitySet as List<T>) ?? new List<T>();
			indx = Entities.FindIndex(e => e.EntityId == id);
			if (indx <= -1)
			{
				return null;
				
			}
			return Entities[indx];
		}

		public T Get(int id)
		{
			if (id < 0)
			{
				return null;
			}

			if (TargetEntitySet.EntitySet == null)
			{
				var TS =  Get();
				if (TS?.EntitySet == null)
				{
					TargetEntitySet.EntitySet = new List<T>();
				}
				else
				{
					TargetEntitySet = TS;
				}
				TargetEntitySet.IsDirty = false;
			}

			List<T> Entities = null;
			int indx = -1;
			if (TargetEntitySet.IsDirty)
			{
				SerializeEntitySetAsync();
			}
			Entities = (TargetEntitySet.EntitySet as List<T>) ?? new List<T>();
			indx = Entities.FindIndex(e => e.EntityId == id);
			if (indx <= -1)
			{
				return null;

			}
			return Entities[indx];
		}

		public virtual async void RemoveAsync(T entity)
		{
			if (TargetEntitySet?.EntitySet == null)
				return ;
			List<T> Entities = (TargetEntitySet.EntitySet as List<T>) ?? new List<T>();
			int indx = Entities.FindIndex(e => e.EntityId == entity.EntityId);
			if (indx > -1)
			{
				Entities.RemoveAt(indx);
				SerializeEntitySetAsync();
			}
		}

		public void Remove(T entity)
		{
			if (TargetEntitySet?.EntitySet == null)
				return;
			List<T> Entities = (TargetEntitySet.EntitySet as List<T>) ?? new List<T>();
			int indx = Entities.FindIndex(e => e.EntityId == entity.EntityId);
			if (indx > -1)
			{
				Entities.RemoveAt(indx);
				SerializeEntitySet();
			}
		}

		public virtual async void RemoveAsync(int id)
		{
			if (TargetEntitySet?.EntitySet == null)
				return ;
			List<T> Entities = (TargetEntitySet.EntitySet as List<T>) ?? new List<T>();
			int indx = Entities.FindIndex(e => e.EntityId == id);
			if (indx > -1)
			{
				Entities.RemoveAt(indx);
				SerializeEntitySetAsync();
			}
		}

		public void Remove(int id)
		{
			if (TargetEntitySet?.EntitySet == null)
				return;
			List<T> Entities = (TargetEntitySet.EntitySet as List<T>) ?? new List<T>();
			int indx = Entities.FindIndex(e => e.EntityId == id);
			if (indx > -1)
			{
				Entities.RemoveAt(indx);
				SerializeEntitySet();
			}
		}

		public virtual async Task<T> UpdateAsync(T entity)
		{
			T retEntity = null;
			if (TargetEntitySet?.EntitySet == null)
				return null;
			List<T> Entities = (TargetEntitySet.EntitySet as List<T>) ?? new List<T>();
			int indx = Entities.FindIndex(e => e.EntityId == entity.EntityId);
			if (indx > -1)
			{
				Entities.RemoveAt(indx);
				Entities.Add(entity);
				SerializeEntitySet();
				retEntity = entity;
			}
			else
			{
				retEntity = await AddAsync(entity);
			}
			return  retEntity;
		}
		public virtual T Update(T entity)
		{
			T retEntity = null;
			if (TargetEntitySet?.EntitySet == null)
				return null;
			List<T> Entities = (TargetEntitySet.EntitySet as List<T>) ?? new List<T>();
			int indx = Entities.FindIndex(e => e.EntityId == entity.EntityId);
			if (indx > -1)
			{
				Entities.RemoveAt(indx);
				Entities.Add(entity);
				SerializeEntitySet();
				retEntity = entity;
			}
			else
			{
				retEntity =  Add(entity);
			}
			return retEntity;
		}


		#endregion

		#region Methods

		protected abstract void DeserializeEntitySet();
		protected abstract void DeserializeEntitySetAsync();
		protected async void SerializeEntitySetAsync()
		{
			string jsonString = JsonConvert.SerializeObject(TargetEntitySet, Formatting.Indented);
			bool isGood = await JsonFileSupport.WriteFileAsync(TargetEntitySet.PathName, jsonString);
			TargetEntitySet.IsDirty = !isGood;
		}
		protected void SerializeEntitySet()
		{
			string jsonString = JsonConvert.SerializeObject(TargetEntitySet, Formatting.Indented);
			bool isGood =  JsonFileSupport.WriteFile(TargetEntitySet.PathName, jsonString);
			TargetEntitySet.IsDirty = !isGood;
		}
		#endregion
	}
}
