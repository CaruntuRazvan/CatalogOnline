using CatalogOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogOnline.ContextModels;

public class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany()
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.NoAction); // Dezactivă operațiunea de ștergere în cascada pentru StudentId în Enrollment
        modelBuilder.Entity<Grade>()
           .HasOne(g => g.Student)
           .WithMany()
           .HasForeignKey(g => g.StudentId)
           .OnDelete(DeleteBehavior.Restrict);
    }
    public void ClearDatabase()
    {
        Grades.RemoveRange(Grades);
        Enrollments.RemoveRange(Enrollments);
        Courses.RemoveRange(Courses);
        Users.RemoveRange(Users);
        SaveChanges();

        ResetIdentity("Grades");
        ResetIdentity("Enrollments");
        ResetIdentity("Courses");
        ResetIdentity("Users");
    }

    private void ResetIdentity(string tableName)
    {
        var sql = $"DBCC CHECKIDENT ('{tableName}', RESEED, 0)";
        Database.ExecuteSqlRaw(sql);
    }


}
