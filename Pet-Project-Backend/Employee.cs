using Newtonsoft.Json;
using Pet_Project_Backend.Calculations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pet_Project_Backend
{
    public class Employee
    {
        [JsonProperty("name")]        
        public string Name { get; set; }

        [JsonProperty("date_of_birth")]
        public string DOB { get; set; }

        [JsonProperty("phone_number")]
        public string Phone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }
    }
}