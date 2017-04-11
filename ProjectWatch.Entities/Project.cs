using System;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;
using Newtonsoft.Json;


namespace ProjectWatch.Entities
{
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class Project : ClientEntityBase, IIdentifiableEntity, ICloneable
	{
		#region Factory Method

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

		public int ProjectId { get; set; }

		public string Name { get; set; }

		public double TimeQuote { get; set; }

		public bool IsBillable { get; set; }
		public double CostQuote { get; set; }

		public string Note { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public int BillingContactId { get; set; }

		public int ManagementContactId { get; set; }

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
