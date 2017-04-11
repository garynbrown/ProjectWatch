using System;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;
using Newtonsoft.Json;

namespace ProjectWatch.Entities
{
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class Contact : ClientEntityBase, IIdentifiableEntity, ICloneable
	{
		#region Constructors
		public Contact()
		{
			
		}
		public Contact(int companyId) : this(companyId,-1)
		{
		}
			
		public Contact(int companyId, int contactId)
		{
			CompanyId = companyId;
			ContactId = contactId;
		}
		#endregion

		#region Factory Method

		/// <summary>
		/// Create a new Customer object.
		/// </summary>
		/// <param name="customer_ID">Initial value of the Customer_ID property.</param>
		public static Contact CreateContact(int contact_ID)
		{
			return new Contact() { ContactId = contact_ID };
		}

		#endregion

		#region Primitive Properties

		private bool _isProjectContact;
		private bool _isBillingContact;

		[DataMember]
		public int ContactId { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public int CompanyId { get; set; }

		public string Street1 { get; set; }


		public string Street2 { get; set; }

		public string City { get; set; }


		public string ZipCode { get; set; }


		public string Phone1 { get; set; }


		public string Phone2 { get; set; }


		public string Email { get; set; }

		public string State { get; set; }


		public string Note { get; set; }

		#endregion

		#region Contract_Implementations

		public object Clone()
		{
			Contact c = new Contact();
			c.FirstName = FirstName;
			c.LastName = LastName;
			c.Email = Email;
			c.ContactId = ContactId;
			c.CompanyId = CompanyId;
			c.Phone1 = Phone1;
			c.Phone2 = Phone2;
			c.Street1 = Street1;
			c.Street2 = Street2;
			c.City = City;
			c.State = State;
			c.Note = Note;
			c.ZipCode = ZipCode;

			return c;
		}
		[JsonIgnore]
		public override int EntityId
		{
			get { return ContactId; }
			set { ContactId = value; }
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"{LastName}, {FirstName}";
		}
		#endregion
	}
}

