using Microsoft.EntityFrameworkCore;
using PersonalFinances.Models;

namespace PersonalFinances.DataContext;

public class EFDataContext : DbContext
{
	private readonly bool _useInMemoryDatabase;

	public EFDataContext(DbContextOptions<EFDataContext> options, bool useInMemoryDatabase = false) : base(options)
	{
		_useInMemoryDatabase = useInMemoryDatabase;
	}

	public DbSet<UserModel> Users { get; set; }
	public DbSet<CategoryModel> Categories { get; set; }
	public DbSet<TransactionModel> Transactions { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		try
		{
			if (!optionsBuilder.IsConfigured)
			{
				if (!_useInMemoryDatabase)
				{
					var connectionString = "Server=localhost;Port=3306;Database=finances;User=root;Password=toor";
					optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
						 .LogTo(Console.WriteLine, LogLevel.Information)
						 .EnableSensitiveDataLogging()
						 .EnableDetailedErrors();
				}
				else
				{
					optionsBuilder.UseInMemoryDatabase("TestDatabase");
				}
			}
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
			modelBuilder.Entity<UserModel>(entity =>
			{
				entity.ToTable("tb_user");
				entity.HasKey(e => e.UserID);
				entity.Property(e => e.UserID).HasColumnName("id");
				entity.Property(e => e.Name).HasColumnName("name");
				entity.Property(e => e.LastName).HasColumnName("lastName");
				entity.Property(e => e.Email).HasColumnName("email");
				entity.Property(e => e.Password).HasColumnName("password");
				entity.Property(e => e.Photo).HasColumnName("photo");
				entity.HasMany(e => e.Transactions)
					 .WithOne(t => t.User)
					 .HasForeignKey(t => t.UserID)
					 .OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<CategoryModel>(entity =>
			{
				entity.ToTable("tb_category");
				entity.HasKey(e => e.CategoryID);
				entity.Property(e => e.CategoryID).HasColumnName("id");
				entity.Property(e => e.Name).HasColumnName("name");
				entity.Property(e => e.Description).HasColumnName("description");
				entity.Property(e => e.Type).HasColumnName("type");
			});

			modelBuilder.Entity<TransactionModel>(entity =>
			{
				entity.ToTable("tb_transaction");
				entity.HasKey(e => e.TransactionID);
				entity.Property(e => e.TransactionID).HasColumnName("id");
				entity.Property(e => e.Description).HasColumnName("description");
				entity.Property(e => e.Value).HasColumnName("value");
				entity.Property(e => e.Type).HasColumnName("type");
				entity.Property(e => e.Date).HasColumnName("date");
				entity.Property(e => e.CreatedAt).HasColumnName("created_at");
				entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

				entity.Property(e => e.UserID).HasColumnName("fk_user");
				entity.Property(e => e.CategoryID).HasColumnName("fk_category");

				entity.HasOne(e => e.User)
					 .WithMany(u => u.Transactions)
					 .HasForeignKey(e => e.UserID)
					 .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(e => e.Category)
					 .WithMany(c => c.Transactions)
					 .HasForeignKey(e => e.CategoryID)
					 .OnDelete(DeleteBehavior.Restrict);
			});

			base.OnModelCreating(modelBuilder);
		}
		catch (Exception ex)
		{
		}
	}
}