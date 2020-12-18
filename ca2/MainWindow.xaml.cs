using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ca2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Employee list and the filtered employees
        List<Employee> employees = new List<Employee>();
        List<Employee> FilteredEmployees = new List<Employee>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // When the window loads, add some employee objects to the employees list. Sort it, set listbox source, check boxes

            // Making some basic employee classes
            employees.Add(new FullTimeEmployee("Jess", "Walsh", 30000.00m));
            employees.Add(new FullTimeEmployee ("Joe", "Murphy", 50000.00m));
            employees.Add(new PartTimeEmployee ("John", "Smith", 15.00m, 20));
            employees.Add(new PartTimeEmployee ("Jane", "Jones", 11.00m, 40));

            // Sort the employees, then set the source of the listbox to the employees list
            employees.Sort();
            LbxEmployees.ItemsSource = employees;

            // This detaches the event handler for checking the boxes, checks them, and then adds the event handler back.
            // This is so the event handler isn't called multiple times at the start (waste)
            FTcheck.Checked -= Tcheck_Checked;
            PTcheck.Checked -= Tcheck_Checked;
            FTcheck.IsChecked = true;
            PTcheck.IsChecked = true;
            FTcheck.Checked += Tcheck_Checked;
            PTcheck.Checked += Tcheck_Checked;
        }

        private void Tcheck_Checked(object sender, RoutedEventArgs e)
        {
            // When one of the checkboxes is checked or unchecked, refresh the list to display the new filtered list
            RefreshList();
        }

        private void LbxEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Puts info into the text boxes once
            // one of the employees in the listbox is selected

            // Clears the info
            ClearInfo();

            // Sets the employee selected to geneiric employee class
            Employee SelectedEmployee = LbxEmployees.SelectedItem as Employee;

            // If it is null, don't do anything
            if (SelectedEmployee != null)
            {

                // Since the firstname and surname is common in FT and PT employee, you can set it
                tbxFirstName.Text = SelectedEmployee.FirstName;
                tbxSurname.Text = SelectedEmployee.Surname;

                // If the emp is FT, cast it to FT and set the salary. 
                // This is because you can't access the derived class properties if it is not in the base class.
                if (SelectedEmployee is FullTimeEmployee)
                {
                    tbxSalary.Text = (SelectedEmployee as FullTimeEmployee).Salary.ToString();

                    // Set the radio button of Full time to true to show it is a full time employee
                    // Also for ease in updating
                    FTradio.IsChecked = true;
                }
                else if (SelectedEmployee is PartTimeEmployee)
                {
                    // Same as above but with Part time properties
                    PartTimeEmployee PTtmp = SelectedEmployee as PartTimeEmployee;

                    tbxHourlyRate.Text = PTtmp.HourlyRate.ToString();
                    tbxHoursWorked.Text = PTtmp.HoursWorked.ToString();

                    PTradio.IsChecked = true;
                }

                // Calculate the montly pay and display it
                tbkMonthlyPay.Text = String.Format("{0:0.00}", SelectedEmployee.CalculateMonthlyPay());
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            // See ClearInfo method
            ClearInfo();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Adds the info from the textboxes to either a full time or part time employee class.
            // This only works if the salary (or hoursworked and hourlyrate for a part time) are numbers
            // Otherwise, it displays an error message on the top of the screen.

            // If the FT radio is checked, create a fulltime employee, otherwise part time
            if ((Boolean)FTradio.IsChecked)
            {
                decimal salary = 0;
                
                // Attmpy to get salary, if it's not valid, display error msg
                if (decimal.TryParse(tbxSalary.Text, out salary))
                {
                    FullTimeEmployee emp = new FullTimeEmployee(tbxFirstName.Text, tbxSurname.Text, salary);
                    employees.Add(emp);
                }
                else
                {
                    tbkMsg.Text = "Salary is not valid";
                }
            }
            else if ((Boolean)PTradio.IsChecked)
            {
                decimal hourlyrate = 0;
                double hoursworked = 0;

                // If the hourly rate and works worked are not valid, display error msg
                if (decimal.TryParse(tbxHourlyRate.Text, out hourlyrate) && double.TryParse(tbxHourlyRate.Text, out hoursworked))
                {
                    // Make a new part time employee with the constuctors
                    PartTimeEmployee emp = new PartTimeEmployee(tbxFirstName.Text, tbxSurname.Text, hourlyrate, hoursworked);
                    employees.Add(emp);
                }
                else
                {
                    tbkMsg.Text = "Hourly rate and hours worked must be numbers";
                }
            }    
            else
            {
                tbkMsg.Text = "A part time or full time option must be selected";
            }

            // Clears the info once done, then refresh the list
            ClearInfo();
            RefreshList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        { 
            // Delrtes a selected employee from the list 
            
            // Get the selected employee
            Employee SelectedEmployee = LbxEmployees.SelectedItem as Employee;


            // If it exists, delete it (remove from list)
            if (SelectedEmployee != null)
            {
                employees.Remove(SelectedEmployee);

                // Refresh list, doesn't need to be done if we didn't delete anything
                RefreshList();
            }
            else
            {
                tbkMsg.Text = "Select an employee to delete";
            }

            ClearInfo();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Updates a selected employee object with info entered into the boxes.
            // Only updates if the info is valid
            // Does not allow you to change a part time employee to a full time and vice-versa

            // Get the selected employee
            Employee SelectedEmployee = LbxEmployees.SelectedItem as Employee;

            if (SelectedEmployee != null)
            {
                // If the employee is full time, try to get and set salary to the one in the textbox
                if (SelectedEmployee is FullTimeEmployee)
                {
                    FullTimeEmployee FTemp = SelectedEmployee as FullTimeEmployee;

                    decimal salary = 0;

                    // Try to get salary. On success, set values to employee obj
                    if ((Boolean)FTradio.IsChecked)
                    {
                        if (decimal.TryParse(tbxSalary.Text, out salary))
                        {
                            FTemp.FirstName = tbxFirstName.Text;
                            FTemp.Surname = tbxSurname.Text;
                            FTemp.Salary = salary;
                        }
                        else
                        {
                            tbkMsg.Text = "Salary is not valid";
                        }
                    }
                    else
                    {
                        decimal hourlyrate = 0;
                        double hoursworked = 0;

                        // If the hourly rate and works worked are not valid, display error msg
                        if (decimal.TryParse(tbxHourlyRate.Text, out hourlyrate) && double.TryParse(tbxHoursWorked.Text, out hoursworked))
                        {
                            // Make a new part time employee with the constuctors
                            employees.Remove(SelectedEmployee);

                            PartTimeEmployee emp = new PartTimeEmployee(tbxFirstName.Text, tbxSurname.Text, hourlyrate, hoursworked);
                            employees.Add(emp);
                        }
                        else
                        {
                            tbkMsg.Text = "Hourly rate and hours worked must be numbers";
                        }
                    }
                }
                else if (SelectedEmployee is PartTimeEmployee)
                {
                    // Casting the selected employee to part time
                    PartTimeEmployee PTemp = SelectedEmployee as PartTimeEmployee;

                    decimal hourlyrate = 0;
                    double hoursworked = 0;

                    // Try to get the hourlyrate and hours worked. If successful, set them to the employee
                    if ((Boolean)PTradio.IsChecked)
                    {
                        if (decimal.TryParse(tbxHourlyRate.Text, out hourlyrate) && double.TryParse(tbxHoursWorked.Text, out hoursworked))
                        {
                            PTemp.FirstName = tbxFirstName.Text;
                            PTemp.Surname = tbxSurname.Text;
                            PTemp.HourlyRate = hourlyrate;
                            PTemp.HoursWorked = hoursworked;
                        }
                        else
                        {
                            tbkMsg.Text = "Hourly rate and hours worked not valid";
                        }
                    }
                    else
                    {
                        decimal salary = 0;

                        // Attmpy to get salary, if it's not valid, display error msg
                        if (decimal.TryParse(tbxSalary.Text, out salary))
                        {
                            employees.Remove(SelectedEmployee);

                            FullTimeEmployee emp = new FullTimeEmployee(tbxFirstName.Text, tbxSurname.Text, salary);
                            employees.Add(emp);
                        }
                        else
                        {
                            tbkMsg.Text = "Salary is not valid";
                        }
                    }
                }
                else
                {
                    tbkMsg.Text = "Select an employee to update";
                }

                // Get the new monthly pay
                tbkMonthlyPay.Text = String.Format("{0:0.00}", SelectedEmployee.CalculateMonthlyPay());
            }
            else
            {
                tbkMsg.Text = "Select an employee to update";
            }

            ClearInfo();
            RefreshList();
        }

        private void RefreshList()
        {
            // Sort the list of employees
            employees.Sort();

            // Make the filtered employees empty
            FilteredEmployees.Clear();
            // Set the listbox source to null
            LbxEmployees.ItemsSource = null;

            // If both are checked, show everything. In this case it is just the employees list
            if (FTcheck.IsChecked == true && PTcheck.IsChecked == true)
            {
                LbxEmployees.ItemsSource = employees;
            }
            else
            {
                // If only the FT is checked, only get full time employees. They are already sorted, so there is no need to sort again
                if (FTcheck.IsChecked == true)
                {
                    foreach (Employee emp in employees)
                    {
                        if (emp is FullTimeEmployee)
                            FilteredEmployees.Add(emp);
                    }
                }
                // Similar to FT check
                else if (PTcheck.IsChecked == true)
                {
                    foreach (Employee emp in employees)
                    {
                        if (emp is PartTimeEmployee)
                            FilteredEmployees.Add(emp);
                    }
                }

                // Set the listbox source to the filtered employees
                LbxEmployees.ItemsSource = FilteredEmployees;
            }
        }

        private void ClearInfo()
        {
            // Sets the info in all the boxes to an empty string.

            tbxFirstName.Text = "";
            tbxSurname.Text = "";
            tbxSalary.Text = "";
            tbxHourlyRate.Text = "";
            tbxHoursWorked.Text = "";
            tbkMonthlyPay.Text = "";
        }
    }
}
