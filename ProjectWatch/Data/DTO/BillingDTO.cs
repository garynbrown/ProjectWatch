using System;
using System.Collections.Generic;
using ProjectWatch.Entities;

namespace ProjectWatch.Data.DTO
{
	public class BillingDTO
	{
		public Billing Bill;
		private List<Company> _companies;
		private List<Project> _projects;
		private List<Phase> _phases;


		public BillingDTO(Billing billing, List<Company> companies, List<Project> projects, List<Phase> phases)
		{
			_companies = companies ?? new List<Company>();
			_projects = projects ?? new List<Project>();
			_phases = phases ?? new List<Phase>();
			Bill = billing.Clone() as Billing;
			MakeDto();
		}

		public string AmountBilledDto { get; set; }

		public string DateBilledDto { get; set; }

		public string HoursBilledDto { get; set; }

		public string CompanyNameDto { get; set; }

		public string ProjectNameDto { get; set; }

		public string PhaseNameDto { get; set; }

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
