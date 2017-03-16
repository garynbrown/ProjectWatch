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
	        //_employees = new List<Contact>();
	        //if (DataUtils.LCust != null)
	        //{
	        //    foreach (Customer cust in DataUtils.LCust)
	        //    {
	        //        if (cust.Company == CompanyName)
	        //        {
	        //            if (LEmployees == null) LEmployees = new List<Customer>();
	        //            LEmployees.Add(cust);
	        //        }
	        //    }
	        //}
        }
		public static Company CreateCompany(global::System.Int32 companyId)
		{
			Company company = new Company();
			company._companyId = companyId;
			return company;
		}
		#endregion

		#region Properties
		public int CompanyId
		{
			get { return _companyId; }
			set
			{
				_companyId = value;
			}
		}
		private int _companyId;
	    public string CompanyName
	    {
		    get { return _companyName; }
		    set { _companyName = value; }
	    }
		private string _companyName;
	    //public List<Contact> Employees
	    //{
		   // get { return _employees; }
		   // set { _employees = value; }
	    //}

		[JsonIgnore]
	    public string PathName => "Company";
		#endregion
		
		#region Fields
		//private List<Contact> _employees;
		#endregion
        #region Methods
   //     public Contact getEmployeebyID(int clntID)
   //     {
			//// todo change this to Linq or Lambda
			//Contact retCust = new Contact();
   //         foreach (Contact clnt in _employees)
   //         {
   //             if (clnt.ContactId == clntID)
   //             {
   //                 retCust = clnt;
   //                 break;
   //             }
   //         }
   //         return retCust;
   //     }
   //     public Contact getEmployeeByName(string First, string Last)
   //     {
			//// todo change this to Linq or Lambda
			//Contact retClnt = new Contact();
   //         foreach (Contact cust in _employees)
   //         {
   //             if ((cust.LastName == Last)&&(cust.FirstName == First))
   //             {
			//		retClnt = cust;
   //                 break;
   //             }
   //          }
   //         return retClnt;
   //     }
        #endregion

		#region Contract_Implementations
		[JsonIgnore]
		public override int EntityId
		{
			get { return CompanyId; }
			set { CompanyId = value; }
		}


	    #endregion

	    public object Clone()
	    {
		    Company c = new Company();
		    c.CompanyId = _companyId;
		    c.CompanyName = _companyName;
		    return c;
	    }

	    public override string ToString()
	    {
		    return _companyName;
	    }
    }
}
