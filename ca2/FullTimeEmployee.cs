using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ca2
{
    class FullTimeEmployee : Employee
    {
        public decimal Salary { get; set; }

        public FullTimeEmployee() : this("none", "none", 0) { }

        public FullTimeEmployee(string firstname, string surname) : this(firstname, surname, 0) { }

        public FullTimeEmployee(string firstname, string surname, decimal salary)
        {
            FirstName = firstname;
            Surname = surname;
            Salary = salary;
        }

        public override decimal CalculateMonthlyPay()
        {
            return Salary / 12;
        }

        public override string ToString()
        {
            return String.Format($"{Surname.ToUpper()}, {FirstName} - Full Time ");
        }
    }
}
