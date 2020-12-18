using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ca2
{
    class PartTimeEmployee : Employee
    {
        public decimal HourlyRate { get; set; }
        public double HoursWorked { get; set; }

        public PartTimeEmployee() : this("none", "none", 0, 0) { }

        public PartTimeEmployee(string firstname, string surname) : this(firstname, surname, 0, 0) { }

        public PartTimeEmployee(string firstname, string surname, decimal hourlyrate, double hoursworked)
        {
            FirstName = firstname;
            Surname = surname;
            HourlyRate = hourlyrate;
            HoursWorked = hoursworked;
        }

        public override decimal CalculateMonthlyPay()
        {
            return HourlyRate * (decimal)HoursWorked;
        }

        public override string ToString()
        {
            return String.Format($"{Surname.ToUpper()}, {FirstName} - Part Time ");
        }
    }
}
