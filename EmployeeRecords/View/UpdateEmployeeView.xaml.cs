using EmployeeRecords.Model;
using EmployeeRecords.ViewModel;
using System.Windows;

namespace EmployeeRecords.View
{
    /// <summary>
    /// Interaction logic for UpdateEmployeeView.xaml
    /// </summary>
    public partial class UpdateEmployeeView : Window
    {
        public UpdateEmployeeView(tblEmployee selectedEmployee)
        {
            InitializeComponent();
            this.DataContext = new UpdateEmployeeViewModel(this, selectedEmployee);
        }
    }
}
