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

    private RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
    
    /// <summary>
    /// Main Window Constructor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Adds a contractor after validating inputs.
    /// </summary>
    public void AddContractor_Click(object sender, RoutedEventArgs e)
    {
        string first = FirstNameInput.Text.Trim().ToUpper();
        string last = LastNameInput.Text.Trim().ToUpper();
        string date = StartDateInput.Text.Trim();
        string wage = HourlyWageInput.Text.Trim();

        string msg = recruitmentSystem.VerifyInputs_AddContractor(first, last, date, wage);
        if (msg != null) { MessageBox.Show(msg); return; }

        DateTime startDate = DateTime.Parse(date);
        decimal hourlyWage = decimal.Parse(wage);

        Contractor c = new Contractor(first, last, startDate, hourlyWage);
        recruitmentSystem.AddContractor(c);
        ContractorList.Items.Add(c);

        MessageBox.Show("Contractor added!");
        FirstNameInput.Text = LastNameInput.Text = StartDateInput.Text = HourlyWageInput.Text = "";
        ContractorActionsExpander.IsExpanded = false;
        ContractorListExpander.IsExpanded = true;
    }

    /// <summary>
    /// Removes a contractor and updates job list.
    /// </summary>
    private void RemoveContractor_Click(object sender, RoutedEventArgs e)
    {
        Contractor? c = ContractorList.SelectedItem as Contractor;
        if (c == null) { MessageBox.Show("Please select a contractor to remove."); return; }

        recruitmentSystem.RemoveContractor(c);
        ContractorList.Items.Remove(c);

        JobList.Items.Clear();
        foreach (Job j in recruitmentSystem.GetAllJobs()) JobList.Items.Add(j);

        ContractorActionsExpander.IsExpanded = false;
        ContractorListExpander.IsExpanded = true;
    }

    /// <summary>
    /// Displays all contractors.
    /// </summary>
    private void GetContractorsButton_Click(object sender, RoutedEventArgs e)
    {
        ContractorList.Items.Clear();
        foreach (var c in recruitmentSystem.GetAllContractors()) ContractorList.Items.Add(c);

        ContractorActionsExpander.IsExpanded = false;
        ContractorListExpander.IsExpanded = true;
        if (ContractorList.Items.Count == 0) MessageBox.Show("No contractors found.");
    }

    /// <summary>
    /// Displays available (unassigned) contractors.
    /// </summary>
    private void GetAvailableContractorsButton_Click(object sender, RoutedEventArgs e)
    {
        ContractorList.Items.Clear();
        foreach (var c in recruitmentSystem.GetAvailableContractors()) ContractorList.Items.Add(c);

        ContractorActionsExpander.IsExpanded = false;
        ContractorListExpander.IsExpanded = true;
        if (ContractorList.Items.Count == 0) MessageBox.Show("No available contractors found.");
    }

    /// <summary>
    /// Adds a new job after validating inputs.
    /// </summary>
    private void AddJobButton_Click(object sender, RoutedEventArgs e)
    {
        string title = JobTitleInput.Text.Trim().ToUpper();
        string costT = JobCostInput.Text.Trim();
        DateTime? dt = JobDateInput.SelectedDate;

        string msg = recruitmentSystem.VerifyInputs_AddJob(title, costT, dt);
        if (msg != null) { MessageBox.Show(msg); return; }

        Job job = new Job(title, dt!.Value, decimal.Parse(costT));
        recruitmentSystem.AddJob(job);
        JobList.Items.Add(job);

        MessageBox.Show("Job added!");
        JobTitleInput.Text = JobCostInput.Text = "";
        JobDateInput.SelectedDate = null;
        JobActionsExpander.IsExpanded = false;
        JobListExpander.IsExpanded = true;
    }

    /// <summary>
    /// Assigns a contractor to a job after validation.
    /// </summary>
    private void AssignJobButton_Click(object sender, RoutedEventArgs e)
    {
        Job? j = JobList.SelectedItem as Job;
        Contractor? c = ContractorList.SelectedItem as Contractor;

        string msg = recruitmentSystem.VerifyInputs_AssignJob(j, c);
        if (msg != null) { MessageBox.Show(msg); return; }

        recruitmentSystem.AssignJob(j!, c!);

        JobList.Items.Clear();
        foreach (var job in recruitmentSystem.GetAllJobs()) JobList.Items.Add(job);

        ContractorList.Items.Remove(c);
        ContractorList.Items.Add(c);

        MessageBox.Show($"Contractor {c!.FirstName} {c.LastName} assigned to job {j!.Title}.");
        JobActionsExpander.IsExpanded = false;
        JobListExpander.IsExpanded = true;
    }

    /// <summary>
    /// Marks a job as complete after validation.
    /// </summary>
    private void CompleteJobButton_Click(object sender, RoutedEventArgs e)
    {
        Job? j = JobList.SelectedItem as Job;
        string msg = recruitmentSystem.VerifyInputs_CompleteJob(j);
        if (msg != null) { MessageBox.Show(msg); return; }

        Contractor? backToPool = j!.ContractorAssigned;
        recruitmentSystem.CompleteJob(j);
        JobList.Items.Remove(j);

        if (backToPool != null && !ContractorList.Items.Contains(backToPool))
            ContractorList.Items.Add(backToPool);

        MessageBox.Show($"Job '{j.Title}' completed!");
        JobActionsExpander.IsExpanded = false;
        JobListExpander.IsExpanded = true;
    }

    /// <summary>
    /// Displays all jobs.
    /// </summary>
    private void GetJobsButton_Click(object sender, RoutedEventArgs e)
    {
        JobList.Items.Clear();
        foreach (var j in recruitmentSystem.GetAllJobs()) JobList.Items.Add(j);

        JobActionsExpander.IsExpanded = false;
        JobListExpander.IsExpanded = true;
        if (JobList.Items.Count == 0) MessageBox.Show("No jobs found.");
    }

    /// <summary>
    /// Displays unassigned jobs.
    /// </summary>
    private void GetUnassignedJobsButton_Click(object sender, RoutedEventArgs e)
    {
        JobList.Items.Clear();
        foreach (var j in recruitmentSystem.GetUnassignedJobs()) JobList.Items.Add(j);

        JobActionsExpander.IsExpanded = false;
        JobListExpander.IsExpanded = true;
        if (JobList.Items.Count == 0) MessageBox.Show("No unassigned jobs found.");
    }

    /// <summary>
    /// Filters jobs by cost after validating inputs.
    /// </summary>
    private void GetJobByCostButton_Click(object sender, RoutedEventArgs e)
    {
        string msg = recruitmentSystem.VerifyInputs_FilterByCost(
            MinCostInput.Text.Trim(), MaxCostInput.Text.Trim(),
            out decimal? min, out decimal? max);

        if (msg != null) { MessageBox.Show(msg); return; }

        JobList.Items.Clear();
        foreach (var j in recruitmentSystem.GetJobsByCost(min, max)) JobList.Items.Add(j);

        JobActionsExpander.IsExpanded = false;
        JobListExpander.IsExpanded = true;
        if (JobList.Items.Count == 0) MessageBox.Show("No jobs found in the selected cost range.");
    }
}

