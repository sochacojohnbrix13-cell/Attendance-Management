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
        // Instance of the data service to manage attendance records
        private readonly AttendanceDataService dataService = new AttendanceDataService();

        // Method to add a new attendance record
        public void AddAttendance(Attendance attendance)
        {
            if (attendance != null)
                dataService.Add(attendance);
        }
        // Method to retrieve all attendance records
        public List<Attendance> GetAttendances()
        {
            return dataService.GetAttendances();
        }
        // Method to update an existing attendance record
        public bool UpdateAttendance(Attendance attendance)
        {
            return dataService.Update(attendance);
        }
        // Method to delete an attendance record by its ID
        public bool DeleteAttendance(Guid id)
        {
            return dataService.Delete(id);
        }
        // Method to calculate the summary of attendance records, including the count of present and absent days and the percentage of presence
        public (int present, int absent, double percentage) GetSummary(List<string> records)
        {
            if (records == null || records.Count == 0)
                return (0, 0, 0);

            int present = 0;
            int absent = 0;
            // Iterate through the attendance records and count the number of present and absent days
            foreach (var day in records)
            {
                if (day.Equals("Present", StringComparison.OrdinalIgnoreCase))
                    present++;
                else if (day.Equals("Absent", StringComparison.OrdinalIgnoreCase))
                    absent++;
            }
            // Calculate the percentage of presence based on the total number of records
            double percentage = (present / (double)records.Count) * 100;

            return (present, absent, percentage);
        }
        // Method to search for attendance records by student name, returning a list of matching records
        public List<Attendance> SearchByName(string name)
        {
            return dataService.GetAttendances()
                .Where(a => a.StudentName.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        // Method to retrieve an attendance record by its ID, returning the record if found or null if not found
        public Attendance? GetById(Guid id)
        {
            return dataService.GetById(id);
        }
    }
}