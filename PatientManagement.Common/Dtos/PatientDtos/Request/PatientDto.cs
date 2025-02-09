using Newtonsoft.Json;
using PatientManagement.Common.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Common.Dtos.PatientDtos
{
    public class PatientDto
    {
        [JsonProperty("first_name")]
        [Required(ErrorMessage = "Enter your first name"), MinLength(3)]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        [Required(ErrorMessage = "Enter your last name"), MinLength(3)]
        public string LastName { get; set; }


        [JsonProperty("age")]
        [Required(ErrorMessage = "Enter a valid age"), Range(1, 200, ErrorMessage = "Age must be between 1 and 200")]
        [DefaultValue(0)]
        public int Age { get; set; }

        [JsonProperty("status")]
        [DefaultValue(PatientStatus.Active)]
        [EnumDataType(typeof(PatientStatus), ErrorMessage = "Status must be one of the following: Active (0), Inactive (1), Discharged (2), Deleted (3)")]
        [SwaggerSchema("Enter a valid status: Active (0), Inactive (1), Discharged (2), Deleted (3)")]
        public PatientStatus Status { get; set; }
    }
}
