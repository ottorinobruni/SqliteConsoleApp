using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

class Program
{
    private static void Main(string[] args)
    {
        const string databaseFile = "crm.db";
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={databaseFile}")
            .Options;

        using (var context = new AppDbContext(options))
        {
            context.Database.EnsureCreated();
            CreateTable(context);
            InsertUsers(context);
            DisplayAllUsers(context);
            DeleteUserByName(context, "Otto");
            DisplayAllUsers(context);
        }

        File.Delete(databaseFile);
        System.Console.WriteLine("Database file deleted!");
    }

    private static void DeleteUserByName(AppDbContext context, string name)
    {
        var user = context.Users.SingleOrDefault(u => u.Name == name);
        if (user != null)
        {
            context.Users.Remove(user);
            context.SaveChanges();
            Console.WriteLine($"User with name '{name}' deleted.");
        }
    }

    private static void DisplayAllUsers(AppDbContext context)
    {
        var users = context.Users;
        Console.WriteLine("Current users in the database:");
        
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id}, Name: {user.Name}, Age: {user.Age}");
        }
    }

    private static void InsertUsers(AppDbContext context)
    {
        var users = new[]
        {
            new User { Name = "Otto", Age = 30 },
            new User { Name = "Tim", Age = 25 },
            new User { Name = "Steve", Age = 28 },
            new User { Name = "Robert", Age = 35 }
        };

        context.Users.AddRange(users);
        context.SaveChanges();
        Console.WriteLine("Users inserted.");
    }

    private static void CreateTable(AppDbContext context)
    {
        // Table creation is handled by EnsureCreated method
        Console.WriteLine("Table created.");
    }
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

     public DbSet<User> Users { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}