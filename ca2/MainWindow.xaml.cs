﻿using System;
using System.Collections.Generic;
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
        List<Employee> employees = new List<Employee>();
        List<Employee> FilteredEmployees = new List<Employee>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            employees.Add(new FullTimeEmployee { FirstName = "Jess", Surname = "Walsh", Salary = 30000.00m });
            employees.Add(new FullTimeEmployee { FirstName = "Joe", Surname = "Murphy", Salary = 50000.00m });
            employees.Add(new PartTimeEmployee { FirstName = "John", Surname = "Smith", HourlyRate = 15.00m, HoursWorked = 20 });
            employees.Add(new PartTimeEmployee { FirstName = "Jane", Surname = "Jones", HourlyRate = 11.00m, HoursWorked = 40 });

            LbxEmployees.ItemsSource = employees;
        }

        private void Tcheck_Checked(object sender, RoutedEventArgs e)
        {
            FilteredEmployees.Clear();
            LbxEmployees.ItemsSource = null;

            if (FTcheck.IsChecked == true && PTcheck.IsChecked == true)
            {
                LbxEmployees.ItemsSource = employees;
            }
            else
            {
                if (FTcheck.IsChecked == true)
                {
                    foreach (Employee emp in employees)
                    {
                        if (emp is FullTimeEmployee)
                            FilteredEmployees.Add(emp);
                    }
                }
                else if (PTcheck.IsChecked == true)
                {
                    foreach (Employee emp in employees)
                    {
                        if (emp is PartTimeEmployee)
                            FilteredEmployees.Add(emp);
                    }
                }

                LbxEmployees.ItemsSource = FilteredEmployees;
            }
        }

        private void Tcheck_(object sender, RoutedEventArgs e)
        {

        }

        private void LbxEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnClear_Click(sender, e);

            Employee SelectedEmployee = LbxEmployees.SelectedItem as Employee;

            if (SelectedEmployee != null)
            {
                tbxFirstName.Text = SelectedEmployee.FirstName;
                tbxSurname.Text = SelectedEmployee.Surname;
                if (SelectedEmployee is FullTimeEmployee)
                {
                    tbxSalary.Text = (SelectedEmployee as FullTimeEmployee).Salary.ToString();
                }
                else if (SelectedEmployee is PartTimeEmployee)
                {
                    tbxHourlyRate.Text = (SelectedEmployee as PartTimeEmployee).HourlyRate.ToString();
                    tbxHoursWorked.Text = (SelectedEmployee as PartTimeEmployee).HoursWorked.ToString();
                }

                tbkMonthlyPay.Text = String.Format("{0:0.00}", SelectedEmployee.CalculateMonthlyPay());
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbxFirstName.Text = "";
            tbxSurname.Text = "";
            tbxSalary.Text = "";
            tbxHourlyRate.Text = "";
            tbxHoursWorked.Text = "";
            tbkMonthlyPay.Text = "";
        }
    }
}
