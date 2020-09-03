using EmployeeRecords.ViewModel;
using System.Windows;

namespace EmployeeRecords.View
{
    /// <summary>
    /// Interaction logic for AddNewEmployeeView.xaml
    /// </summary>
    public partial class AddNewEmployeeView : Window
    {
        public AddNewEmployeeView()
        {
            InitializeComponent();
            this.DataContext = new AddNewEmployeeViewModel(this);
        }
    }
}
