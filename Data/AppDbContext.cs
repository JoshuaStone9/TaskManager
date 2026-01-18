using Microsoft.EntityFrameworkCore;

namespace WorkProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<WorkTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial data from JSON concept
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1001, Name = "John Smith" },
                new Employee { Id = 1002, Name = "Sarah Johnson" },
                new Employee { Id = 1003, Name = "Michael Williams" },
                new Employee { Id = 1004, Name = "Emily Brown" },
                new Employee { Id = 1005, Name = "David Martinez" }
            );

            modelBuilder.Entity<WorkTask>().HasData(
                new WorkTask { Id = 1, Title = "Grind and Dose", Status = "Not Started", AssignedToId = 1001 },
                new WorkTask { Id = 2, Title = "Milk Delivery", Status = "Not Started", AssignedToId = 1002 },
                new WorkTask { Id = 3, Title = "Food Delivery", Status = "Not Started", AssignedToId = 1003 },
                new WorkTask { Id = 4, Title = "General Coffee Knowledge", Status = "Not Started", AssignedToId = 1004 },
                new WorkTask { Id = 5, Title = "Espresso Check", Status = "Not Started", AssignedToId = 1005 },
                new WorkTask { Id = 6, Title = "Moments of Heart", Status = "Not Started", AssignedToId = 1001 }
            );
        }
    }
}
