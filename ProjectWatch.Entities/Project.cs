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
	[DataContract]
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class Project : ClientEntityBase, IIdentifiableEntity, ICloneable
	{
		#region Factory Method

		/// <summary>
		/// Create a new Project object.
		/// </summary>
		/// <param name="project_ID">Initial value of the Project_ID property.</param>
		public static Project CreateProject(global::System.Int32 projectId)
		{
			Project project = new Project();
			project.ProjectId = projectId;
			return project;
		}

		public Project(int projectId)
			: this()
		{
			ProjectId = projectId;
		}
		public Project()
		{
			CompnayId = -1;
			BillingContactId = -1;
			StartDate = DateTime.Today;
			ManagementContactId = -1;
		}
		#endregion

		#region Primitive Properties

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public int ProjectId { get; set; }

		//public int LastPhaseId { get; set; }
		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		//[DataMember]
		//public int ContactId
		//{
		//	get
		//	{
		//		return _clientId;
		//	}
		//	set
		//	{
		//		_clientId = value;
		//		RaisePropertyChanged(() => _clientId);
		//	}
		//}
		//private int _clientId;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public string Name { get; set; }

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public double TimeQuote { get; set; }

		[DataMember]
		public bool IsBillable { get; set; }
		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public double CostQuote { get; set; }

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public string Note { get; set; }

		[DataMember]
		public DateTime StartDate { get; set; }

		[DataMember]
		public DateTime EndDate { get; set; }

		[DataMember]
		public int BillingContactId { get; set; }

		[DataMember]
		public int ManagementContactId { get; set; }

		[DataMember]
		public int CompnayId { get; set; }

		public bool HasChild { get; set; }
		public bool HasParent { get; set; }
		public double Rate { get; set; }

		#endregion

		#region Overrides
		public override string ToString()
		{
			return Name;
		}
		#endregion

		#region Contract_Implementations

		public object Clone()
		{
			Project p = new Project();
			p.ProjectId = ProjectId;
			p.IsBillable = IsBillable;
			p.HasChild = HasChild;
			p.HasParent = HasParent;
			//p.LastPhaseId = LastPhaseId;
			p.Rate = Rate;
			p.Name = Name;
			p.Note = Note;
			p.CostQuote = CostQuote;
			p.TimeQuote = TimeQuote;
			p.StartDate = StartDate;
			p.EndDate = EndDate;
			p.BillingContactId = BillingContactId;
			p.ManagementContactId = ManagementContactId;
			p.CompnayId = CompnayId;

			return p;
		}
		[JsonIgnore]
		public override int EntityId
		{
			get { return ProjectId; }
			set { ProjectId = value; }
		}
		#endregion
	}
}
