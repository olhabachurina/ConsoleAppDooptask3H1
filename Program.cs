// See https://aka.ms/new-console-template for more information
using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

class Program
{
    static string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=doptask3;Trusted_Connection=True;TrustServerCertificate=True";

    static void Main()
    {
        CreateDatabase();
        CreateDatabaseTable();
        InsertMultipleUsers();
        UpdateYoungUsers();
        DisplayUsers();
    }
    static void CreateDatabase()
    {
        using (SqlConnection connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=True;TrustServerCertificate=True"))
        {
            connection.Open();
            string checkDatabaseQuery = "IF DB_ID('doptask3') IS NULL CREATE DATABASE doptask3";
            using (SqlCommand command = new SqlCommand(checkDatabaseQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
    static void CreateDatabaseTable()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string createTableQuery = "CREATE TABLE Users (Id INT PRIMARY KEY IDENTITY(1,1), Name NVARCHAR(100), Age INT, Country NVARCHAR(100), IsAdult BIT)";
            using (SqlCommand command = new SqlCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
    static void InsertMultipleUsers()
    {
        List<User> users = new List<User>
        {
            new User { Name = "Petrenko", Age = 25, Country = "Ukraine", IsAdult = true },
            new User { Name = "Ivanov", Age = 30, Country = "Russia", IsAdult = true },
            new User { Name = "Smith", Age = 17, Country = "USA", IsAdult = false },
            new User { Name = "Lee", Age = 22, Country = "South Korea", IsAdult = true },
            new User { Name = "Chen", Age = 19, Country = "China", IsAdult = true }
        };

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string insertQuery = "INSERT INTO Users (Name, Age, Country, IsAdult) VALUES (@Name, @Age, @Country, @IsAdult)";

            foreach (User user in users)
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Age", user.Age);
                    command.Parameters.AddWithValue("@Country", user.Country);
                    command.Parameters.AddWithValue("@IsAdult", user.IsAdult ? 1 : 0);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
    static void UpdateYoungUsers()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string updateQuery = "UPDATE Users SET IsAdult = 0 WHERE Age < 18";

            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
    static void DisplayUsers()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT * FROM Users";

            using (SqlCommand command = new SqlCommand(selectQuery, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"User {reader["Id"]}: {reader["Name"]}, Age: {reader["Age"]}, Is Adult: {reader["IsAdult"]}");
                    }
                }
            }
        }
    }
}
class User
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Country { get; set; }
    public bool IsAdult { get; set; }
}
