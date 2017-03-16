using System;
using System.Collections.Generic;
using Core.Common.Contracts;
using Core.Common.Core;
using Newtonsoft.Json;

namespace Core.Common.Data
{
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public abstract class EntitySetBase
	{
		[JsonIgnore]
		public ClientEntityBase EntityBase;
		
	}

	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public abstract class EntitySetBase<T> : EntitySetBase, IEntitySet<T>
		where T : ClientEntityBase, new()
	{
		[JsonIgnore]
		public abstract string PathName { get; }
		public IEnumerable<T> EntitySet ;
		public abstract IEnumerable<T> GetEntitySet<T>();

		[JsonIgnore]
		public bool IsDirty { get; set; }
	}
}
