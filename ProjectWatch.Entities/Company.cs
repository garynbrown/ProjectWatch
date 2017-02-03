using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;

namespace ProjectWatch.Entities
{
	public class Company : ClientEntityBase, IIdentifiableEntity
    {
		public static Company CreateCompany(global::System.Int32 companyId)
		{
			Company company = new Company();
			company._companyId = companyId;
			return company;
		}
		public Company()
	    {
		    
	    }

		public int CompanyId
		{
			get { return _companyId; }
			set
			{
				_companyId = value;
			}
		}
		
		private int _companyId;
		public string CompanyName;
		//        public int CompanyID;
		private List<Contact> LEmployees;
		public Company(string CName)
        {
            CompanyName = CName;
            LEmployees = new List<Contact>();
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
        public Contact getEmployeebyID(int clntID)
        {
			Contact retCust = new Contact();
            foreach (Contact clnt in LEmployees)
            {
                if (clnt.ContactId == clntID)
                {
                    retCust = clnt;
                    break;
                }
            }
            return retCust;
        }
        public Contact getEmployeeByName(string First, string Last)
        {
			Contact retClnt = new Contact();
            foreach (Contact cust in LEmployees)
            {
                if ((cust.LastName == Last)&&(cust.FirstName == First))
                {
					retClnt = cust;
                    break;
                }
             }
            return retClnt;
        }

		public override int EntityId
		{
			get { return CompanyId; }
			set { CompanyId = value; }
		}

	    public string PathName => "Company";
	}
}
