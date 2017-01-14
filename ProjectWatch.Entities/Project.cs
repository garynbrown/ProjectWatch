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
	[DataContract]
	public class Project : ClientEntityBase, IIdentifiableEntity
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

		#endregion

		public override string ToString()
		{
			return _name;
		}

		public int EntityId
		{
			get { return ProjectId; }
			set { ProjectId = value; }
		}

		public string PathName => "Project";
	}
}
