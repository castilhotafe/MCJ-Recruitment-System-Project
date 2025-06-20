﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MCJRecruitmentApp;

public partial class MainWindow : Window
{
    /* 
    Quick object structure reminder

    Contractor(string firstName, string lastName, DateTime startDate, decimal hourlyWage)
    firstName from FirstNameInput.Text
    lastName from LastNameInput.Text
    startDate from StartDateInput.Text or SelectedDate (converted to DateTime)
    hourlyWage from HourlyWageInput.Text (converted to decimal)

    Job(string title, DateTime date, decimal cost)
    title from JobTitleInput.Text
    date from JobDateInput.SelectedDate
    cost from JobCostInput.Text (converted to decimal)

    Stored in the back-end lists:
    contractorList → holds all Contractor objects
    jobList → holds all Job objects

    Shown in UI using:
    ContractorList.Items.Add(...)
    JobList.Items.Add(...)
    */

    private RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
    
    /// <summary>
    /// Main Window Constructor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    public void AddContractor_Click(object sender, RoutedEventArgs e)
    {
        string inputFirstName = FirstNameInput.Text.Trim().ToUpper();
        string inputLastName = LastNameInput.Text.Trim().ToUpper();
        string inputStartDateText = StartDateInput.Text.Trim();
        string inputHourlyWageText = HourlyWageInput.Text.Trim();

        string result = recruitmentSystem.VerifyInputs(inputFirstName, inputLastName, inputStartDateText, inputHourlyWageText);

        if (result != null)
        {
            MessageBox.Show(result);
            return;
        }

        DateTime startDate = DateTime.Parse(StartDateInput.Text);
        decimal hourlyWage = decimal.Parse(HourlyWageInput.Text);

        Contractor newContractor = new Contractor(inputFirstName, inputLastName, startDate, hourlyWage);
        recruitmentSystem.AddContractor(newContractor);
        ContractorList.Items.Add(newContractor);

        MessageBox.Show("Contractor added!");

        FirstNameInput.Text = "";
        LastNameInput.Text = "";
        StartDateInput.Text = "";
        HourlyWageInput.Text = "";

        ContractorActionsExpander.IsExpanded = false;
        ContractorListExpander.IsExpanded = true;
    }

    private void RemoveContractor_Click(object sender, RoutedEventArgs e)
    {
        Contractor? selectedContractor = ContractorList.SelectedItem as Contractor;

        if (selectedContractor != null)
        {
            recruitmentSystem.RemoveContractor(selectedContractor);
            ContractorList.Items.Remove(selectedContractor);
            JobList.Items.Clear();
            foreach (Job job in recruitmentSystem.GetAllJobs())
            {
                JobList.Items.Add(job);
            }
            ContractorActionsExpander.IsExpanded = false;
            ContractorListExpander.IsExpanded = true;
        }
        else
        {
            MessageBox.Show("Please select a contractor to remove.");
        }
    }

    private void GetContractorsButton_Click(object sender, RoutedEventArgs e)
    {
        ContractorList.Items.Clear();

        foreach (Contractor contractor in recruitmentSystem.GetAllContractors())
        {
            ContractorList.Items.Add(contractor);
        }

        ContractorActionsExpander.IsExpanded = false;
        ContractorListExpander.IsExpanded = true;

        if (ContractorList.Items.Count == 0)
        {
            MessageBox.Show("No contractors found.");
        }
    }

    private void GetAvailableContractorsButton_Click(object sender, RoutedEventArgs e)
    {
        ContractorList.Items.Clear();

        foreach (Contractor contractor in recruitmentSystem.GetAvailableContractors())
        {
            ContractorList.Items.Add(contractor);
        }

        ContractorActionsExpander.IsExpanded = false;
        ContractorListExpander.IsExpanded = true;

        if (ContractorList.Items.Count == 0)
        {
            MessageBox.Show("No available contractors found.");
        }
    }

