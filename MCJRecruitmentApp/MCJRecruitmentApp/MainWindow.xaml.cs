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

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // This list stores all added contractors
    List<Contractor> contractorList = new List<Contractor>();
    public MainWindow()
    {
        InitializeComponent();
    }

    // This method runs when the Add Contractor button is clicked
    private void AddContractor_Click(object sender, RoutedEventArgs e)
    {
        // Get text from the input boxes
        string firstName = FirstNameInput.Text.Trim();
        string lastName = LastNameInput.Text.Trim();
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
}
