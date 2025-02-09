using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Common.Dtos.PatientDtos.Request
{
    public class PatientUpdateDto
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last")]
        public string LastName { get; set; }


        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("status")]

        public string Status { get; set; }
    }
}
