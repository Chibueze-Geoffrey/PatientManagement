using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Common.Dtos.Response
{
    public partial class LogModel
    {
        public long Id { get; set; }
        public string Action { get; set; }
        public object Request { get; set; }
        public DateTime? RequestTime { get; set; }
        public string LogMode { get; set; }
        public object Response { get; set; }
        public DateTime? ResponseTime { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedOn { get; set; }
        public double TimeSpent { get; set; }
    }
}
