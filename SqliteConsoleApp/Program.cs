using Dapper;
using Microsoft.Data.Sqlite;

class Program
{
    private static void Main(string[] args)
    {
        const string databaseFile = "crm.db";
        var connectionString = $"Data Source={databaseFile}";

        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            CreateTable(connection);
            InsertUsers(connection);
            DisplayAllUsers(connection);
            DeleteUserByName(connection, "Otto");
            DisplayAllUsers(connection);
        }

        File.Delete(databaseFile);
        System.Console.WriteLine("Database file deleted!");
    }

    private static void DeleteUserByName(SqliteConnection connection, string name)
    {
        var sql = "DELETE FROM user WHERE name = @Name";
        connection.Execute(sql, new { Name = name });
        Console.WriteLine($"User with name '{name}' deleted.");
    }

    private static void DisplayAllUsers(SqliteConnection connection)
    {
        var sql = "SELECT id, name, age FROM user";
        var users = connection.Query<User>(sql);

        Console.WriteLine("Current users in the database:");
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id}, Name: {user.Name}, Age: {user.Age}");
        }
    }

    private static void InsertUsers(SqliteConnection connection)
    {
        var sql = @"
            INSERT INTO user (name, age)
            VALUES (@Name, @Age)";

        var users = new[]
        {
            new User { Name = "Otto", Age = 30 },
            new User { Name = "Tim", Age = 25 },
            new User { Name = "Steve", Age = 28 },
            new User { Name = "Robert", Age = 35 }
        };

        connection.Execute(sql, users);
        Console.WriteLine("Users inserted.");
    }

    private static void CreateTable(SqliteConnection connection)
    {
        var sql =
            @"
                CREATE TABLE IF NOT EXISTS user (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    age INTEGER NOT NULL
                );
            ";
        
        connection.Execute(sql);
        Console.WriteLine("Table created.");
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}