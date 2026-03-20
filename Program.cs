using System;
using AttendanceManagementModels;
using AttendanceManagementAppService;

namespace Attendance_Management
{
    internal class Program
    {
        static List<string> attendanceLogs = new List<string>();
        static AttendanceAppService attendanceService = new AttendanceAppService();

        static void Main(string[] args)
        {
            Console.WriteLine("ATTENDANCE MANAGEMENT SYSTEM");

            bool addAnotherStudent = true;

            while (addAnotherStudent)
            {
                Console.Write("Enter Student Name: ");
                string studentName = Console.ReadLine();

                Attendance newAttendance = new Attendance
                {
                    AttendanceId = Guid.NewGuid(),
                    StudentName = studentName
                };

                RecordAttendance(newAttendance);

                attendanceService.AddAttendance(newAttendance);

                ShowSummary(newAttendance);

                DisplayLogs(studentName);

                Console.Write("\nDo you want to record another attendance? yes/no: ");
                string choice = Console.ReadLine().ToLower();

                if (choice != "yes")
                    addAnotherStudent = false;

                attendanceLogs.Clear();
            }
        }

        static void RecordAttendance(Attendance attendance)
        {
            for (int i = 0; i < attendance.Days.Length; i++)
            {
                Console.Write("Day " + (i + 1) + " (P/A): ");
                string input = Console.ReadLine();

                if (input.Equals("P", StringComparison.OrdinalIgnoreCase))
                    attendance.Days[i] = "Present";
                else
                    attendance.Days[i] = "Absent";

                attendanceLogs.Add("Day " + (i + 1) + " : " + attendance.Days[i]);
            }
        }

        static void ShowSummary(Attendance attendance)
        {
            var result = attendanceService.GetSummary(attendance.Days);

            Console.WriteLine("\nAttendance Summary for: " + attendance.StudentName);
            Console.WriteLine("Present: " + result.present);
            Console.WriteLine("Absent : " + result.absent);
            Console.WriteLine("Percentage: " + result.percentage.ToString("0.00") + "%\n");
        }

        static void DisplayLogs(string studentName)
        {
            Console.WriteLine("Attendance Logs for: " + studentName);

            foreach (var log in attendanceLogs)
            {
                Console.WriteLine(log);
            }
        }
    }
}