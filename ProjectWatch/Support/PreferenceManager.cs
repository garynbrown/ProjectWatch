using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjectWatch.Support
{
	public static class PreferenceManager
	{
		//  https://msdn.microsoft.com/en-us/library/system.environment.specialfolder(v=vs.110).aspx 
#if DEBUG
		public static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ProjectWatch\Test\Data\ProjectWatch.config");
#else
		public static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ProjectWatch\Data\ProjectWatch.config");
#endif
#if OldCode
		public static bool ProVersion = false;
		public static int LastProject = -1;
		public static int LastPhase = -1;
		public static DateTime LastBillingDate = new DateTime();
#endif
		private static string _configString = string.Empty;

		public static void LoadConfiguration(ref PreferenceSettings PSettings)
		{
			if (!File.Exists(ConfigPath))
			{
				PSettings = new PreferenceSettings();
				return;
			}
			using (TextReader reader = new StreamReader(ConfigPath))
			{
				_configString = reader.ReadToEnd();
				PSettings = JsonConvert.DeserializeObject<PreferenceSettings>(_configString);
			}
		}

		public static void saveConfiguration(PreferenceSettings PSettings)
		{
			if (!Directory.Exists(Path.GetDirectoryName(ConfigPath)))
			{
				string dir = Path.GetDirectoryName(ConfigPath);
				Directory.CreateDirectory(dir);
			}
			JsonSerializer Jserializer = new JsonSerializer();
			Jserializer.Formatting = Formatting.Indented;
			using (StreamWriter writer = new StreamWriter(ConfigPath))
			{
				using (JsonWriter Jwriter = new JsonTextWriter(writer))
				{
					Jserializer.Serialize(Jwriter, PSettings);
				}
			}

		}
	}
}
