using Microsoft.EntityFrameworkCore;
using SmartSpend.API.Models;

namespace SmartSpend.API.Data
{
    public class SmartSpendDbContext : DbContext
    {
        public SmartSpendDbContext(DbContextOptions<SmartSpendDbContext> options) : base(options) { }

        // DbSets -- one per table!
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<RecurringTransaction> RecurringTransactions { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Users table mapping
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
            });

            // Categories table mapping
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryId).HasColumnName("category_id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.IsDefault).HasColumnName("is_default");
            });

            // Transactions table mapping
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transactions");
                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.CategoryId).HasColumnName("category_id");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
                entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.IsRecurring).HasColumnName("is_recurring");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                // Relationships
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Transactions)
                      .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Transactions)
                      .HasForeignKey(e => e.CategoryId);
            });

            // Budgets table mapping
            modelBuilder.Entity<Budget>(entity =>
            {
                entity.ToTable("budgets");
                entity.HasKey(e => e.BudgetId);
                entity.Property(e => e.BudgetId).HasColumnName("budget_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.CategoryId).HasColumnName("category_id");
                entity.Property(e => e.LimitAmount).HasColumnName("limit_amount");
                entity.Property(e => e.Month).HasColumnName("month");
                entity.Property(e => e.Year).HasColumnName("year");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                // Relationships
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Budgets)
                      .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Budgets)
                      .HasForeignKey(e => e.CategoryId);
            });

            // RecurringTransactions table mapping
            modelBuilder.Entity<RecurringTransaction>(entity =>
            {
                entity.ToTable("recurring_transactions");
                entity.HasKey(e => e.RecurringId);
                entity.Property(e => e.RecurringId).HasColumnName("recurring_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.CategoryId).HasColumnName("category_id");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
                entity.Property(e => e.Frequency).HasColumnName("frequency");
                entity.Property(e => e.StartDate).HasColumnName("start_date");
                entity.Property(e => e.EndDate).HasColumnName("end_date");
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.IsActive).HasColumnName("is_active");

                // Relationships
                entity.HasOne(e => e.User)
                      .WithMany(u => u.RecurringTransactions)
                      .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Category)
                      .WithMany()
                      .HasForeignKey(e => e.CategoryId);
            });

            // Notifications table mapping
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("notifications");
                entity.HasKey(e => e.NotificationId);
                entity.Property(e => e.NotificationId).HasColumnName("notification_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Message).HasColumnName("message");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.IsRead).HasColumnName("is_read");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                // Relationships
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(e => e.UserId);
            });
        }
    }
}