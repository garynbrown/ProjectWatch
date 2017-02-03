using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
	public interface IEntitySet<T>
		where T : class, ClientEntityBase<T>
	{
		IEnumerable<T> GetEntitySet<T>();
	}
}
