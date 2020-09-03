using EmployeeRecords.Command;
using EmployeeRecords.Loggers;
using EmployeeRecords.Model;
using EmployeeRecords.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace EmployeeRecords.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private readonly MainWindow view;
        private readonly BackgroundWorker workerDelete = new BackgroundWorker();
        private vwEmployee employee;
        private List<vwEmployee> employees;
        private tblEmployee selectedEmployee;
        private List<tblLocation> locations;
        private readonly DataAccess db = new DataAccess();
        int employeeID;
        #endregion

        #region Properties            

        public vwEmployee Employee
        {
            get
            {
                return employee;
            }
            set
            {
                employee = value;
                OnPropertyChanged(nameof(Employee));
            }
        }
        public List<vwEmployee> Employees
        {
            get
            {
                return employees;
            }
            set
            {
                employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public List<tblLocation> Locations
        {
            get
            {
                return locations;
            }
            set
            {
                locations = value;
                OnPropertyChanged(nameof(Locations));
            }
        }
        #endregion
        #region Constructors
        internal MainWindowViewModel(MainWindow view)
        {
            this.view = view;
            Employees = LoadEmpolyees();
            Locations = LoadLocations();
            selectedEmployee = new tblEmployee();
            Employee = new vwEmployee();
            AddLocations();
            workerDelete.DoWork += LogDeletedEmployee;
        }
        #endregion

        #region Methods

        private List<tblLocation> LoadLocations()
        {
            try
            {
                var db = new DataAccess();
                return db.LoadLocations();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        private void LogDeletedEmployee(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2000);
            Logger.Instance.Log($"[{DateTime.Now.ToString("dd.MM.yyyy hh: mm")}] Deleted employee with ID: '{employeeID}'");
        }

        private List<vwEmployee> LoadEmpolyees()
        {
            try
            {
                return db.LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        private readonly string locationsPath = @"..\Locations.txt";
        public void AddLocations()
        {
            if (!File.Exists(locationsPath))
            {
                File.WriteAllLines(locationsPath, new string[]
                {
                    "Kosovska 20, Novi Sad, Serbia",
                    "Zeleznicka 3, Novi Sad, Serbia",
                    "Mise Dimitrijevica 15, Novi Sad, Serbia"
                });
            }
            var locationsArr = File.ReadAllLines(locationsPath);
            var parsedLocations = ParseToLocations(locationsArr);
            var dataAccess = new DataAccess();
            if (!Locations.Any())
            {
                if (parsedLocations.Any())
                {
                    foreach (var locationToAdd in parsedLocations)
                    {
                        dataAccess.AddNewLocation(locationToAdd);
                    }
                }
            }
        }

        private List<tblLocation> ParseToLocations(string[] arr)
        {
            var locations = new List<tblLocation>();
            if (arr.Length > 0)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var location = new tblLocation();
                    location.Adress = arr[i].Split(',')[0];
                    location.Place = arr[i].Split(',')[1].TrimStart();
                    location.State = arr[i].Split(',')[2].TrimStart();

                    locations.Add(location);
                }
            }
            return locations;
        }
        #endregion
        #region Commands
        //deleting employee
        private ICommand deleteEmployee;
        public ICommand DeleteEmployee
        {
            get
            {
                if (deleteEmployee == null)
                {
                    deleteEmployee = new RelayCommand(param => DeleteEmployeeExecute(), param => CanDeleteEmployee());
                }
                return deleteEmployee;
            }
        }

        private bool CanDeleteEmployee()
        {
            if (Employee == null)
                return false;
            return true;
        }

        private void DeleteEmployeeExecute()
        {
            try
            {
                if (Employee != null)
                {
                    employeeID = Employee.EmployeeID;
                    bool isExistingEmployee = db.IsExistingEmployee(employeeID);

                    if (isExistingEmployee == true)
                    {
                        DeleteEmployeeView deleteEmployee = new DeleteEmployeeView();
                        deleteEmployee.ShowDialog();
                        if ((deleteEmployee.DataContext as DeleteEmployeeViewModel).ShouldDelete == true)
                        {
                            db.DeleteEmployee(employeeID);
                            workerDelete.RunWorkerAsync();

                            Employees = LoadEmpolyees();
                        }
                    }
                    else
                    {
                        MessageBox.Show("[ERROR]");
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //updating employee

        private ICommand updateEmployee;
        public ICommand UpdateEmployee
        {
            get
            {
                if (updateEmployee == null)
                {
                    updateEmployee = new RelayCommand(param => UpdateEmployeeExecute(), param => CanUpdateEmployee());
                }
                return updateEmployee;
            }
        }

        private bool CanUpdateEmployee()
        {
            if (Employee == null)
                return false;
            return true;
        }

        private void UpdateEmployeeExecute()
        {
            try
            {
                if (Employee != null)
                {
                    int employeeID = Employee.EmployeeID;
                    bool isExistingEmployee = db.IsExistingEmployee(employeeID);

                    if (isExistingEmployee == true)
                    {
                        selectedEmployee = db.LoadEmployee(employeeID);
                        UpdateEmployeeView updateEmployee = new UpdateEmployeeView(selectedEmployee);
                        updateEmployee.ShowDialog();
                        if ((updateEmployee.DataContext as UpdateEmployeeViewModel).IsUpdatedEmployee == true)
                        {
                            Employees = LoadEmpolyees();
                        }
                    }
                    else
                    {
                        MessageBox.Show("[ERROR]");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        //adding new emloyee

        private ICommand addNewEmployee;
        public ICommand AddNewEmployee
        {
            get
            {
                if (addNewEmployee == null)
                {
                    addNewEmployee = new RelayCommand(param => AddNewEmployeeExecute(), param => CanAddNewEmployee());
                }
                return addNewEmployee;
            }
        }

        private void AddNewEmployeeExecute()
        {
            try
            {
                AddNewEmployeeView addNewEmployeeView = new AddNewEmployeeView();
                addNewEmployeeView.ShowDialog();

                if ((addNewEmployeeView.DataContext as AddNewEmployeeViewModel).IsAddedNewEmployee == true)
                {
                    Employees = LoadEmpolyees();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanAddNewEmployee()
        {
            return true;
        }
        #endregion
    }
}
