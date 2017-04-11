using System;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;
using Newtonsoft.Json;

namespace ProjectWatch.Entities
{
//	OnPropertyChanged(() => _billingId);
	[DataContract]
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class Phase : ClientEntityBase, IIdentifiableEntity, ICloneable
	{
		#region Factory Method

		public Phase()
		{
			
		}

		public Phase(int projectId) 
			:this(projectId,-1)
		{
		}

		public Phase(int projectId, int phaseId)
		{
			ProjectId = projectId;
			PhaseId = phaseId;
		}
		#endregion

		#region Primitive Properties

		[DataMember]
		public int PhaseId { get; set; }

		[DataMember]
		public int ProjectId { get; set; }

		[DataMember]
		public string PhaseName { get; set; }

		[DataMember]
		public bool IsBillable { get; set; }

		[DataMember]
		public DateTime DueDate { get; set; }

		[DataMember]
		public double HourQuote { get; set; }

		[DataMember]
		public double TimeQuote { get; set; }

		[DataMember]
		public double Rate { get; set; }

		[DataMember]
		public string Note { get; set; }

		public bool HasChild { get; set; }
		public bool HasParent { get; set; }


		#endregion

		#region Contract_Implementations
		[JsonIgnore]
		public override int EntityId
		{
			get { return PhaseId; }
			set { PhaseId = value; }
		}
		public object Clone()
		{
			Phase p = new Phase();
			p.IsBillable = IsBillable;
			p.HasChild = HasChild;
			p.HasParent = HasParent;
			p.DueDate = DueDate;
			p.HourQuote = HourQuote;
			p.Note = Note;
			p.PhaseId = PhaseId;
			p.PhaseName = PhaseName;
			p.ProjectId = ProjectId;
			p.Rate = Rate;
			p.TimeQuote = TimeQuote;

			return p;
		}
		#endregion


		#region Overrides
		public override string ToString()
		{
			return PhaseName;
		}
		#endregion
	}
}
