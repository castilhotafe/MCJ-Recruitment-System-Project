using System.Text;
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
    List<Contractor> contractorList = new List<Contractor>();
    List<Job> jobList = new List<Job>();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void AddContractor_Click(object sender, RoutedEventArgs e)
    {
        string firstName = FirstNameInput.Text.Trim().ToUpper();
        string lastName = LastNameInput.Text.Trim().ToUpper();
        string startDateText = StartDateInput.Text.Trim();
        string hourlyWageText = HourlyWageInput.Text.Trim();

        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
            string.IsNullOrEmpty(startDateText) || string.IsNullOrEmpty(hourlyWageText))
        {
            MessageBox.Show("Please fill in all fields.");
            return;
        }

        if (!firstName.All(char.IsLetter) || !lastName.All(char.IsLetter))
        {
            MessageBox.Show("First and Last Name must contain only letters.");
            return;
        }

        DateTime startDate;
        if (!DateTime.TryParse(startDateText, out startDate))
        {
            MessageBox.Show("Please enter a valid date.");
            return;
        }

        decimal hourlyWage;
        if (!decimal.TryParse(hourlyWageText, out hourlyWage))
        {
            MessageBox.Show("Please enter a valid hourly wage.");
            return;
        }

        Contractor newContractor = new Contractor(firstName, lastName, startDate, hourlyWage);
        contractorList.Add(newContractor);
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
        Contractor selectedContractor = ContractorList.SelectedItem as Contractor;

        if (selectedContractor != null)
        {
            contractorList.Remove(selectedContractor);
            ContractorList.Items.Remove(selectedContractor);
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

        foreach (Contractor contractor in contractorList)
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

        foreach (Contractor contractor in contractorList)
        {
            bool isAssigned = jobList.Any(job => job.ContractorAssigned == contractor);

            if (!isAssigned)
            {
                ContractorList.Items.Add(contractor);
            }
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

        decimal cost;
        if (!decimal.TryParse(costText, out cost))
        {
            MessageBox.Show("Please enter a valid cost.");
            return;
        }

        Job newJob = new Job(title, selectedDate.Value, cost);
        jobList.Add(newJob);
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
        Job selectedJob = JobList.SelectedItem as Job;
        Contractor selectedContractor = ContractorList.SelectedItem as Contractor;

        if (selectedJob == null || selectedContractor == null)
        {
            MessageBox.Show("Please select a job and a contractor to assign.");
            return;
        }

        if (selectedJob.ContractorAssigned != null)
        {
            MessageBox.Show("This job is already assigned to another contractor.");
            return;
        }

        selectedJob.ContractorAssigned = selectedContractor;
        selectedJob.Completed = false;

        int index = JobList.Items.IndexOf(selectedJob);
        if (index >= 0)
        {
            JobList.Items[index] = selectedJob;
        }

        ContractorList.Items.Remove(selectedContractor);
        ContractorList.Items.Add(selectedContractor);

        MessageBox.Show($"Contractor {selectedContractor.FirstName} {selectedContractor.LastName} assigned to job {selectedJob.Title}.");

        JobActionsExpander.IsExpanded = false;
        JobListExpander.IsExpanded = true;
    }

    private void CompleteJobButton_Click(object sender, RoutedEventArgs e)
    {
        Job selectedJob = JobList.SelectedItem as Job;

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

        Contractor contractorToReturn = selectedJob.ContractorAssigned;
        selectedJob.Completed = true;
        selectedJob.ContractorAssigned = null;

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

        foreach (Job job in jobList)
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

        foreach (Job job in jobList)
        {
            if (job.ContractorAssigned == null)
            {
                JobList.Items.Add(job);
            }
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

        decimal minCost;
        decimal maxCost;

        bool hasMin = decimal.TryParse(MinCostInput.Text.Trim(), out minCost);
        bool hasMax = decimal.TryParse(MaxCostInput.Text.Trim(), out maxCost);

        if (!hasMin && !hasMax)
        {
            MessageBox.Show("Please enter at least one valid cost.");
            return;
        }

        foreach (Job job in jobList)
        {
            bool showJob = true;

            if (hasMin && job.Cost < minCost)
                showJob = false;

            if (hasMax && job.Cost > maxCost)
                showJob = false;

            if (showJob)
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
