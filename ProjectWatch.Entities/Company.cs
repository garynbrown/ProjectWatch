using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;
using Newtonsoft.Json;

namespace ProjectWatch.Entities
{
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class Company : ClientEntityBase, IIdentifiableEntity, ICloneable
    {
		#region Constructors

	    public Company(int companyId)
	    {
		    CompanyId = companyId;
	    }
		public Company()
	    {
		    
	    }
		public Company(string CName)
        {
            CompanyName = CName;
	        CompanyId = -1;
        }
		public static Company CreateCompany(global::System.Int32 companyId)
		{
			Company company = new Company();
			company.CompanyId = companyId;
			return company;
		}
		#endregion


		#region Properties
		public int CompanyId { get; set; }

	    public string CompanyName { get; set; }

	    public string Note { get; set; }

	    [JsonIgnore]
	    public string PathName => "Company";
		#endregion
		

		#region Contract_Implementations
		[JsonIgnore]
		public override int EntityId
		{
			get { return CompanyId; }
			set { CompanyId = value; }
		}

	    public object Clone()
	    {
		    Company c = new Company();
		    c.CompanyId = CompanyId;
		    c.CompanyName = CompanyName;
		    c.Note = Note;
		    return c;
	    }

	    #endregion


	    #region Overrides
	    public override string ToString()
	    {
		    return CompanyName;
	    }
	    #endregion
    }
}
