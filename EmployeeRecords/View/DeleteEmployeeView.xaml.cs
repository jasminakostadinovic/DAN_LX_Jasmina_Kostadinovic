using EmployeeRecords.ViewModel;
using System.Windows;

namespace EmployeeRecords.View
{
    /// <summary>
    /// Interaction logic for DeleteEmployeeView.xaml
    /// </summary>
    public partial class DeleteEmployeeView : Window
    {
        public DeleteEmployeeView()
        {
            InitializeComponent();
            this.DataContext = new DeleteEmployeeViewModel(this);
        }
    }
}
