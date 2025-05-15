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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MCJRecruitmentApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // This list stores all added contractors
    List<Contractor> contractorList = new List<Contractor>();
    List<Job> jobList = new List<Job>();
    public MainWindow()
    {
        InitializeComponent();
    }

    // This method runs when the Add Contractor button is clicked
    private void AddContractor_Click(object sender, RoutedEventArgs e)
    {
        // Get text from the input boxes
        string firstName = FirstNameInput.Text.Trim().ToUpper();
        string lastName = LastNameInput.Text.Trim().ToUpper();
        string startDateText = StartDateInput.Text.Trim();
        string hourlyWageText = HourlyWageInput.Text.Trim();

        // Check if any field is empty
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
            string.IsNullOrEmpty(startDateText) || string.IsNullOrEmpty(hourlyWageText))
        {
            MessageBox.Show("Please fill in all fields.");
            return;
        }

        // Convert StartDate from string to DateTime
        DateTime startDate;
        if (!DateTime.TryParse(startDateText, out startDate))
        {
            MessageBox.Show("Please enter a valid date.");
            return;
        }

        // Convert HourlyWage from string to decimal
        decimal hourlyWage;
        if (!decimal.TryParse(hourlyWageText, out hourlyWage))
        {
            MessageBox.Show("Please enter a valid hourly wage.");
            return;
        }

        // Create a new Contractor object using the input values
        Contractor newContractor = new Contractor(firstName, lastName, startDate, hourlyWage);

        // Add the new contractor to the list (back-end)
        contractorList.Add(newContractor);
        // Add the new contractor to the ListBox (front-end)
        ContractorList.Items.Add(newContractor);


        MessageBox.Show("Contractor added!");

        // clear the input fields
        FirstNameInput.Text = "";
        LastNameInput.Text = "";
        StartDateInput.Text = "";
        HourlyWageInput.Text = "";
    }

    private void RemoveContractor_Click(object sender, RoutedEventArgs e)
    {
        // Get the selected item from the ListBox
        Contractor selectedContractor = ContractorList.SelectedItem as Contractor;

        // If nothing is selected, show a message and stop here
        if (!(selectedContractor == null))
        {
            // Remove the contractor from the list (back-end)
            contractorList.Remove(selectedContractor);

            // Remove the contractor from the ListBox (front-end)
            ContractorList.Items.Remove(selectedContractor);
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
    }

    private void GetAvailableContractorsButton_Click(object sender, RoutedEventArgs e)
    {
        ContractorList.Items.Clear();

        foreach (Contractor contractor in contractorList)
        {
            // Verifica se ele está atribuído a algum job
            bool isAssigned = jobList.Any(job => job.ContractorAssigned == contractor);

            if (!isAssigned)
            {
                ContractorList.Items.Add(contractor);
            }
        }

        if (ContractorList.Items.Count == 0)
        {
            MessageBox.Show("No available contractors found.");
        }
    }

    private void AddJobButton_Click(object sender, RoutedEventArgs e)
    {
        // Get values from input fields
        string title = JobTitleInput.Text.Trim().ToUpper();
        string costText = JobCostInput.Text.Trim();
        DateTime? selectedDate = JobDateInput.SelectedDate;

        // Check if any field is empty
        if (string.IsNullOrEmpty(title) || selectedDate == null || string.IsNullOrEmpty(costText))
        {
            MessageBox.Show("Please fill in all job fields.");
            return;
        }

        // Convert cost to decimal
        decimal cost;
        if (!decimal.TryParse(costText, out cost))
        {
            MessageBox.Show("Please enter a valid cost.");
            return;
        }

        // Create new job
        Job newJob = new Job(title, selectedDate.Value, cost);

        // Add to job list (back-end)
        jobList.Add(newJob);

        // Add to UI list (front-end)
        JobList.Items.Add(newJob);

        // Confirm to user
        MessageBox.Show("Job added!");

        // Clear input fields
        JobTitleInput.Text = "";
        JobCostInput.Text = "";
        JobDateInput.SelectedDate = null;
    }

    private void AssignJobButton_Click(object sender, RoutedEventArgs e)
    {
        // Get selected job and contractor from the ListBoxes
        Job selectedJob = JobList.SelectedItem as Job;
        Contractor selectedContractor = ContractorList.SelectedItem as Contractor;

        // Check if any field is empty
        if (selectedJob == null || selectedContractor == null)
        {
            MessageBox.Show("Please select a job and a contractor to assign.");
            return;
        }

        // Check if the job is already assigned
        if (selectedJob.ContractorAssigned != null)
        {
            MessageBox.Show("This job is already assigned to another contractor.");
            return;
        }

        // Assign the contractor to the job
        selectedJob.ContractorAssigned = selectedContractor;
        selectedJob.Completed = false;

        // Update the job display in the ListBox 
        int index = JobList.Items.IndexOf(selectedJob);
        if (index >= 0)
        {
            JobList.Items[index] = selectedJob;
        }

        // Update the contractor in the ListBox
        ContractorList.Items.Remove(selectedContractor);
        ContractorList.Items.Add(selectedContractor);

        // Confirm to user
        MessageBox.Show($"Contractor {selectedContractor.FirstName} {selectedContractor.LastName} assigned to job {selectedJob.Title}.");

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

        // Save the contractor before unassigning
        Contractor contractorToReturn = selectedJob.ContractorAssigned;

        // Mark the job as completed and unassign the contractor
        selectedJob.Completed = true;
        selectedJob.ContractorAssigned = null;

        // Update the job display in the ListBox
        JobList.Items.Remove(selectedJob);

        // Add contractor back to the ContractorList if not already there
        if (contractorToReturn != null && !ContractorList.Items.Contains(contractorToReturn))
        {
            ContractorList.Items.Add(contractorToReturn);
        }

        MessageBox.Show($"Job '{selectedJob.Title}' completed!");
    }

    private void GetJobsButton_Click(object sender, RoutedEventArgs e)
    {
        // Clear the current job list in the UI
        JobList.Items.Clear();

        // Loop through all jobs and add them to the ListBox
        foreach (Job job in jobList)
        {
            JobList.Items.Add(job);
        }

        // If no jobs exist, show a message
        if (JobList.Items.Count == 0)
        {
            MessageBox.Show("No jobs found.");
        }
    }

    private void GetUnassignedJobsButton_Click(object sender, RoutedEventArgs e)
    {
        // Clear the job list in the UI to avoid duplicates
        JobList.Items.Clear();

        // Loop through all jobs and only add the unassigned ones
        foreach (Job job in jobList)
        {
            if (job.ContractorAssigned == null)
            {
                JobList.Items.Add(job);
            }
        }

        // If no unassigned jobs are found, inform the user
        if (JobList.Items.Count == 0)
        {
            MessageBox.Show("No unassigned jobs found.");
        }
    }

    private void GetJobByCostButton_Click(object sender, RoutedEventArgs e)
    {
        // Clear previous search results from the JobList
        JobList.Items.Clear();

        // Try to parse the min and max cost from input fields
        decimal minCost, maxCost;

        if (!decimal.TryParse(MinCostInput.Text.Trim(), out minCost) ||
            !decimal.TryParse(MaxCostInput.Text.Trim(), out maxCost))
        {
            MessageBox.Show("Please enter valid numbers for Min and Max Cost.");
            return;
        }

        // Filter jobs within the specified cost range
        List<Job> filteredJobs = jobList
            .Where(job => job.Cost >= minCost && job.Cost <= maxCost)
            .ToList();

        foreach (Job job in filteredJobs)
        {
            JobList.Items.Add(job);
        }

        if (filteredJobs.Count == 0)
        {
            MessageBox.Show("No jobs found in the specified cost range.");
        }
    }
}
