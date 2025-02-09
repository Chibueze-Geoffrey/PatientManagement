using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Common.Dtos.PatientDtos
{
    public class PatientDto
    {
        [JsonProperty("first_name")]
        [Required(ErrorMessage = "Enter your first name")]
        public string FirstName { get; set; }

        [JsonProperty("last")]
        [Required(ErrorMessage = "Enter your last name")]
        public string LastName { get; set; }


        [JsonProperty("age")]
        [Required(ErrorMessage = "Enter a valid age")]
        public int Age { get; set; }

        [JsonProperty("status")]

        [Required(ErrorMessage = "Enter a valid status, 0 for active,  1 for inactive,  2 for discharged,   3 for deleted")]
        public string Status { get; set; }
    }
}
