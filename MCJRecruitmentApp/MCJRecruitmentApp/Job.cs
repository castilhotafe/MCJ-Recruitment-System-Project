using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCJRecruitmentApp
{
    public class Job
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public bool Completed { get; set; }
        public Contractor ContractorAssigned { get; set; }

        public Job(string title, DateTime date, decimal cost)
        {
            Title = title;
            Date = date;
            Cost = cost;
            Completed = false;
            ContractorAssigned = null;
        }

        public override string ToString()
        {
            string status;
            if (!(Completed))
            {
                status = "Pending";
            }
            else
            {
                status = "Completed";
            }

            string contractorName;
            if (ContractorAssigned == null)
            {
                contractorName = "Unassigned";
            }
            else
            {
                contractorName = $"{ContractorAssigned.FirstName} {ContractorAssigned.LastName}";
            }

            return $"{Title} - {Date.ToShortDateString()} - ${Cost:F2} - {status} - {contractorName}";
        }
    }
}
