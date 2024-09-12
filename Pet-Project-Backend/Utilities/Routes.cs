using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pet_Project_Backend.Utilities
{
    public static class Routes
    {
        public const string CreateEmployee = "employee/createEmployee";
        public const string UpdateEmployeeById = "employee/updateEmployee/{id}";
        public const string GetAllEmployees = "employee/getAllEmployees";
        public const string GetEmployeeById = "employee/getEmployeeById/{id}";
        public const string DeleteEmployeeById = "employee/deleteEmployeeById/{id}";
    }
}
