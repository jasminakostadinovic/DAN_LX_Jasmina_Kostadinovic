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
    class AddNewEmployeeViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Fields
        private BackgroundWorker workerAddNew = new BackgroundWorker();
        private readonly AddNewEmployeeView addNewEmployeeView;
        private tblLocation location;
        private List<tblLocation> locations;
        private tblEmployee employee;
        private tblSector sector;
        private List<tblSector> sectors;
        private string sectorName;
        private List<string> managers;
        private string personalNo;
        private string registrationNumber;
        private string sex;
        #endregion

        #region Properties
        public bool IsAddedNewEmployee { get; internal set; }
        public bool CanSave { get; set; }
        public int EmployeeAge { get; private set; }
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
                if (sector == value) return;
                sector = value;
                OnPropertyChanged(nameof(Sector));
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
        #endregion

        #region Constructors
        public AddNewEmployeeViewModel(AddNewEmployeeView addNewEmployeeView)
        {
            Employee = new tblEmployee();
            this.addNewEmployeeView = addNewEmployeeView;
            PersonalNo = string.Empty;
            Sex = string.Empty;
            Employee.Telephone = string.Empty;
            RegistrationNumber = string.Empty;
            SectorName = string.Empty;
            Employee.Manager = string.Empty;
            Employee.Surname = string.Empty;
            Employee.GivenName = string.Empty;
            Locations = LoadLocations();
            Sector = new tblSector();
            CanSave = true;
            Location = new tblLocation();
            Sectors = LoadSectors();
            Managers = LoadManagers();
            workerAddNew.DoWork += LogAddedNewEmployee;
        }

        private void LogAddedNewEmployee(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2000);
            Logger.Instance.Log($"[{DateTime.Now.ToString("dd.MM.yyyy hh: mm")}] Created new employee with registration number: '{Employee.RegistrationNumber}'");
        }

        #endregion

        #region Methods
        private List<string> LoadManagers()
        {
            try
            {
                var db = new DataAccess();
                return db.LoadManagers(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private List<tblSector> LoadSectors()
        {
            try
            {
                var db = new DataAccess();
                return db.LoadSectors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
                return null;
            }
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
                CanSave = true;
                var validate = new Validation();
                var db = new DataAccess();
                string validationMessage = string.Empty;

                if (name == nameof(PersonalNo))
                {
                    if (!validate.IsValidPersonalNoFormat(PersonalNo))
                    {
                        validationMessage = "Invalid personal number format!";
                        CanSave = false;
                    }
                    if (!validate.IsUniquePersonalNo(PersonalNo, db.LoadPersonalNumbers()))
                    {
                        validationMessage = "Personal number must be unique!";
                        CanSave = false;
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
                else if (name == nameof(Sex))
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
                else if (name == nameof(RegistrationNumber))
                {
                    if (RegistrationNumber.Length != 9 || !validate.IsDigitsOnly(RegistrationNumber))
                    {
                        validationMessage = "Invalid registration number format!";
                        CanSave = false;
                    }
                    if (!validate.IsUniqueRegistrationNo(RegistrationNumber, db.LoadRegistrationNumbers()))
                    {
                        validationMessage = "Registartion number must be unique!";
                        CanSave = false;
                    }
                }
                if (string.IsNullOrEmpty(validationMessage))
                    CanSave = true;

                return validationMessage;
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
            if (
                string.IsNullOrWhiteSpace(Employee.GivenName)
                || string.IsNullOrWhiteSpace(Employee.Surname)
                || string.IsNullOrWhiteSpace(Sex)
                || string.IsNullOrWhiteSpace(Employee.Telephone)
                || string.IsNullOrWhiteSpace(RegistrationNumber)
                || string.IsNullOrWhiteSpace(PersonalNo)
                || Location.LocationID == 0
                || string.IsNullOrWhiteSpace(SectorName)
                || CanSave == false)
                return false;
            return true;
        }
        private void SaveExecute()
        {
            try
            {
                var db = new DataAccess();
                Employee.Sex = Sex.ToLower();
                Employee.RegistrationNumber = RegistrationNumber;
                Employee.PersonalNo = PersonalNo;
                Employee.LocationID = Location.LocationID;
                Employee.DateOfBirth = GeneratingData.GenerateBirthdate(Employee.PersonalNo);
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


                //adding new employee to database 
                db.AddNewEmployee(Employee);

                workerAddNew.RunWorkerAsync();

                IsAddedNewEmployee = true;

                addNewEmployeeView.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Escaping action
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
            IsAddedNewEmployee = false;
            addNewEmployeeView.Close();
        }
        #endregion
    }
}
