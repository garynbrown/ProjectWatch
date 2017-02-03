using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Common.Core;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectWatch.Data.DataRepositories;
using ProjectWatch.Data.DataSets;
using ProjectWatch.Entities;
using ProjectWatch.Support;

namespace PWTest
{
	public class testObject
		{
			public string LastProject = "PW";
			public int NumberOfThings = 42;
			public string Path = @"C:\Program Files";
		}
	[TestClass]
	public class JasonRW
	{

		[TestMethod]
		public void WriteJsonTest()
		{
			var basePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			basePath = Path.Combine(basePath, "test.json");
			JsonSerializer Jserializer = new JsonSerializer();
			testObject t = new testObject();
			t.Path = basePath;
			Jserializer.Formatting = Formatting.Indented;
			//			TextReader reader = new StreamReader(basePath);
			using (StreamWriter writer = new StreamWriter(basePath))
			{
				using (JsonWriter Jwriter = new JsonTextWriter(writer))
				{
					Jserializer.Serialize(Jwriter, t);
				}
			}
			using (TextReader reader = new StreamReader(basePath))
			{
				//using (JsonReader JsonReader = new JsonTextReader( reader))
				//{
					t.Path = "empty";
					string stringInput = reader.ReadToEnd();
//					Jserializer.Deserialize<testObject>(stringInput);
					t = JsonConvert.DeserializeObject<testObject>(stringInput);
				//}
			}
			File.Delete(basePath);
			Assert.AreEqual(basePath,t.Path);
		}

		[TestMethod]
		public void ConfigTest()
		{
			int psLastProjectTest = 0;
			//PreferenceSettings ps = ClientEntityBase.Container.GetExportedValue<PreferenceSettings>();
			PreferenceSettings ps = new PreferenceSettings();
			if (File.Exists(PreferenceManager.ConfigPath))
			{
				PreferenceManager.LoadConfiguration(ref ps);
				psLastProjectTest = ps.LastProject;
			}
			else
			{
				ps.LastProject = psLastProjectTest;				
			}
			PreferenceManager.saveConfiguration(ps);
			ps.LastProject = 99;
			PreferenceManager.LoadConfiguration(ref ps);
			Assert.AreEqual(psLastProjectTest, ps.LastProject);
		}

		[TestMethod]
		public void AddProjectTest()
		{

			ProjectSet PS = new ProjectSet() ;
			Project p1 = new Project() {ProjectId = 1,Name="Project1"};
			Project p2 = new Project() { ProjectId = 2, Name = "Project2" };
			PS.EntitySet = new List<Project>();
			List<Project> projects = PS.EntitySet as List<Project>;
//			projects.Add(p1);
			projects.Add(p2);
			ProjectRepository PR = new ProjectRepository(PS);
			Project testSave = PR.AddAsync(p1).Result;
			Assert.IsTrue(PR.GetAsync().Result.EntitySet.Count() ==2);
		}

		[TestMethod]
		public async void LoadProjectTest()
		{
			ProjectSet PS = new ProjectSet() ;
			ProjectRepository PR = new ProjectRepository(PS);
			var DS = await PR.GetAsync();
			DS = await PR.GetAsync();
			Assert.AreEqual(PS.PathName,DS.PathName);
		}

		[TestMethod]
		public  void AddTimeCard()
		{
			TimeCard tc = new TimeCard();
			TimeCardSet tcs = new TimeCardSet();
			tcs.EntitySet = new List<TimeCard>();
			List<TimeCard> TimeCards = tcs.EntitySet as List<TimeCard>;
			TimeCardRepository tcr = new TimeCardRepository(tcs);
			var DS = tcr.AddAsync(tc);
		}
	}
}
