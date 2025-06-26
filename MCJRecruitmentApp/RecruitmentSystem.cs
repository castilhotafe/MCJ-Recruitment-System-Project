using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MCJRecruitmentApp
{
    //PROPERTIES


    /// <summary>
    /// Handles the core logic of the recruitment system, managing contractors and jobs.
    /// </summary>
    public class RecruitmentSystem
    {
        /// <summary>
        /// Gets the list of contractors in the system.
        /// </summary>
        public List<Contractor> Contractors { get; private set; }

        /// <summary>
        /// Gets the list of jobs in the system.
        /// </summary>
        public List<Job> Jobs { get; private set; }


        //CONSTRUCTOR


        /// <summary>
        /// Initializes a new instance of the <see cref="RecruitmentSystem"/> class with empty contractor and job lists.
        /// </summary>
        public RecruitmentSystem()
        {
            Contractors = new List<Contractor>();
            Jobs = new List<Job>();
        }


        //METHODS


        /// <summary>
        /// Adds a contractor to the list.
        /// </summary>
        public void AddContractor(Contractor contractor)
        {
            Contractors.Add(contractor);
        }

        /// <summary>
        /// Removes a contractor and unassigns them from jobs.
        /// </summary>
        public void RemoveContractor(Contractor contractor)
        {
            foreach (Job job in GetAllJobs())
            {
                if (job.ContractorAssigned == contractor)
                {
                    job.ContractorAssigned = null;
                    job.Completed = false;
                }
            }
            Contractors.Remove(contractor);
        }

        /// <summary>
        /// Returns a copy of all contractors.
        /// </summary>
        public List<Contractor> GetAllContractors()
        {
            return new List<Contractor>(Contractors);
        }

        /// <summary>
        /// Returns contractors not assigned to any job.
        /// </summary>
        public List<Contractor> GetAvailableContractors()
        {
            List<Contractor> availableContractors = new List<Contractor>();

            foreach (Contractor contractorLocal in Contractors)
            {
                bool isAssigned = false;

                foreach (Job jobLocal in Jobs)
                {
                    if (jobLocal.ContractorAssigned == contractorLocal)
                    {
                        isAssigned = true;
                        break;
                    }
                }

                if (!isAssigned)
                {
                    availableContractors.Add(contractorLocal);
                }
            }

            return availableContractors;
        }

        /// <summary>
        /// Adds a job to the list.
        /// </summary>
        public void AddJob(Job job)
        {
            Jobs.Add(job);
        }

        /// <summary>
        /// Assigns a contractor to a job if it's not already assigned.
        /// </summary>
        public bool AssignJob(Job job, Contractor contractor)
        {
            if (job.ContractorAssigned != null)
                return false;

            job.ContractorAssigned = contractor;
            job.Completed = false;
            return true;
        }

        /// <summary>
        /// Completes a job and clears the contractor.
        /// </summary>
        public void CompleteJob(Job job)
        {
            job.Completed = true;
            job.ContractorAssigned = null;
        }

        /// <summary>
        /// Returns all jobs.
        /// </summary>
        public List<Job> GetAllJobs()
        {
            return new List<Job>(Jobs);
        }

        /// <summary>
        /// Returns jobs with no contractor assigned.
        /// </summary>
        public List<Job> GetUnassignedJobs()
        {
            List<Job> unassignedJobs = new List<Job>();

            foreach (Job job in Jobs)
            {
                if (job.ContractorAssigned == null)
                {
                    unassignedJobs.Add(job);
                }
            }

            return unassignedJobs;
        }

        /// <summary>
        /// Returns jobs filtered by cost range.
        /// </summary>
        public List<Job> GetJobsByCost(decimal? minCost, decimal? maxCost)
        {
            List<Job> filteredJobs = new List<Job>();

            foreach (Job job in Jobs)
            {
                bool isAboveMin = !minCost.HasValue || job.Cost >= minCost.Value;
                bool isBelowMax = !maxCost.HasValue || job.Cost <= maxCost.Value;

                if (isAboveMin && isBelowMax)
                {
                    filteredJobs.Add(job);
                }
            }

            return filteredJobs;
        }

        /// <summary>
        /// Verifies Add Contractor inputs.
        /// </summary>
        public string VerifyInputs_AddContractor(string inputFirstName, string inputLastName, string inputStartDateText, string inputHourlyWageText)
        {
            if (string.IsNullOrEmpty(inputFirstName) || string.IsNullOrEmpty(inputLastName) ||
                string.IsNullOrEmpty(inputStartDateText) || string.IsNullOrEmpty(inputHourlyWageText))
            {
                return "Please fill in all fields.";
            }

            if (!inputFirstName.All(char.IsLetter) || !inputLastName.All(char.IsLetter))
            {
                return "First and Last Name must contain only letters.";
            }

            if (!DateTime.TryParse(inputStartDateText, out DateTime _))
            {
                return "Please enter a valid date.";
            }

            if (!decimal.TryParse(inputHourlyWageText, out decimal hourlyWage))
            {
                return "Please enter a valid hourly wage.";
            }

            if (hourlyWage < 0)
            {
                return "Hourly wage cannot be negative.";
            }

            return null;
        }

        /// <summary>
        /// Verifies Add Job inputs.
        /// </summary>
        public string VerifyInputs_AddJob(string title, string costText, DateTime? date)
        {
            if (string.IsNullOrEmpty(title) || date == null || string.IsNullOrEmpty(costText))
            {
                return "Please fill in all job fields.";
            }

            if (!decimal.TryParse(costText, out decimal cost))
            {
                return "Please enter a valid cost.";
            }

            if (cost < 0)
            {
                return "Cost cannot be negative.";
            }

            return null;
        }

        /// <summary>
        /// Verifies Assign Job inputs.
        /// </summary>
        public string VerifyInputs_AssignJob(Job? selectedJob, Contractor? selectedContractor)
        {
            if (selectedJob == null || selectedContractor == null)
            {
                return "Please select a job and a contractor to assign.";
            }

            if (selectedJob.ContractorAssigned != null)
            {
                return "This job is already assigned.";
            }

            return null;
        }

        /// <summary>
        /// Verifies Complete Job inputs.
        /// </summary>
        public string VerifyInputs_CompleteJob(Job? selectedJob)
        {
            if (selectedJob == null)
            {
                return "Please select a job to complete.";
            }

            if (selectedJob.Completed)
            {
                return "This job is already completed.";
            }

            return null;
        }

        /// <summary>
        /// Verifies inputs for cost filtering.
        /// </summary>
        public string VerifyInputs_FilterByCost(string minCostText, string maxCostText, out decimal? minCost, out decimal? maxCost)
        {
            bool hasMin = decimal.TryParse(minCostText, out decimal minParsed);
            bool hasMax = decimal.TryParse(maxCostText, out decimal maxParsed);

            minCost = hasMin ? minParsed : (decimal?)null;
            maxCost = hasMax ? maxParsed : (decimal?)null;

            if (!hasMin && !hasMax)
            {
                return "Please enter at least one valid cost.";
            }

            return null;
        }
    }
}