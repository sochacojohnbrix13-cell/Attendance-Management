using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagementModels
{
    public class Attendance
    {
        public Guid AttendanceId { get; set; }
        public string StudentName { get; set; }
        public string[] Days { get; set; } = new string[7];
    }
}