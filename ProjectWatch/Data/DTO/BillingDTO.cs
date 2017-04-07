using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DTO
{
	public class BillingDTO
	{
		public Billing Bill;
		private string _amountBilledDto;
		private List<Company> _companies;
		private List<Project> _projects;
		private List<Phase> _phases;
		private string _companyName;
		private string _projectName;
		private string _PhaseName;
		private string _dateBilledDto;
		private string _hoursBilledDto;
		private string _companyNameDto;
		private string _projectNameDto;
		private string _phaseNameDto;


		public BillingDTO(Billing billing, List<Company> companies, List<Project> projects, List<Phase> phases)
		{
			_companies = companies ?? new List<Company>();
			_projects = projects ?? new List<Project>();
			_phases = phases ?? new List<Phase>();
			Bill = billing.Clone() as Billing;
			MakeDto();
		}

		public string AmountBilledDto
		{
			get { return _amountBilledDto; }
			set { _amountBilledDto = value; }
		}

		public string DateBilledDto
		{
			get { return _dateBilledDto; }
			set { _dateBilledDto = value; }
		}

		public string HoursBilledDto
		{
			get { return _hoursBilledDto; }
			set { _hoursBilledDto = value; }
		}

		public string CompanyNameDto
		{
			get { return _companyNameDto; }
			set { _companyNameDto = value; }
		}

		public string ProjectNameDto
		{
			get { return _projectNameDto; }
			set { _projectNameDto = value; }
		}

		public string PhaseNameDto
		{
			get { return _phaseNameDto; }
			set { _phaseNameDto = value; }
		}

		void MakeDto()
		{
			HoursBilledDto = Bill.Hours_Billed > 0 ? $"{Bill.Hours_Billed:D.##}" : "";
			DateBilledDto = Bill.DateBilled > DateTime.MinValue ? $"{Bill.DateBilled:d}": "";
			Project p = Bill.ProjectId > -1 ? _projects.Find(r => r.ProjectId == Bill.ProjectId) : null;
			ProjectNameDto = p != null ? p.Name : "";
			CompanyNameDto = p != null && p.CompnayId > -1 ? _companies.Find(c => c.CompanyId == p.CompnayId).CompanyName : "";
			PhaseNameDto = p != null && Bill.PhaseId > -1 ? _phases.Find(s => s.PhaseId == Bill.PhaseId).PhaseName : "";
			AmountBilledDto = Bill.AmountBilled > 0 ? $"{Bill.AmountBilled:C}" : "";

		}
	}
}
