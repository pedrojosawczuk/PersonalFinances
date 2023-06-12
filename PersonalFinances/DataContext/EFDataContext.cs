using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using PersonalFinances.Models;
using DotNetEnv;

namespace PersonalFinances.DataContext;

public class EFDataContext : DbContext
{
   public DbSet<UserModel> Users { get; set; }
   public DbSet<CategoryModel> Categories { get; set; }
   public DbSet<TransactionModel> Transactions { get; set; }

   public EFDataContext()
   {
      Users = Set<UserModel>();
      Categories = Set<CategoryModel>();
      Transactions = Set<TransactionModel>();
   }
   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
      try
      {
         base.OnConfiguring(optionsBuilder);

         Env.Load();
         optionsBuilder.UseMySql(
             Environment.GetEnvironmentVariable("SQL_CONNECT"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.6.12-mariadb"));
      }
      catch (ArgumentException)
      {
         throw new ArgumentException("Error retrieving environment variables.");
      }
      catch (Exception ex)
      {
         throw new Exception(ex.Message);
      }
   }
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      try
      {
         modelBuilder.Entity<UserModel>().ToTable("tb_user");
         modelBuilder.Entity<CategoryModel>().ToTable("tb_category");
         modelBuilder.Entity<TransactionModel>().ToTable("tb_transaction");

         modelBuilder.Entity<UserModel>().HasKey(t => t.UserID);
         modelBuilder.Entity<CategoryModel>().HasKey(c => c.CategoryID);
         modelBuilder.Entity<TransactionModel>().HasKey(t => t.TransactionID);
      }
      catch (Exception ex)
      {
         throw new Exception(ex.Message);
      }
   }
}