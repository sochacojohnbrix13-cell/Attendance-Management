using System;
using System.Collections.Generic;
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
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nATTENDANCE MANAGEMENT SYSTEM");
                Console.WriteLine("1. Add Attendance");
                Console.WriteLine("2. View All");
                Console.WriteLine("3. Update Attendance");
                Console.WriteLine("4. Delete Attendance");
                Console.WriteLine("5. Exit");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddAttendance();
                        break;
                    case "2":
                        ViewAll();
                        break;
                    case "3":
                        UpdateAttendance();
                        break;
                    case "4":
                        DeleteAttendance();
                        break;
                    case "5":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }

        static void AddAttendance()
        {
            Console.Write("Enter Student Name: ");
            string name = Console.ReadLine();

            Attendance attendance = new Attendance
            {
                AttendanceId = Guid.NewGuid(),
                StudentName = name,
                Records = new List<string>()
            };

            RecordAttendance(attendance);

            attendanceService.AddAttendance(attendance);

            ShowSummary(attendance);

            Console.WriteLine("Attendance added!");
            attendanceLogs.Clear();
        }

        static void ViewAll()
        {
            var list = attendanceService.GetAttendances();

            foreach (var item in list)
            {
                Console.WriteLine($"\nID: {item.AttendanceId}");
                Console.WriteLine($"Name: {item.StudentName}");

                for (int i = 0; i < item.Records.Count; i++)
                {
                    Console.WriteLine($"Day {i + 1}: {item.Records[i]}");
                }

                var summary = attendanceService.GetSummary(item.Records);
                Console.WriteLine($"Present: {summary.present}, Absent: {summary.absent}, %: {summary.percentage:F2}");
            }
        }

        static void UpdateAttendance()
        {
            Console.Write("Enter Attendance ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Invalid ID");
                return;
            }

            Console.Write("Enter New Name: ");
            string name = Console.ReadLine();

            Attendance updated = new Attendance
            {
                AttendanceId = id,
                StudentName = name,
                Records = new List<string>()
            };

            RecordAttendance(updated);

            if (attendanceService.UpdateAttendance(updated))
            {
                Console.WriteLine("Updated successfully!");
                ShowSummary(updated);
            }
            else
            {
                Console.WriteLine("Record not found");
            }

            attendanceLogs.Clear();
        }

        static void DeleteAttendance()
        {
            Console.Write("Enter Attendance ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Invalid ID");
                return;
            }

            if (attendanceService.DeleteAttendance(id))
                Console.WriteLine("Deleted successfully!");
            else
                Console.WriteLine("Record not found");
        }

        static void RecordAttendance(Attendance attendance)
        {
            Console.Write("How many days to record? ");
            if (!int.TryParse(Console.ReadLine(), out int totalDays) || totalDays <= 0)
            {
                Console.WriteLine("Invalid number");
                return;
            }

            int presentCount = 0;

            for (int i = 0; i < totalDays; i++)
            {
                Console.Write($"Day {i + 1} (P/A): ");
                string input = Console.ReadLine();

                string status;
                if (input.Equals("P", StringComparison.OrdinalIgnoreCase))
                {
                    status = "Present";
                    presentCount++;
                }
                else
                {
                    status = "Absent";
                }

                attendance.Records.Add(status);
            }
            attendance.Days = presentCount;
        }

        static void ShowSummary(Attendance attendance)
        {
            Console.WriteLine($"\nAttendance Summary for: {attendance.StudentName}");
            Console.WriteLine("Days Present: " + attendance.Days);

            for (int i = 0; i < attendance.Records.Count; i++)
            {
                Console.WriteLine($"Day {i + 1}: {attendance.Records[i]}");
            }

            double percentage = attendance.Records.Count > 0
                ? ((double)attendance.Days / attendance.Records.Count) * 100
                : 0;

            Console.WriteLine("Attendance Percentage: " + percentage.ToString("0.00") + "%\n");
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