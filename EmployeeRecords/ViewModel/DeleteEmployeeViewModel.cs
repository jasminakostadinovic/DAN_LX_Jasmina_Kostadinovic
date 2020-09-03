using EmployeeRecords.Command;
using EmployeeRecords.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EmployeeRecords.ViewModel
{
    class DeleteEmployeeViewModel : ViewModelBase
    {
        #region Fields
        private readonly DeleteEmployeeView deleteEmployee;
        #endregion

        #region Properties
        public bool ShouldDelete { get; set; }
        #endregion

        #region Constructors
        public DeleteEmployeeViewModel(DeleteEmployeeView deleteEmployee)
        {
            this.deleteEmployee = deleteEmployee;

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
            return true;
        }

        private void SaveExecute()
        {
            ShouldDelete = true;
            deleteEmployee.Close();
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
            ShouldDelete = false;
            deleteEmployee.Close();
        }
        #endregion
    }
}
