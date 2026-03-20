using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceManagementModels;

namespace AttendanceManagementDataService
{
    public class AttendanceDataService
    {
        private List<Attendance> attendances = new List<Attendance>();

        public void Add(Attendance attendance)
        {
            attendances.Add(attendance);
        }

        public List<Attendance> GetAttendances()
        {
            return attendances;
        }

        public Attendance? GetById(Guid id)
        {
            return attendances.FirstOrDefault(a => a.AttendanceId == id);
        }
    }
}
