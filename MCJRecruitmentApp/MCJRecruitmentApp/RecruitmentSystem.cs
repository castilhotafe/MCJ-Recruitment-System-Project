using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCJRecruitmentApp
{
    public record RecruitmentSystem(List<Contractor> Contractors, List<Job> Jobs)
    {
        public List<Contractor> Contractors { get; private set; } = Contractors;
        public List<Job> Jobs { get; private set; } = Jobs;

        public RecruitmentSystem() : this(new List<Contractor>(), new List<Job>())
        {
        }

        public void AddContractor(Contractor contractor)
        {
            Contractors.Add(contractor);
        }

        public void RemoveContractor(Contractor contractor)
        {
            Contractors.Remove(contractor);
        }

        public List<Contractor> GetAllContractors()
        {
            return new List<Contractor>(Contractors);
        }

        public List<Contractor> GetAvailableContractors()
        {
            return Contractors.Where(contractor =>
                !Jobs.Any(job => job.ContractorAssigned == contractor)).ToList();
        }

        public void AddJob(Job job)
        {
            Jobs.Add(job);
        }

        public bool AssignJob(Job job, Contractor contractor)
        {
            if (job.ContractorAssigned != null)
                return false;

            job.ContractorAssigned = contractor;
            job.Completed = false;
            return true;
        }

        public void CompleteJob(Job job)
        {
            job.Completed = true;
            job.ContractorAssigned = null;
        }

        public List<Job> GetAllJobs()
        {
            return new List<Job>(Jobs);
        }

        public List<Job> GetUnassignedJobs()
        {
            return Jobs.Where(job => job.ContractorAssigned == null).ToList();
        }

        public List<Job> GetJobsByCost(decimal? minCost, decimal? maxCost)
        {
            return Jobs.Where(job =>
                (!minCost.HasValue || job.Cost >= minCost.Value) &&
                (!maxCost.HasValue || job.Cost <= maxCost.Value)).ToList();
        }
    }
}