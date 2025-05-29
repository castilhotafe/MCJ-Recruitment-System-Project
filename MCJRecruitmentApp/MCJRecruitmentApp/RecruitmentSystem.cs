using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Adds a contractor to the recruitment system.
        /// </summary>
        /// <param name="contractor">The new Contractor object</param>

        public void AddContractor(Contractor contractor)
        {
            Contractors.Add(contractor);
        }


        /// <summary>
        /// Removes a contractor from the recruitment system.
        /// </summary>
        /// <param name="contractor">The object to be removed</param>
        public void RemoveContractor(Contractor contractor)
        {
            Contractors.Remove(contractor);
        }


        /// <summary>
        /// Retrieves a copy of all contractors in the recruitment system.
        /// </summary>
        /// <returns>
        /// A list of all Contractor objects in the system
        /// </returns>
        public List<Contractor> GetAllContractors()
        {
            return new List<Contractor>(Contractors);
        }


        /// <summary>
        /// Retrieves a list of contractors who are not currently assigned to any job.
        /// </summary>
        /// <returns></returns>
        public List<Contractor> GetAvailableContractors()
        {
            // Create a new list to store contractors who are not assigned to any job
            List<Contractor> availableContractors = new List<Contractor>();

            // Loop through each contractor in the full list of contractors
            foreach (Contractor contractorLocal in Contractors)
            {
                // Assume this contractor is not assigned
                bool isAssigned = false;

                // Loop through each job in the system
                foreach (Job jobLocal in Jobs)
                {
                    // Check if this job is assigned to the current contractor
                    if (jobLocal.ContractorAssigned == contractorLocal)
                    {
                        // Mark contractor as assigned and stop checking further
                        isAssigned = true;
                        break;
                    }
                }

                // If the contractor was not found in any job, add them to the list
                if (!isAssigned)
                {
                    availableContractors.Add(contractorLocal);
                }
            }

            // Return the list of available unassigned contractors
            return availableContractors;
        }


        /// <summary>
        /// Adds a job to the recruitment system.
        /// </summary>
        /// <param name="job">The new job object</param>
        public void AddJob(Job job)
        {
            Jobs.Add(job);
        }


        /// <summary>
        /// Assigns a job to a contractor if the job is not already assigned to another contractor.
        /// </summary>
        /// <param name="job">Job to be verifyied</param>
        /// <param name="contractor">Contractor top be verifyied</param>
        /// <returns>
        /// True if the job was successfully assigned to the contractor; otherwise, false if the job is already assigned to another contractor.
        /// </returns>
        public bool AssignJob(Job job, Contractor contractor)
        {
            if (job.ContractorAssigned != null)
                return false;

            job.ContractorAssigned = contractor;
            job.Completed = false;
            return true;
        }


        /// <summary>
        /// Marks a job as completed and clears the assigned contractor.
        /// </summary>
        /// <param name="job"></param>
        public void CompleteJob(Job job)
        {
            job.Completed = true;
            job.ContractorAssigned = null;
        }


        /// <summary>
        /// Retrieves a list of all jobs in the recruitment system.
        /// </summary>
        /// <returns>
        /// A copy of the list of all Job objects in the system.
        /// </returns>
        public List<Job> GetAllJobs()
        {
            return new List<Job>(Jobs);
        }


        /// <summary>
        /// Retrieves a list of jobs that are not currently assigned to any contractor.
        /// </summary>
        /// <returns>
        /// A list of Job objects that are unassigned, meaning their ContractorAssigned property is null.
        /// </returns>
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
        /// Retrieves a list of jobs filtered by cost range.
        /// </summary>
        /// <param name="minCost"></param>
        /// <param name="maxCost"></param>
        /// <returns>
        /// A list of Job objects where the Cost is within the specified range.
        /// </returns>
        public List<Job> GetJobsByCost(decimal? minCost, decimal? maxCost)
        {
            // Create an empty list to hold jobs that match the cost filter
            List<Job> filteredJobs = new List<Job>();

            // Go through every job in the main Jobs list
            foreach (Job job in Jobs)
            {
                // Check if the job is above or equal to the minimum cost
                // This is true if no minimum was given, OR if the job's cost is >= min
                bool isAboveMin = !minCost.HasValue || job.Cost >= minCost.Value;

                // Check if the job is below or equal to the maximum cost
                // This is true if no maximum was given, OR if the job's cost is <= max
                bool isBelowMax = !maxCost.HasValue || job.Cost <= maxCost.Value;

                // Only add the job if it passed BOTH checks
                if (isAboveMin && isBelowMax)
                {
                    filteredJobs.Add(job);
                }
            }

            // Return the final list with all matching jobs
            return filteredJobs;
        }
    }
}