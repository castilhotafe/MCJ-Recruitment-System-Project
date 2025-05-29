using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCJRecruitmentApp
{
    /// <summary>
    /// Represents a contractor in the recruitment application. 
    /// Defines the basic details of a contractor including name, start date, and hourly wage.
    /// </summary>
    public class Contractor
    {
        /// <summary>
        /// Gets or sets the first name of the contractor.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name of the contractor.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets the start date of the contractor's engagement.
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Gets or sets the hourly wage of the contractor.
        /// </summary>
        public decimal HourlyWage { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Contractor"/> class with specified details.
        /// </summary>
        /// <param name="firstName">The contractor's first name.</param>
        /// <param name="lastName">The contractor's last name. </param>
        /// <param name="startDate">The contractor's start date</param>
        /// <param name="hourlyWage">The contractor's hourly wage.</param>
        public Contractor(string firstName, string lastName, DateTime startDate, decimal hourlyWage)
        {
            FirstName = firstName;
            LastName = lastName;
            StartDate = startDate;
            HourlyWage = hourlyWage;
        }


        /// <summary>
        /// Returns a string representation of the contractor's details.
        /// </summary>
        /// <returns>
        /// A string that includes the contractor's first name, last name, start date, and hourly wage formatted as currency.
        /// </returns>
        public override string ToString()
        {
            return $"{FirstName} {LastName}, Start Date: {StartDate.ToShortDateString()}, Hourly Wage: {HourlyWage:C}";
        }

    }
}
