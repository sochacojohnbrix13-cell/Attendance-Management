namespace Attendance_Management
{
    internal class Program
    {
        static string studentName;
        static string[] attendance = new string[7];
        static List<string> attendanceLogs = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("ATTENDANCE MANAGEMENT SYSTEM");



            bool addAnotherStudent = true;

            while (addAnotherStudent)
            {
                InputStudentName();
                bool isRecord = Option();

                while (isRecord)
                {
                    Record();
                    Summary();

                    isRecord = Option();
                }


                DisplayLogs();
                Console.Write("\nDo you want to record another attendance? yes/no: ");
                string choice = Console.ReadLine().ToLower();
                if (choice != "yes")
                {
                    addAnotherStudent = false;
                }
                attendanceLogs.Clear();
                attendance = new string[7];

            }
        }
        static void InputStudentName()
        {
            Console.Write("Enter Students Name: ");
            studentName = Console.ReadLine();
            Console.WriteLine("\nRecording attendance for " + studentName + "\n");
        }
        static bool Option()
        {
            Console.Write("Do you want to record attendance? yes/no: ");
            string input = Console.ReadLine().ToLower();

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
                    return false;
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

                if (input.Equals("P", StringComparison.OrdinalIgnoreCase))
                {
                    attendance[i] = "Present";
                }
                else if(input.Equals("A", StringComparison.OrdinalIgnoreCase))
                {
                    attendance[i] = "Absent";
                }
                else
                {
                    Console.WriteLine("Invalid Input. Marked as absent.");
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

            double percentage = (present / (double)attendance.Length) * 100;

            Console.WriteLine("\nAttendance Summary for: "+studentName);
            Console.WriteLine("Present: " + present);
            Console.WriteLine("Absent : " + absent);
            Console.WriteLine("Percentage: " + percentage.ToString("0.00") + "%\n");
        }
        static void DisplayLogs()
        {
            Console.WriteLine("Attendance Logs for: "+studentName);
            foreach (var log in attendanceLogs)
            {
                Console.WriteLine(log);
            }
        }
    }
}