using EmployeeRecords.Command;
using EmployeeRecords.Loggers;
using EmployeeRecords.Model;
using EmployeeRecords.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Validations;

namespace EmployeeRecords.ViewModel
{
    class UpdateEmployeeViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Fields
        private UpdateEmployeeView updateEmployeeView;
        private tblLocation location;
        private BackgroundWorker workerUpdate = new BackgroundWorker();
        private List<tblLocation> locations;
        private tblEmployee employee;
        private string surname;
        private string givenName;
        private string personalNo;
        private string sex;
        private string telephone;
        private string registrationNumber;
        private List<string> managers;
        private string manager;
        private string sectorName;
        private tblSector sector;
        private List<tblSector> sectors;
        #endregion

        #region Properties
        public bool CanSave { get; set; }
        public int EmployeeAge { get; private set; }
        public tblEmployee PreviousEmployeeData { get; set; }

        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                if (surname == value) return;
                surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        public string GivenName
        {
            get
            {
                return givenName;
            }
            set
            {
                if (givenName == value) return;
                givenName = value;
                OnPropertyChanged(nameof(GivenName));
            }
        }
        public tblEmployee Employee
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
        public List<tblSector> Sectors
        {
            get
            {
                return sectors;
            }
            set
            {
                if (sectors == value) return;
                sectors = value;
                OnPropertyChanged(nameof(Sectors));
            }
        }

        public tblSector Sector

        {
            get
            {
                return sector;
            }
            set
            {
                sector = value;
                OnPropertyChanged(nameof(Sector));
            }
        }
        public string SectorName
        {
            get
            {
                return sectorName;
            }
            set
            {
                sectorName = value;
                OnPropertyChanged(nameof(SectorName));
            }
        }

        public string PersonalNo
        {
            get
            {
                return personalNo;
            }
            set
            {
                if (personalNo == value) return;
                personalNo = value;
                OnPropertyChanged(nameof(PersonalNo));
            }
        }

        public string Sex
        {
            get
            {
                return sex;
            }
            set
            {
                sex = value;
                OnPropertyChanged(nameof(Sex));
            }
        }

        public string Telephone
        {
            get
            {
                return telephone;
            }
            set
            {
                telephone = value;
                OnPropertyChanged(nameof(Telephone));
            }
        }

        public string RegistrationNumber
        {
            get
            {
                return registrationNumber;
            }
            set
            {
                registrationNumber = value;
                OnPropertyChanged(nameof(RegistrationNumber));
            }
        }

        public List<string> Managers
        {
            get
            {
                return managers;
            }
            set
            {
                managers = value;
                OnPropertyChanged(nameof(Managers));
            }
        }
        public string Manager
        {
            get
            {
                return manager;
            }
            set
            {
                manager = value;
                OnPropertyChanged(nameof(Manager));
            }
        }
        public bool IsUpdatedEmployee { get; internal set; }
        public tblLocation Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                OnPropertyChanged(nameof(Location));
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
        public bool IsUpdatedClient { get; internal set; }
        #endregion

