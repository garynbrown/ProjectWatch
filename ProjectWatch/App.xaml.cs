using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Core.Common.Core;
using ProjectWatch.Client.Bootstrapper;
using ProjectWatch.Support;

namespace ProjectWatch
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			ClientEntityBase.Container = MEFLoader.Init(new List<ComposablePartCatalog>()
			{
				new AssemblyCatalog(Assembly.GetExecutingAssembly())
			});
			PreferenceSettings pSettings = ClientEntityBase.Container.GetExportedValue<PreferenceSettings>();
			PreferenceManager.LoadConfiguration( ref pSettings);
		}
	}
}
