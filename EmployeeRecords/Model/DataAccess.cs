using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecords.Model
{
    class DataAccess
    {
        public List<vwEmployee> LoadEmployees()
        {
            using (var context = new EmployeeRecordsEntities())
            {
                var employees = new List<vwEmployee>();

                if (context.vwEmployees.Any())
                {
                    foreach (var item in context.vwEmployees)
                    {
                        employees.Add(item);
                    }
                    return employees;
                }
                return employees;
            }
        }

        public List<tblLocation> LoadLocations()
        {
            using (var context = new EmployeeRecordsEntities())
            {
                var locations = new List<tblLocation>();

                if (context.tblLocations.Any())
                {
                    foreach (var item in context.tblLocations)
                    {
                        locations.Add(item);
                    }
                    return locations.OrderBy(o => o.Adress).ToList();
                }
                return locations;
            }
        }
        public void AddNewLocation(tblLocation location)
        {
            using (var context = new EmployeeRecordsEntities())
            {
                context.tblLocations.Add(location);
                context.SaveChanges();
            }
        }

        internal bool IsExistingEmployee(int employeeID)
        {
            using (var context = new EmployeeRecordsEntities())
            {
                return context.tblEmployees.Any(e => e.EmployeeID == employeeID);
            }
        }

        internal void DeleteEmployee(int employeeID)
        {
            using (var context = new EmployeeRecordsEntities())
            {
                var employeeToRemove = context.tblEmployees.FirstOrDefault(e => e.EmployeeID == employeeID);
                if (employeeToRemove != null)
                {
                    context.tblEmployees.Remove(employeeToRemove);
                    context.SaveChanges();
                }
            }
        }

        internal tblEmployee LoadEmployee(int employeeID)
        {
            using (var context = new EmployeeRecordsEntities())
            {
                if (context.tblEmployees.Any(e => e.EmployeeID == employeeID))
                {
                    return context.tblEmployees.First(e => e.EmployeeID == employeeID);
                }
                return new tblEmployee();
            }
        }

        internal List<string> LoadManagers(int employeeID)
        {
            using (var context = new EmployeeRecordsEntities())
            {
                var managers = new List<string>();
                if (context.tblEmployees.Any())
                {
                    foreach (var item in context.tblEmployees)
                    {
                        if (item.EmployeeID != employeeID)
                            managers.Add(item.GivenName + " " + item.Surname);
                    }
                    return managers;
                }
                return managers;
            }
        }

        internal void UpdateEmployee(int employeeID, tblEmployee employee)
        {
            using (var context = new EmployeeRecordsEntities())
            {
                if (context.tblEmployees.Any(e => e.EmployeeID == employeeID))
                {
                    var employeeToUpdate = context.tblEmployees.First(e => e.EmployeeID == employeeID);
                    employeeToUpdate.GivenName = employee.GivenName;
                    employeeToUpdate.Surname = employee.Surname;
                    employeeToUpdate.Sex = employee.Sex;
                    employeeToUpdate.PersonalNo = employee.PersonalNo;
                    employeeToUpdate.RegistrationNumber = employee.RegistrationNumber;
                    employeeToUpdate.Telephone = employee.Telephone;
                    employeeToUpdate.LocationID = employee.LocationID;
                    employeeToUpdate.Manager = employee.Manager;
                    employeeToUpdate.SectorID = employee.SectorID;

                    context.SaveChanges();
                }
            }
        }

        internal List<string> LoadPersonalNumbers()
        {
            using (var context = new EmployeeRecordsEntities())
            {
                var personalNumbers = new List<string>();
                if (context.tblEmployees.Any())
                {
                    foreach (var item in context.tblEmployees)
                    {
                        personalNumbers.Add(item.PersonalNo);
                    }
                    return personalNumbers;
                }
                return personalNumbers;
            }
        }

        internal List<tblSector> LoadSectors()
        {
            using (var context = new EmployeeRecordsEntities())
            {
                var sectors = new List<tblSector>();

                if (context.tblSectors.Any())
                {
                    foreach (var item in context.tblSectors)
                    {
                        sectors.Add(item);
                    }
                    return sectors;
                }
                return sectors;
            }
        }

        internal void AddNewSector(tblSector newSector)
        {
            using (var context = new EmployeeRecordsEntities())
            {
                context.tblSectors.Add(newSector);
                context.SaveChanges();
            }
        }

        internal void AddNewEmployee(tblEmployee employee)
        {
            using (var context = new EmployeeRecordsEntities())
            {
                context.tblEmployees.Add(employee);
                context.SaveChanges();
            }
        }

        internal List<string> LoadRegistrationNumbers()
        {
            using (var context = new EmployeeRecordsEntities())
            {
                var registrationNumbers = new List<string>();
                if (context.tblEmployees.Any())
                {
                    foreach (var item in context.tblEmployees)
                    {
                        registrationNumbers.Add(item.RegistrationNumber);
                    }
                    return registrationNumbers;
                }
                return registrationNumbers;
            }
        }
    }
}
