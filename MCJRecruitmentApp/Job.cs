using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCJRecruitmentApp
{
    /// <summary>
    /// Represents a job in the recruitment application.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Gets or sets the title of the job.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the date when the job is scheduled.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Gets or sets the cost associated with the job.
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the job is completed.
        /// </summary>
        public bool Completed { get; set; }
        /// <summary>
        /// Gets or sets the contractor assigned to the job.
        /// </summary>
        public Contractor? ContractorAssigned { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Job"/> class with specified details.
        /// </summary>
        /// <param name="title">The job's title.</param>
        /// <param name="date">The job's schedule date.</param>
        /// <param name="cost">The job's cost.</param>
        public Job(string title, DateTime date, decimal cost)
        {
            Title = title;
            Date = date;
            Cost = cost;
            Completed = false;
            ContractorAssigned = null;
        }


        /// <summary>
        /// Returns a string representation of the job's details.
        /// </summary>
        /// <returns>
        /// A string that includes the job's title, date, cost formatted as currency, status (Pending or Completed), 
        /// and the contractor's name (or "Unassigned" if no contractor is assigned).
        /// </returns>
        public override string ToString()
        {
            string status; // Determine the status of the job based on the Completed property
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
