using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;

namespace ProjectWatch.Entities
{
	public class Customer : ClientEntityBase, IIdentifiableEntity
	{
		#region Factory Method

		/// <summary>
		/// Create a new Customer object.
		/// </summary>
		/// <param name="customer_ID">Initial value of the Customer_ID property.</param>
		public static Customer CreateCustomer(int customer_ID)
		{
			return new Customer() { _customerId = customer_ID };
		}

		#endregion

		#region Primitive Properties
		private string _note;
		private string _state;
		private string _email;
		private string _phone2;
		private string _phone1;
		private string _zipCode;
		private string _city;
		private string _street2;
		private string _streete1;
		private int _customerId;
		private string _firstName;
		private string _lastName;
		private int _companyId;

		[DataMember]
		public int CustomerId
		{
			get { return _customerId; }
			set {
				if (_customerId != value)
				{
					_customerId = value;

				}
			}
		}

		public string FirstName
		{
			get { return _firstName; }
			set
			{
				_firstName = value;
			}
		}

		public string LastName
		{
			get { return _lastName; }
			set
			{
				_lastName = value;
			}
		}

		public int CompanyId
		{
			get { return _companyId; }
			set
			{
				_companyId = value;
			}
		}

		///// <summary>
		///// No Metadata Documentation available.
		///// </summary>
		//[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		//[DataMemberAttribute()]
		//public global::System.String Company
		//{
		//	get
		//	{
		//		return _Company;
		//	}
		//	set
		//	{
		//		OnCompanyChanging(value);
		//		ReportPropertyChanging("Company");
		//		_Company = StructuralObject.SetValidValue(value, true);
		//		ReportPropertyChanged("Company");
		//		OnCompanyChanged();
		//	}
		//}
		//private global::System.String _Company;
		//partial void OnCompanyChanging(global::System.String value);
		//partial void OnCompanyChanged();

		public string Streete1
		{
			get { return _streete1; }
			set
			{
				_streete1 = value;
			}
		}
		

		public string Street2
		{
			get { return _street2; }
			set
			{
				_street2 = value;
			}
		}

		public string City
		{
			get { return _city; }
			set
			{
				_city = value;
			}
		}


		public string ZipCode
		{
			get { return _zipCode; }
			set
			{
				_zipCode = value;
			}
		}


		public string Phone1
		{
			get { return _phone1; }
			set
			{
				_phone1 = value;
			}
		}


		public string Phone2
		{
			get { return _phone2; }
			set
			{
				_phone2 = value;
			}
		}


		public string Email
		{
			get { return _email; }
			set
			{
				_email = value;
			}
		}

		public string State
		{
			get { return _state; }
			set
			{
				_state = value;
			}
		}


		public string Note
		{
			get { return _note; }
			set
			{
				_note = value;
			}
		}

		#endregion

		public int EntityId
		{
			get {return CustomerId; }
			set { CustomerId = value; }
		}
	}
}