    private void AddJobButton_Click(object sender, RoutedEventArgs e)
    {
        string title = JobTitleInput.Text.Trim().ToUpper();
        string costText = JobCostInput.Text.Trim();
        DateTime? selectedDate = JobDateInput.SelectedDate;

        if (string.IsNullOrEmpty(title) || selectedDate == null || string.IsNullOrEmpty(costText))
        {
            MessageBox.Show("Please fill in all job fields.");
            return;
        }

        if (!decimal.TryParse(costText, out decimal cost))
        {
            MessageBox.Show("Please enter a valid cost.");
            return;
        }

        if (cost < 0)
        {
            MessageBox.Show("Cost cannot be negative");
            return;
        }

        Job newJob = new Job(title, selectedDate.Value, cost);
        recruitmentSystem.AddJob(newJob);
        JobList.Items.Add(newJob);

        MessageBox.Show("Job added!");

        JobTitleInput.Text = "";
        JobCostInput.Text = "";
        JobDateInput.SelectedDate = null;

        JobActionsExpander.IsExpanded = false;
        JobListExpander.IsExpanded = true;
    }

    private void AssignJobButton_Click(object sender, RoutedEventArgs e)
    {
        Job? selectedJob = JobList.SelectedItem as Job;
        Contractor? selectedContractor = ContractorList.SelectedItem as Contractor;

        if (selectedJob == null || selectedContractor == null)
        {
            MessageBox.Show("Please select a job and a contractor to assign.");
            return;
        }

        bool success = recruitmentSystem.AssignJob(selectedJob, selectedContractor);

        if (!success)
        {
            MessageBox.Show("This job is already assigned.");
            return;
        }

        JobList.Items.Clear();
        foreach (Job job in recruitmentSystem.GetAllJobs())
        {
            JobList.Items.Add(job);
        }

        ContractorList.Items.Remove(selectedContractor);
        ContractorList.Items.Add(selectedContractor);

        MessageBox.Show($"Contractor {selectedContractor.FirstName} {selectedContractor.LastName} assigned to job {selectedJob.Title}.");

        JobActionsExpander.IsExpanded = false;
        JobListExpander.IsExpanded = true;
    }

    private void CompleteJobButton_Click(object sender, RoutedEventArgs e)
    {
        Job? selectedJob = JobList.SelectedItem as Job;

        if (selectedJob == null)
        {
            MessageBox.Show("Please select a job to complete.");
            return;
        }

        if (selectedJob.Completed)
        {
            MessageBox.Show("This job is already completed.");
            return;
        }

        Contractor? contractorToReturn = selectedJob.ContractorAssigned;

        recruitmentSystem.CompleteJob(selectedJob);
        JobList.Items.Remove(selectedJob);

        if (contractorToReturn != null && !ContractorList.Items.Contains(contractorToReturn))
        {
            ContractorList.Items.Add(contractorToReturn);
        }

        MessageBox.Show($"Job '{selectedJob.Title}' completed!");

        JobActionsExpander.IsExpanded = false;
        JobListExpander.IsExpanded = true;
    }

    private void GetJobsButton_Click(object sender, RoutedEventArgs e)
    {
        JobList.Items.Clear();

        foreach (Job job in recruitmentSystem.GetAllJobs())
        {
            JobList.Items.Add(job);
        }

        JobActionsExpander.IsExpanded = false;
        JobListExpander.IsExpanded = true;

        if (JobList.Items.Count == 0)
        {
            MessageBox.Show("No jobs found.");
        }
    }

    private void GetUnassignedJobsButton_Click(object sender, RoutedEventArgs e)
    {
        JobList.Items.Clear();

        foreach (Job job in recruitmentSystem.GetUnassignedJobs())
        {
            JobList.Items.Add(job);
        }

        JobActionsExpander.IsExpanded = false;
        JobListExpander.IsExpanded = true;

        if (JobList.Items.Count == 0)
        {
            MessageBox.Show("No unassigned jobs found.");
        }
    }

    private void GetJobByCostButton_Click(object sender, RoutedEventArgs e)
    {
        JobList.Items.Clear();

        bool hasMin = decimal.TryParse(MinCostInput.Text.Trim(), out decimal minCost);
        bool hasMax = decimal.TryParse(MaxCostInput.Text.Trim(), out decimal maxCost);

        if (!hasMin && !hasMax)
        {
            MessageBox.Show("Please enter at least one valid cost.");
            return;
        }

        var jobs = recruitmentSystem.GetJobsByCost(
            hasMin ? minCost : (decimal?)null,
            hasMax ? maxCost : (decimal?)null
        );

        foreach (Job job in jobs)
        {
            JobList.Items.Add(job);
        }

        JobActionsExpander.IsExpanded = false;
        JobListExpander.IsExpanded = true;

        if (JobList.Items.Count == 0)
        {
            MessageBox.Show("No jobs found in the selected cost range.");
        }
    }
}
