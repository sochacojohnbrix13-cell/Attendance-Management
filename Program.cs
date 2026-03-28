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
            // Main loop to display the menu and handle user input
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nATTENDANCE MANAGEMENT SYSTEM");
                Console.WriteLine("1. Add Attendance");
                Console.WriteLine("2. View All");
                Console.WriteLine("3. Update Attendance");
                Console.WriteLine("4. Delete Attendance");
                Console.WriteLine("5. Search Student");
                Console.WriteLine("6. View by ID");
                Console.WriteLine("7. Exit");
                Console.Write("Choose: ");
                // Read the user's choice from the console
                string choice = Console.ReadLine() ?? string.Empty;

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
                        SearchStudent();
                        break;
                    case "6":
                        ViewById();
                        break;
                    case "7":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
                Pause();
            }
        }

        // Method to add a new attendance record for a student
        static void AddAttendance()
        {
            Console.Write("Enter Student Name: ");
            string name = Console.ReadLine() ?? string.Empty;

            // Validate the student name input
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty");
                return;
            }

            Attendance attendance = new Attendance
            {
                AttendanceId = Guid.NewGuid(),
                StudentName = name,
                Records = new List<string>()
            };
            // Call the method to record attendance for the student
            RecordAttendance(attendance);
            // Add the attendance record to the service
            attendanceService.AddAttendance(attendance);
            // Display the summary of the attendance record
            ShowSummary(attendance);

            Console.WriteLine("Attendance added!");
            attendanceLogs.Clear();
        }
        // Method to view all attendance records
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

        // Method to update an existing attendance record
        static void UpdateAttendance()
        {
            Console.Write("Enter Attendance ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Invalid ID");
                return;
            }
            
            Console.Write("Enter New Name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Attendance updated = new Attendance
            {
                AttendanceId = id,
                StudentName = name,
                Records = new List<string>()
            };
            // Call the method to record attendance for the student
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
        // Method to delete an attendance record based on its ID
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
        // Method to view an attendance record by its ID
        static void SearchStudent()
        {
            Console.Write("Enter name to search: ");
            string name = Console.ReadLine() ?? string.Empty;

            var results = attendanceService.SearchByName(name);

            if (results.Count == 0)
            {
                Console.WriteLine("No results found.");
                return;
            }

            foreach (var item in results)
                ShowSummary(item);
        }
        // Method to view an attendance record by its ID
        static void ViewById()
        {
            Console.Write("Enter ID: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                var item = attendanceService.GetById(id);

                if (item != null)
                    ShowSummary(item);
                else
                    Console.WriteLine("Record not found");
            }
        }

        // Method to record attendance for a student, allowing the user to input the attendance status for a specified number of days
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
                string input;
                // Loop to ensure valid input for attendance status (Present/Absent)
                do
                {
                    Console.Write($"Day {i + 1} (P/A): ");
                    input = Console.ReadLine().ToUpper();
                }
                while (input != "P" && input != "A");

                if (input == "P")
                {
                    attendance.Records.Add("Present");
                    presentCount++;
                }
                else
                {
                    attendance.Records.Add("Absent");
                }
            }
            attendance.Days = presentCount;
        }

        // Method to display a summary of the attendance record for a student
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
        // Method to display the attendance logs for a specific student
        static void DisplayLogs(string studentName)
        {
            Console.WriteLine("Attendance Logs for: " + studentName);

            foreach (var log in attendanceLogs)
            {
                Console.WriteLine(log);
            }
        }
        // Method to pause the console and wait for user input before clearing the screen
        static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}