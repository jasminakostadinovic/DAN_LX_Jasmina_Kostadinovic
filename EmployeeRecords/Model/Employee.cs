namespace EmployeeRecords.Model
{
    public partial class vwEmployee
    {
        public string BirthDate
        {
            get
            {
                return DateOfBirth.ToShortDateString();
            }
        }
    }
}
