using EmployeeRecords.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeRecords
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //hiding id columns
            if (e.Column.Header.ToString() == "EmployeeID"
                || e.Column.Header.ToString() == "LocationID"
                || e.Column.Header.ToString() == "SectorID"
                || e.Column.Header.ToString() == "DateOfBirth")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }
    }
}