        #region Constructors
        public UpdateEmployeeViewModel(UpdateEmployeeView updateEmployeeView, tblEmployee selectedEmployee)
        {
            workerUpdate.DoWork += LogUpdatedEmployee;
            this.updateEmployeeView = updateEmployeeView;
            Sectors = LoadSectors();
            Managers = LoadManagers(selectedEmployee.EmployeeID);
            Locations = LoadLocations();
            Employee = new tblEmployee();
            PreviousEmployeeData = new tblEmployee();
            Surname = selectedEmployee.Surname;
            GivenName = selectedEmployee.GivenName;
            PersonalNo = selectedEmployee.PersonalNo;
            Telephone = selectedEmployee.Telephone;
            RegistrationNumber = selectedEmployee.RegistrationNumber;
            Sex = selectedEmployee.Sex;
            Manager = selectedEmployee.Manager;
            Sector = Sectors.FirstOrDefault(s => s.SectorID == selectedEmployee.SectorID);
            SectorName = Sectors.FirstOrDefault(s => s.SectorID == selectedEmployee.SectorID).Name;
            Location = Locations.FirstOrDefault(x => x.LocationID == selectedEmployee.LocationID);

            Employee.EmployeeID = selectedEmployee.EmployeeID;
            Employee.GivenName = selectedEmployee.GivenName;
            Employee.Surname = selectedEmployee.Surname;
            Employee.PersonalNo = selectedEmployee.PersonalNo;
            Employee.LocationID = selectedEmployee.LocationID;
            Employee.Sex = selectedEmployee.Sex;
            Employee.RegistrationNumber = selectedEmployee.RegistrationNumber;
            Employee.SectorID = selectedEmployee.SectorID;
            Employee.Telephone = selectedEmployee.Telephone;
            Employee.Manager = selectedEmployee.Manager;

            //keeping previous client data in property 
            PreviousEmployeeData.GivenName = selectedEmployee.GivenName;
            PreviousEmployeeData.Surname = selectedEmployee.Surname;
            PreviousEmployeeData.PersonalNo = selectedEmployee.PersonalNo;
            PreviousEmployeeData.LocationID = selectedEmployee.LocationID;
            PreviousEmployeeData.Sex = selectedEmployee.Sex;
            PreviousEmployeeData.RegistrationNumber = selectedEmployee.RegistrationNumber;
            PreviousEmployeeData.SectorID = selectedEmployee.SectorID;
            PreviousEmployeeData.Telephone = selectedEmployee.Telephone;
            PreviousEmployeeData.Manager = selectedEmployee.Manager;

            CanSave = true;
        }

