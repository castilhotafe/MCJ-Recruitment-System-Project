using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCJRecruitmentApp
{
    public class Contractor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime StartDate { get; set; }

       public decimal HourlyWage { get; set; }


        public Contractor(string firstName, string lastName, DateTime startDate, decimal hourlyWage)
        {
            FirstName = firstName;
            LastName = lastName;
            StartDate = startDate;
            HourlyWage = hourlyWage;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}, Start Date: {StartDate.ToShortDateString()}, Hourly Wage: {HourlyWage:C}";
        }

    }
}
