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
			project._projectId = projectId;
			return project;
		}
		public Project(int projectId)
		{
			_projectId = projectId;
		}
		public Project()
		{

		}
		#endregion

		#region Primitive Properties

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public int ProjectId
		{
			get
			{
				return _projectId;
			}
			set
			{
				if (_projectId != value)
				{
					_projectId = value;
					RaisePropertyChanged(() => ProjectId);
				}
			}
		}
		private int _projectId;
		public int LastPhaseId { get; set; }
		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public int ContactId
		{
			get
			{
				return _clientId;
			}
			set
			{
				_clientId = value;
				RaisePropertyChanged(() => _clientId);
			}
		}
		private int _clientId;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				RaisePropertyChanged(() => _name);
			}
		}
		private string _name;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public double TimeQuote
		{
			get
			{
				return _timeQuote;
			}
			set
			{
				_timeQuote = value;
				RaisePropertyChanged(() => _timeQuote);
			}
		}
		private double _timeQuote;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public double CostQuote
		{
			get
			{
				return _costQuote;
			}
			set
			{
				_costQuote = value;
				RaisePropertyChanged(() => _costQuote);
			}
		}
		private double _costQuote;

		/// <summary>
		/// No Metadata Documentation available.
		/// </summary>
		[DataMember]
		public string Note
		{
			get
			{
				return _note;
			}
			set
			{
				_note = value;
				RaisePropertyChanged(() => _note);
			}
		}
		private string _note;
		private DateTime _startDate;
		private DateTime _endDate;

		[DataMember]
		public DateTime StartDate
		{
			get { return _startDate; }
			set { _startDate = value; }
		}

		[DataMember]
		public DateTime EndDate
		{
			get { return _endDate; }
			set { _endDate = value; }
		}
		[DataMember]
		public int BillingContactId { get; set; }

		[DataMember]
		public int ManagementContactId { get; set; }

		[DataMember]
		public int CompnayId { get; set; }
	

		#endregion

		#region Overrides
		public override string ToString()
		{
			return _name;
		}
		#endregion

		#region Contract_Implementations

		public object Clone()
		{
			Project p = new Project();
			p.ProjectId = ProjectId;
			p.LastPhaseId = LastPhaseId;
			p.ContactId = ContactId;
			p._clientId = _clientId;
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
