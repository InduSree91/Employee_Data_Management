using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Pet_Project_Backend.Utilities
{
    public class Validations
    {
        public List<string> validation(Employee employee)
        {
            List<string> validationErrors = new List<string>();

            // Validating Name
            if (string.IsNullOrEmpty(employee.Name))
            {
                validationErrors.Add("Name should be mentioned.");
            }
            if (employee.Name.Length < 2 || employee.Name.Length > 100)
            {
                validationErrors.Add("Name should be between 2 to 100 characters.");
            }
            if (!Regex.IsMatch(employee.Name, @"^[A-Za-z]+(?:[\s][A-Za-z]+)*$"))
            {
                validationErrors.Add("Name is not valid. Name should only contain alphabets.");
            }

            // Validate Gender
            if(string.IsNullOrEmpty(employee.Gender))
            {
                validationErrors.Add("Gender should be mentioned.");
            }

            // Validate Date Of Birth
            if (string.IsNullOrEmpty(employee.DOB))
            {
                validationErrors.Add("Date of Birth should be mentioned.");
            }
            if (!Regex.IsMatch(employee.DOB, @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$"))
            {
                validationErrors.Add("Date should be mentioned in the format dd/MM/yyyy");
            }

            // Validating Phone Number
            if (string.IsNullOrEmpty(employee.Phone))
            {
                validationErrors.Add("Phone number should be mentioned.");
            }
            if (employee.Phone.Length != 10)
            {
                validationErrors.Add("Phone number should only contain 10 characters.");
            }
            if (!Regex.IsMatch(employee.Phone, @"^[0-9]{1,10}$"))
            {
                validationErrors.Add("Phone number is not valid. Phone number should only contains numbers.");
            }

            // Validating Email
            if (string.IsNullOrEmpty(employee.Email))
            {
                validationErrors.Add("Email should be mentioned");
            }
            if (!Regex.IsMatch(employee.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                validationErrors.Add("Email is not valid. Enter a valid email.");
            }

            // Validating Id
            if (string.IsNullOrEmpty(employee.Id))
            {
                validationErrors.Add("Id is null.");
            }
            return validationErrors;
        }
    }
}
