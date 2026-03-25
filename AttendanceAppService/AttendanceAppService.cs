using System;
using System.Collections.Generic;
using AttendanceManagementModels;
using AttendanceManagementDataService;

namespace AttendanceManagementAppService
{
    public class AttendanceAppService
    {
        private readonly AttendanceDataService dataService = new AttendanceDataService();

        public void AddAttendance(Attendance attendance)
        {
            if (attendance != null)
                dataService.Add(attendance);
        }

        public List<Attendance> GetAttendances()
        {
            return dataService.GetAttendances();
        }

        public bool UpdateAttendance(Attendance attendance)
        {
            return dataService.Update(attendance);
        }

        public bool DeleteAttendance(Guid id)
        {
            return dataService.Delete(id);
        }
        public (int present, int absent, double percentage) GetSummary(List<string> records)
        {
            if (records == null || records.Count == 0)
                return (0, 0, 0);

            int present = 0;
            int absent = 0;

            foreach (var day in records)
            {
                if (day.Equals("Present", StringComparison.OrdinalIgnoreCase))
                    present++;
                else if (day.Equals("Absent", StringComparison.OrdinalIgnoreCase))
                    absent++;
            }

            double percentage = (present / (double)records.Count) * 100;

            return (present, absent, percentage);
        }
    }
}