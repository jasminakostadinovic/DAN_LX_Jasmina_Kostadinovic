using System;
using System.Globalization;

namespace EmployeeRecords.Model
{
    class GeneratingData
    {
        public static DateTime GenerateBirthdate(string personalNo)
        {
            DateTime dateValue;
            string dateFromInput;
            if (personalNo[4] == '9')
                dateFromInput = "" + personalNo[2] + personalNo[3] + "/" + personalNo[0] + personalNo[1] + "/1" + personalNo[4] + personalNo[5] + personalNo[6] + " 00:00:00";
            else
                dateFromInput = "" + personalNo[2] + personalNo[3] + "/" + personalNo[0] + personalNo[1] + "/2" + personalNo[4] + personalNo[5] + personalNo[6] + " 00:00:00";
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var styles = DateTimeStyles.None;
            DateTime.TryParse(dateFromInput, culture, styles, out dateValue);
            return dateValue;
        }

        internal static int CalculateAge(DateTime birthdate)
        {
            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int dob = int.Parse(birthdate.ToString("yyyyMMdd"));
            int age = (now - dob) / 10000;
            return age;
        }
    }
}
