using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pet_Project_Backend.Calculations
{
    public class CalculateAge : Employee
    {
        public int AgeCalculation(string DOB)
        {
            string dateOfBirth = Regex.Replace(DOB, @"[\/]", "");

            int day = int.Parse(dateOfBirth.Substring(0, 2));
            int month = int.Parse(dateOfBirth.Substring(2, 2));
            int year = int.Parse(dateOfBirth.Substring(dateOfBirth.Length - 4));

            DateTime birthDate = new DateTime(year, month, day);

            DateTime today = DateTime.Now;

            int age = today.Year - birthDate.Year;

            if (today < new DateTime(today.Year, birthDate.Month, birthDate.Day))
            {
                age--;
            }

            return age;

        }
    }
}
