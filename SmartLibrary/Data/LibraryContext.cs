using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Models;

namespace SmartLibrary.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        // Tables
        public DbSet<User> Users => Set<User>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Faculty> Faculty => Set<Faculty>();
        public DbSet<Staff> Staff => Set<Staff>();

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Loan> Loans => Set<Loan>();
        public DbSet<Fine> Fines => Set<Fine>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---- USER INHERITANCE (TPH: Table-Per-Hierarchy) ----
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Student>("Student")
                .HasValue<Faculty>("Faculty")
                .HasValue<Staff>("Staff");

            // ---- USER RELATIONSHIPS ----
            modelBuilder.Entity<User>()
                .HasMany(u => u.Loans)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---- BOOK RELATIONSHIPS ----
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Loans)
                .WithOne(l => l.Book)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---- LOAN & FINE (1:1) ----
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Fine)
                .WithOne(f => f.Loan)
                .HasForeignKey<Fine>(f => f.LoanId);
        }
    }
}
