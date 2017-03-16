using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Core;
using Core.Common.Data;
using Newtonsoft.Json;

namespace Core.Common.Utils
{
	public static class JsonFileSupport
	{
		public static readonly string DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ProjectWatch\Data");
		public static string JsonSerializeSetOfType<T>(T entitySetModel)
		{
			return JsonConvert.SerializeObject(entitySetModel, Formatting.Indented);
		}
		public static string JsonSerializeObjectOfType<T>(T entityModel)
		{
			return JsonConvert.SerializeObject(entityModel, Formatting.Indented);
		}

		public static T JsonDeserializeSetOfType<T>(string jsonContent)
			where T: EntitySetBase
		{
			T TargetEntitySet = ClientEntityBase.Container.GetExportedValue<T>();
			if (TargetEntitySet == null)
				return null;
			return JsonConvert.DeserializeObject<T>(jsonContent);
		}

		public static T JsonDeseializeObjectOfType<T>(string jsonContent)
			where T : ClientEntityBase
		{
			return JsonConvert.DeserializeObject<T>(jsonContent);
		}
		public static  bool WriteFile(string filePath, string jsonContent)
		{
			bool retValue = true;
			string location = Path.Combine(DataPath, filePath + ".json");
			try
			{

				if (!Directory.Exists(Path.GetDirectoryName(location)))
				{
					string dir = Path.GetDirectoryName(location);
					Directory.CreateDirectory(dir);
				}
				StringBuilder sb = new StringBuilder(jsonContent);
				using (StreamWriter writer = new StreamWriter(location))
				{

					 writer.Write(jsonContent);
				}
				//File.WriteAllText(location, jsonContent);
			}
			catch (Exception ex)
			{
				return false;
			}
			return retValue;
		}

		public static async Task<bool> WriteFileAsync(string filePath, string jsonContent)
		{
			bool retValue = true;
			string location = Path.Combine(DataPath, filePath + ".json");
			try
			{

				if (!Directory.Exists(Path.GetDirectoryName(location)))
				{
					string dir = Path.GetDirectoryName(location);
					Directory.CreateDirectory(dir);
				}
				StringBuilder sb = new StringBuilder(jsonContent);
				using ( StreamWriter writer = new StreamWriter(location))
				{

					await writer.WriteAsync(jsonContent);
				}
				//File.WriteAllText(location, jsonContent);
			}
			catch (Exception ex)
			{
				return false;
			}
			return retValue;
		}

		public static async Task<string> JsonReadFileAsync(string filePath)
		{
			string retValue = string.Empty;
			string inString;
			string location = Path.Combine(DataPath, filePath + ".json");
			if (!File.Exists(location))
			{
				return retValue;
			}

			try
			{
				using (StreamReader reader = File.OpenText(location))
				{
					//retValue = reader.ReadToEnd();
					  retValue = await reader.ReadToEndAsync();
					//retValue = await Task.Run(() => reader.ReadToEnd());
				}
			}
			catch (Exception ex)
			{
				return string.Empty;
			}
			return retValue;
		}
		public static string JsonReadFile(string filePath)
		{
			string retValue = string.Empty;
			string inString;
			string location = Path.Combine(DataPath, filePath + ".json");
			if (!File.Exists(location))
			{
				return retValue;
			}

			try
			{
				//StringReader reader = new StringReader(location);
				//Task<string> taskstring = reader.ReadToEndAsync();
				retValue = File.ReadAllText(location);
			}
			catch (Exception ex)
			{
				return string.Empty;
			}
			//JsonSerializer Jserializer = new JsonSerializer();
			//Jserializer.Formatting = Formatting.Indented;

			//using (StreamWriter writer = new StreamWriter(filePath))
			//{
			//	using (JsonWriter Jwriter = new JsonTextWriter(writer))
			//	{
			//		Jserializer.Serialize(Jwriter, PSettings);
			//	}
			//}
			return retValue;

		}
	}
}
