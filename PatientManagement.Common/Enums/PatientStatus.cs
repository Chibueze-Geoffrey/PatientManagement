using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Common.Enums
{
    public enum PatientStatus
    {
        [Display(Name = "Active", Description = "0")]
        Active = 0,

        [Display(Name = "Inactive", Description = "1")]
        Inactive = 1,

        [Display(Name = "Discharged", Description = "2")]
        Discharged = 2,

        [Display(Name = "Deleted", Description = "3")]
        Deleted = 3
    }
    public enum LogEnumMode
    {
        Warning,
        Error,
        Request,
        Response
    }
}
