using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;
using Newtonsoft.Json;

namespace ProjectWatch.Entities
{
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class Contact : ClientEntityBase, IIdentifiableEntity, ICloneable
	{
		public Contact()
		{
			
		}
		public Contact(int companyId) : this(companyId,-1)
		{
			
		}

		public Contact(int companyId, int contactId)
		{
			_companyId = companyId;
			_contactId = contactId;
		}
		#region Factory Method

		/// <summary>
		/// Create a new Customer object.
		/// </summary>
		/// <param name="customer_ID">Initial value of the Customer_ID property.</param>
		public static Contact CreateContact(int contact_ID)
		{
			return new Contact() { _contactId = contact_ID };
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
		private string _street1;
		private int _contactId;
		private string _firstName;
		private string _lastName;
		private int _companyId;
		private bool _isProjectContact;
		private bool _isBillingContact;

		[DataMember]
		public int ContactId
		{
			get { return _contactId; }
			set
			{
				if (_contactId != value)
				{
					_contactId = value;

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

		public string Street1
		{
			get { return _street1; }
			set
			{
				_street1 = value;
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

		#region Contract_Implementations

		public object Clone()
		{
			Contact c = new Contact();
			c.FirstName = _firstName;
			c.LastName = _lastName;
			c.Email = _email;
			c.ContactId = _contactId;
			c.CompanyId = _companyId;
			c.Phone1 = _phone1;
			c.Phone2 = _phone2;
			c.Street1 = _street1;
			c.Street2 = Street2;
			c.City = _city;
			c.State = _state;
			c.Note = _note;
			c.ZipCode = _zipCode;

			return c;
		}
		[JsonIgnore]
		public override int EntityId
		{
			get { return ContactId; }
			set { ContactId = value; }
		}
		#endregion

		public override string ToString()
		{
			return $"{LastName}, {FirstName}";
		}
	}
}

