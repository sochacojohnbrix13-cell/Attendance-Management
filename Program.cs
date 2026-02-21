namespace Attendance_Management
{
    internal class Program
    {
        static string[] attendance = new string[7];
        static List<string> attendanceLogs = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("ATTENDANCE MANAGEMENT SYSTEM");

            bool isRecord = Option();

            while (isRecord)
            {
                Record();
                Summary();

                isRecord = Option();
            }

            DisplayLogs();
        }
        static bool Option()
        {
            Console.Write("Do you want to record attendance? yes/no: ");
            string input = Console.ReadLine();

            bool isRecord = false;

            switch (input)
            {
                case "yes":
                    isRecord = true;
                    break;
                case "no":
                    isRecord = false;
                    break;
                default:
                    Console.WriteLine("Invalid input. System will exit.");
                    Environment.Exit(0);
                    break;
            }

            return isRecord;
        }
        static void Record()
        {
            for (int i = 0; i < attendance.Length; i++)
            {
                Console.Write("Day " + (i + 1) + " (P/A): ");
                string input = Console.ReadLine();

                if (input == "P" || input == "p")
                {
                    attendance[i] = "Present";
                }
                else
                {
                    attendance[i] = "Absent";
                }

                AttendanceLog(i + 1, attendance[i]);
            }
        }
        static void AttendanceLog(int day, string status)
        {
            attendanceLogs.Add("Day " + day + " : " + status);
        }
        static void Summary()
        {
            int present = 0;
            int absent = 0;

            for (int i = 0; i < attendance.Length; i++)
            {
                if (attendance[i] == "Present")
                {
                    present++;
                }
                else
                {
                    absent++;
                }
            }

            double percentage = (present / 7.0) * 100;

            Console.WriteLine("\nAttendance Summary");
            Console.WriteLine("Present: " + present);
            Console.WriteLine("Absent : " + absent);
            Console.WriteLine("Percentage: " + percentage + "%\n");
        }
        static void DisplayLogs()
        {
            Console.WriteLine("Attendance Logs:");
            foreach (var log in attendanceLogs)
            {
                Console.WriteLine(log);
            }
        }
    }
}