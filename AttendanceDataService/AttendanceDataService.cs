using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using AttendanceManagementModels;

namespace AttendanceManagementDataService
{
    public class AttendanceDataService
    {
        // Connection string for SQL Server database
        private string connectionString = @"Server=localhost\SQLEXPRESS;
            Database=AttendanceDB;
            Trusted_Connection=True;
            TrustServerCertificate=True;";
        // File path for JSON storage
        private readonly string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "student.json");
        // In-memory list to hold attendance records
        private List<Attendance> attendances = new List<Attendance>();

        public AttendanceDataService()
        {
            // Load existing attendance records from the JSON file when the service is initialized
            LoadFromFile();
        }

        public void LoadFromFile()
        {
            if (File.Exists(filepath))
            {
                try
                {
                    // Read the JSON file and deserialize it into the attendances list
                    string json = File.ReadAllText(filepath); attendances = JsonSerializer.Deserialize<List<Attendance>>(json) ?? new List<Attendance>();
                }
                catch
                {
                    // If there's an error during deserialization, initialize an empty list
                    attendances = new List<Attendance>();
                }
            }
        }
        // Save the current state of the attendances list back to the JSON file
        public void SaveToFile()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // Format the JSON with indentation for better readability
            };
            string json = JsonSerializer.Serialize(attendances, options); File.WriteAllText(filepath, json);
        }
        // Add a new attendance record to the in-memory list and save it to the JSON file and SQL database
        public void Add(Attendance attendance)
        {
            if (GetById(attendance.AttendanceId) == null)
            {
                // If the attendance record does not already exist, add it to the list and save it
                attendances.Add(attendance);
                SaveToFile();
                // Insert the new attendance record into the SQL database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO Attendances (AttendanceId, StudentName, Days, Records)
                    VALUES (@AttendanceId, @StudentName, @Days, @Records)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AttendanceId", attendance.AttendanceId);
                        command.Parameters.AddWithValue("@StudentName", attendance.StudentName);
                        command.Parameters.AddWithValue("@Days", attendance.Days);
                        command.Parameters.AddWithValue("@Records", string.Join(",", attendance.Records));
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        // Retrieve all attendance records from the in-memory list
        public List<Attendance> GetAttendances()
        {
            return attendances;
        }
        // Retrieve a specific attendance record by its unique identifier
        public Attendance? GetById(Guid id)
        {
            return attendances.FirstOrDefault(a => a.AttendanceId == id);
        }
        // Update an existing attendance record in the in-memory list and save the changes to the JSON file and SQL database
        public bool Update(Attendance updated)
        {
            var existing = GetById(updated.AttendanceId);
            // If the attendance record exists, update its properties and save the changes
            if (existing != null)
            {
                existing.StudentName = updated.StudentName;
                existing.Days = updated.Days;
                existing.Records = updated.Records;
                // Save the updated attendance record to the JSON file
                SaveToFile();
                // Update the existing attendance record in the SQL database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE Attendances
                     SET StudentName = @StudentName, Days = @Days, Records = @Records
                      WHERE AttendanceId = @AttendanceId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AttendanceId", updated.AttendanceId);
                        command.Parameters.AddWithValue("@StudentName", updated.StudentName);
                        command.Parameters.AddWithValue("@Days", updated.Days);
                        command.Parameters.AddWithValue("@Records", string.Join(",", updated.Records));
                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }

            return false;
        }
        // Delete an attendance record from the in-memory list and remove it from the JSON file and SQL database
        public bool Delete(Guid id)
        {
            var attendance = GetById(id);
            // If the attendance record exists, remove it from the list and save the changes
            if (attendance != null)
            {
                attendances.Remove(attendance);
                SaveToFile();
                // Delete the attendance record from the SQL database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Attendances WHERE AttendanceId = @AttendanceId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AttendanceId", id);
                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }

            return false;
        }
    }
}