        private void LogUpdatedEmployee(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2000);
            Logger.Instance.Log($"[{DateTime.Now.ToString("dd.MM.yyyy hh: mm")}] Updated employee with ID: '{Employee.EmployeeID}'");
        }

        #endregion

        #region Methods

        private List<tblSector> LoadSectors()
        {
            try
            {
                var db = new DataAccess();
                return db.LoadSectors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

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

        private List<string> LoadManagers(int employeeID)
        {
            try
            {
                var db = new DataAccess();
                return db.LoadManagers(employeeID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        #endregion

        #region Commands
        private ICommand save;
        public ICommand Save
        {
            get
            {
                if (save == null)
                {
                    save = new RelayCommand(param => SaveExecute(), param => CanSaveExecute());
                }
                return save;
            }
        }

        private bool CanSaveExecute()
        {
            if (PersonalNo == PreviousEmployeeData.PersonalNo
                && GivenName == PreviousEmployeeData.GivenName
                && Surname == PreviousEmployeeData.Surname
                && Location.LocationID == PreviousEmployeeData.LocationID
                && Sex.ToLower() == PreviousEmployeeData.Sex.ToLower()
                && Telephone == PreviousEmployeeData.Telephone
                && Employee.SectorID == PreviousEmployeeData.SectorID
                && Manager == PreviousEmployeeData.Manager
                && SectorName == GetPreviousEmployeeSectorName()
                && Employee.RegistrationNumber == PreviousEmployeeData.RegistrationNumber)
                return false;
            if (string.IsNullOrWhiteSpace(GivenName)
                || string.IsNullOrWhiteSpace(Surname)
                || string.IsNullOrWhiteSpace(Telephone)
                || string.IsNullOrWhiteSpace(RegistrationNumber)
                || string.IsNullOrWhiteSpace(PersonalNo)
                || string.IsNullOrWhiteSpace(Sex))
                return false;
            if (CanSave == false)
                return false;
            return true;
        }

        private string GetPreviousEmployeeSectorName()
        {
            return Sectors.FirstOrDefault(s => s.SectorID == PreviousEmployeeData.SectorID).Name;
        }

        private void SaveExecute()
        {
            try
            {
                var db = new DataAccess();
                Employee.Surname = Surname;
                Employee.GivenName = GivenName;
                Employee.LocationID = Location.LocationID;
                Employee.Sex = Sex.ToLower();
                Employee.PersonalNo = PersonalNo;
                Employee.DateOfBirth = GeneratingData.GenerateBirthdate(Employee.PersonalNo);
                EmployeeAge = GeneratingData.CalculateAge(Employee.DateOfBirth);
                Employee.Manager = Manager;
                Employee.Telephone = Telephone;
                Employee.RegistrationNumber = RegistrationNumber;
                if (Sectors.Any(s => s.Name == SectorName))
                {
                    Employee.SectorID = Sectors.First(s => s.Name == SectorName).SectorID;
                }
                else
                {
                    var newSector = new tblSector() { Name = SectorName };
                    //adding new sector to database
                    db.AddNewSector(newSector);
                    Sectors = LoadSectors();
                    Employee.SectorID = Sectors.FirstOrDefault(s => s.Name == SectorName).SectorID;
                }

                //updating employee
                db.UpdateEmployee(Employee.EmployeeID, Employee);

                workerUpdate.RunWorkerAsync();

                IsUpdatedEmployee = true;
                updateEmployeeView.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private ICommand exit;


        public ICommand Exit
        {
            get
            {
                if (exit == null)
                {
                    exit = new RelayCommand(param => ExitExecute(), param => CanExitExecute());
                }
                return exit;
            }
        }

        private bool CanExitExecute()
        {
            return true;
        }

        private void ExitExecute()
        {
            IsUpdatedClient = false;
            updateEmployeeView.Close();
        }

        #endregion

        #region IDataErrorInfoImplementation
        //validations

        public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string name]
        {
            get
            {
                var validate = new Validation();
                var db = new DataAccess();
                string validationMessage = string.Empty;

                if (name == "PersonalNo")
                {
                    if (!validate.IsValidPersonalNoFormat(PersonalNo))
                    {
                        validationMessage = "Invalid personal number format!";
                        CanSave = false;
                    }
                    if (PersonalNo != Employee.PersonalNo)
                    {
                        if (!validate.IsUniquePersonalNo(PersonalNo, db.LoadPersonalNumbers()))
                        {
                            validationMessage = "Personal number must be unique!";
                            CanSave = false;
                        }
                    }

                    //if peronal number is valid we can check the age of user
                    if (validationMessage == string.Empty)
                    {
                        EmployeeAge = GeneratingData.CalculateAge(GeneratingData.GenerateBirthdate(PersonalNo));
                        if (EmployeeAge < 16)
                        {
                            validationMessage = "Persons younger than 16 years old can not be employed.";
                            CanSave = false;
                        }
                    }

                }
                else if (name == "Sex")
                {
                    if (!string.IsNullOrWhiteSpace(Sex))
                    {
                        var sexToLower = Sex.ToLower();
                        if (sexToLower != "m" && sexToLower != "f" && sexToLower != "x")
                        {
                            validationMessage = "Please use letter 'm' for male,'f' for female sex or 'x' for other!";
                            CanSave = false;
                        }
                    }

                }
                else if (name == "RegistrationNumber")
                {
                    if (RegistrationNumber.Length != 9 || !validate.IsDigitsOnly(RegistrationNumber))
                    {
                        validationMessage = "Invalid registration number format!";
                        CanSave = false;
                    }

                    if (RegistrationNumber != Employee.RegistrationNumber)
                    {
                        if (!validate.IsUniqueRegistrationNo(RegistrationNumber, db.LoadRegistrationNumbers()))
                        {
                            validationMessage = "Registartion number must be unique!";
                            CanSave = false;
                        }
                    }
                }
                if (string.IsNullOrEmpty(validationMessage))
                    CanSave = true;
                return validationMessage;
            }
        }
        #endregion
    }
}
