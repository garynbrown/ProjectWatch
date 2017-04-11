using  Core.Common.Core;
using System.Collections.Generic;

namespace Core.Common.Contracts
{
	public interface IEntitySet<T>
		where T : ClientEntityBase, new()
	{
		string PathName { get; }
		IEnumerable<T> GetEntitySet<T>();
	}
}
