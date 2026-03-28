using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagementModels
{
    public class Attendance
    {
        //id for attendance record
        public Guid AttendanceId { get; set; }
        //name of the student
        public string StudentName { get; set; } = string.Empty;
        //list of attendance records (e.g., "Present", "Absent")
        public List<string> Records { get; set; } = new List<string>();
        //number of days recorded
        public int Days { get; set; }
        //calculated property to determine the number of absences based on the total records and days present
        public int Absences => Records.Count - Days;



    }
}