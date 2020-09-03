namespace EmployeeRecords.Model
{
    public partial class tblLocation
    {
        public string FullAdress
        {
            get
            {
                return $"{Adress}, {Place}, {State}";
            }
        }
    }
}
