using Microsoft.EntityFrameworkCore;
using PersonalFinances.DataContext;
using PersonalFinances.Models;

namespace PersonalFinances.DataContextTest;
public class EFDataContextMock
{
    public static EFDataContext GetMockedDbContext()
    {
        var options = new DbContextOptionsBuilder<EFDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new EFDataContext(options, useInMemoryDatabase: true);

        // Seed database with initial data
        SeedDatabase(dbContext);

        return dbContext;
    }

    private static void SeedDatabase(EFDataContext dbContext)
    {
        // Clear existing data
        dbContext.Categories.RemoveRange(dbContext.Categories);
        dbContext.Users.RemoveRange(dbContext.Users);
        dbContext.Transactions.RemoveRange(dbContext.Transactions);
        dbContext.SaveChanges();

        // Add new data
        var users = new List<UserModel>
        {
            new UserModel
            {
                UserID = 1,
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "hashedPassword",
                Photo = null,
            },
            new UserModel
            {
                UserID = 2,
                Name = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                Password = "hashedPassword",
                Photo = null,
            }
        };

        var categories = new List<CategoryModel>
        {
            new CategoryModel
            {
                CategoryID = 1,
                Name = "Food",
                Description = "Food expenses",
                Type = "Expense"
            },
            new CategoryModel
            {
                CategoryID = 2,
                Name = "Salary",
                Description = "Monthly salary",
                Type = "Income"
            }
        };

        var transactions = new List<TransactionModel>
        {
            new TransactionModel
            {
                TransactionID = 1,
                Description = "Groceries",
                Value = 100,
                Type = "Expense",
               //  Date = DateTime.Now.Date,
                UserID = 1,
                CategoryID = 1
            },
            new TransactionModel
            {
                TransactionID = 2,
                Description = "Monthly salary",
                Value = 2000,
                Type = "Income",
               //  Date = "2024-06-10",
                UserID = 2,
                CategoryID = 2
            }
        };

        dbContext.Users.AddRange(users);
        dbContext.Categories.AddRange(categories);
        dbContext.Transactions.AddRange(transactions);

        dbContext.SaveChanges();
    }
}