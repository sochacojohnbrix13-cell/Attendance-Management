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
        private string connectionString = @"Server=localhost\SQLEXPRESS;
            Database=AttendanceDB;
            Trusted_Connection=True;
            TrustServerCertificate=True;";

        private readonly string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "student.json");

        private List<Attendance> attendances = new List<Attendance>();

        public AttendanceDataService()
        {
            LoadFromFile();
        }

        public void LoadFromFile()
        {
            if (File.Exists(filepath))
            {
                try
                {
                    string json = File.ReadAllText(filepath);attendances = JsonSerializer.Deserialize<List<Attendance>>(json) ?? new List<Attendance>();
                }
                catch
                {
                    attendances = new List<Attendance>();
                }
            }
        }

        public void SaveToFile()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(attendances, options);File.WriteAllText(filepath, json);
        }

        public void Add(Attendance attendance)
        {
            if (GetById(attendance.AttendanceId) == null)
            {
                attendances.Add(attendance);
                SaveToFile();

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

        public List<Attendance> GetAttendances()
        {
            return attendances;
        }

        public Attendance? GetById(Guid id)
        {
            return attendances.FirstOrDefault(a => a.AttendanceId == id);
        }

        public bool Update(Attendance updated)
        {
            var existing = GetById(updated.AttendanceId);

            if (existing != null)
            {
                existing.StudentName = updated.StudentName;
                existing.Days = updated.Days;
                existing.Records = updated.Records;

                SaveToFile();

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

        public bool Delete(Guid id)
        {
            var attendance = GetById(id);

            if (attendance != null)
            {
                attendances.Remove(attendance);
                SaveToFile();

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