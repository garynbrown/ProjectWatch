using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWatch.Client.Bootstrapper
{
    public class MEFLoader
    {
		public static CompositionContainer Init()
		{
			return Init(null);
		}

		public static CompositionContainer Init(ICollection<ComposablePartCatalog> CatalogParts)
		{
			AggregateCatalog catalog = new AggregateCatalog();
			//catalog.Catalogs.Add(new AssemblyCatalog(typeof(MainWindowViewModel).Assembly));

			if (CatalogParts != null)
			{
				foreach (var part in CatalogParts)
				{
					catalog.Catalogs.Add(part);
				}
			}
			CompositionContainer container = new CompositionContainer(catalog);
			return container;
		}
	}
}
