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
        public string StudentName { get; set; } = string.Empty;

        public List<string> Records { get; set; } = new List<string>();

        public int Days { get; set; }


    }
}