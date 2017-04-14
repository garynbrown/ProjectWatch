using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace ProjectWatch.ViewModel
{
	class AboutViewModel
	{
		public string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();
		public string AssembyName => Assembly.GetExecutingAssembly().GetName().Name;
		public RelayCommand ShowHelpCommand { get; set; }

		void OnShowHelp()
		{
			string currentPath = Directory.GetCurrentDirectory();
			Process[] plist = System.Diagnostics.Process.GetProcesses();
			string title;
			string name;
			bool isRunning = false;
			foreach (var _process in plist)
			{
				title = _process.MainWindowTitle;
				name = _process.ProcessName;
				if (name.Contains("hh") && title.Contains("Project"))
				{
					isRunning = true;
					break;
				}
			}
			if (!isRunning)
			{
#if DEBUG
				Process.Start(@"..\..\ProjectWatch.chm");
#else
				Process.Start(Path.Combine(currentPath,"ProjectWatch.chm"));
#endif
			}

		}

		public RelayCommand<string> GotoUriCommand { get; set; }

		void OnGotoUri(string uri)
		{
			Uri targetUri = new Uri(uri);
			Process.Start(new ProcessStartInfo(targetUri.AbsoluteUri));
		}

		public RelayCommand<string> EmailMeCommand { get; set; }

		void OnEmailMe(string email)
		{
			Uri targetUri = new Uri(email);
			Process.Start(targetUri.AbsoluteUri);
		}

		public AboutViewModel()
		{
			ShowHelpCommand = new RelayCommand(OnShowHelp);
			GotoUriCommand = new RelayCommand<string>(OnGotoUri);
			EmailMeCommand = new RelayCommand<string>(OnEmailMe);
		}
	}
}
