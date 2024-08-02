using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

class Program
{
  private static void Main(string[] args)
  {
    using (var context = new AppDbContext())
    {
      context.Database.Migrate();
      CreateTable(context);
      InsertUsers(context);
      DisplayAllUsers(context);
      DeleteUserByName(context, "Otto");
      DisplayAllUsers(context);
    }

    // File.Delete(databaseFile);
    // Console.WriteLine("Database file deleted!");
  }

  private static void DeleteUserByName(AppDbContext context, string name)
  {
    var user = context.Users.SingleOrDefault(u => u.Name == name);
    if (user != null)
    {
      context.Users.Remove(user);
      context.SaveChanges();
      Console.WriteLine($"User with name ‘{name}’ deleted.");
    }
  }

  private static void DisplayAllUsers(AppDbContext context)
  {
    var users = context.Users.ToList();
    Console.WriteLine("Current users in the database:");

    foreach (var user in users)
    {
      Console.WriteLine($"ID: {user.Id}, Name: {user.Name}, Age: {user.Age}, Email: {user.Email}");
    }
  }

  private static void InsertUsers(AppDbContext context)
  {
    var users = new[]
    {
      new User { Name = "Otto", Age = 30, Email = "otto@example.com" },
      new User { Name = "Tim", Age = 25, Email = "tim@example.com" },
      new User { Name = "Steve", Age = 28, Email = "steve@example.com" },
      new User { Name = "Robert", Age = 35, Email = "robert@example.com" }
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
  public string DbPath { get; }

  public DbSet<User> Users { get; set; }

  public AppDbContext()
  {
    DbPath = "crm.db";
  }

  protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite($"Data Source={DbPath}");
}

public class User
{
  public int Id { get; set; }
  public string Name { get; set; }
  public int Age { get; set; }
  public string Email { get; set; } // New property
}