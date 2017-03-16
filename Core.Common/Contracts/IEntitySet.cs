using System;
using  Core.Common.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Data;

namespace Core.Common.Contracts
{
	public interface IEntitySet<T>
		where T : ClientEntityBase, new()
	{
		string PathName { get; }
		IEnumerable<T> GetEntitySet<T>();
	}
}
