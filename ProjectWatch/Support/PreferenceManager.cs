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
		public static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"ProjectWatch\Config\ProjectWatch.config");
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
				return;
			using (TextReader reader = new StreamReader(ConfigPath))
			{
				_configString = reader.ReadToEnd();
				PSettings = JsonConvert.DeserializeObject<PreferenceSettings>(_configString);
#if OldCode
//JsonTextReader JReader = new JsonTextReader( new StringReader(ConfigString));
//while (JReader.Read())
//{
//	if (JReader.Value != null)
//	{
//		if (JReader.TokenType.ToString() == "PropertyName")
//		{
//			switch (JReader.Value.ToString())
//			{
//				case ("ProVersion"):
//				{
//					JReader.Read();
//					ProVersion = (bool) JReader.Value;
//						break;
//				}
//				case ("LastProject"):
//				{
//					JReader.Read();
//					LastProject = (int)JReader.Value;
//					break;
//				}
//				case ("LastPhase"):
//					{
//						JReader.Read();
//						LastPhase = (int)JReader.Value;
//						break;
//					}
//				case ("LastBillingDate"):
//					{
//						JReader.Read();
//						LastBillingDate = (DateTime)JReader.Value;
//						break;
//					}

//			}
//		}
//	}
//}
#endif
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
