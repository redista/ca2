using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ca2
{
    abstract class Employee : IComparable<Employee>
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }

        abstract public decimal CalculateMonthlyPay();

        public int CompareTo(Employee other)
        {
            return Surname.CompareTo(other.Surname);
        }
    }
}
