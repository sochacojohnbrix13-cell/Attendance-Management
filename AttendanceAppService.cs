using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceManagementModels;
using AttendanceManagementDataService;

namespace AttendanceManagementAppService
{
    public class AttendanceAppService
    {
        AttendanceDataService dataService = new AttendanceDataService();

        public void AddAttendance(Attendance attendance)
        {
            dataService.Add(attendance);
        }

        public (int present, int absent, double percentage) GetSummary(string[] attendance)
        {
            int present = 0;
            int absent = 0;

            foreach (var day in attendance)
            {
                if (day == "Present")
                    present++;
                else
                    absent++;
            }

            double percentage = (present / (double)attendance.Length) * 100;

            return (present, absent, percentage);
        }

        public List<Attendance> GetAttendances()
        {
            return dataService.GetAttendances();
        }
    }
}
