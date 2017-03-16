﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Data;
using Core.Common.Utils;
using Newtonsoft.Json;
using ProjectWatch.Contracts.RepositoryInterfaces;
using ProjectWatch.Data.DataSets;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DataRepositories
{
	[Export(typeof(ICompanyRepository))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class CompanyRepository : DataRepositoryBase<Company>, ICompanyRepository
	{
		#region Constructors
		public CompanyRepository()
		{
			base.TargetEntitySet = ClientEntityBase.Container.GetExportedValue<EntitySetBase<Company>>();
		}

		public CompanyRepository(EntitySetBase<Company> targetEntitySet ) : base(targetEntitySet)
		{
		}
		#endregion
		#region Overrides
		protected override void DeserializeEntitySet()
		{
			string jString = JsonFileSupport.JsonReadFile(TargetEntitySet.PathName);
			var ts = JsonConvert.DeserializeObject<CompanySet>(jString);
			if (ts?.EntitySet == null)
			{
				TargetEntitySet.EntitySet = new List<Company>();
			}
			else
			{
				TargetEntitySet = ts;
			}
			TargetEntitySet.IsDirty = false;
		}

		protected override async void DeserializeEntitySetAsync()
		{
			string jString = await JsonFileSupport.JsonReadFileAsync(TargetEntitySet.PathName);
			var ts = JsonConvert.DeserializeObject<CompanySet>(jString);
			if (ts?.EntitySet == null)
			{
				TargetEntitySet.EntitySet = new List<Company>();
			}
			else
			{
				TargetEntitySet = ts;
			}
			TargetEntitySet.IsDirty = false;
		}
		#endregion
	}
}
