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
        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM user WHERE name = $name";
        command.Parameters.AddWithValue("$name", name);
        command.ExecuteNonQuery();
        Console.WriteLine($"User with name '{name}' deleted.");
    }

    private static void DisplayAllUsers(SqliteConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = "SELECT id, name, age FROM user";

        using (var reader = command.ExecuteReader())
        {
            Console.WriteLine("Current users in the database:");
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                var age = reader.GetInt32(2);
                Console.WriteLine($"ID: {id}, Name: {name}, Age: {age}");
            }
        }
    }

    private static void InsertUsers(SqliteConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText =
            @"
                INSERT INTO user (name, age)
                VALUES ('Otto', 30),
                    ('Tim', 25),
                    ('Steve', 28),
                    ('Robert', 35);
            ";

        command.ExecuteNonQuery();
        Console.WriteLine("Users inserted.");
    }

    private static void CreateTable(SqliteConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS user (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    age INTEGER NOT NULL
                );
            ";
        
        command.ExecuteNonQuery();
        Console.WriteLine("Table created.");
    }
}