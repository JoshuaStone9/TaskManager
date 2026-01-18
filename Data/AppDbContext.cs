using Microsoft.EntityFrameworkCore;

namespace TaskManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure TaskAssignment relationships
            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Task)
                .WithMany(t => t.Assignments)
                .HasForeignKey(ta => ta.TaskId);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Employee)
                .WithMany()
                .HasForeignKey(ta => ta.EmployeeId);

            // Seed initial data
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1001, Name = "John Smith" },
                new Employee { Id = 1002, Name = "Sarah Johnson" },
                new Employee { Id = 1003, Name = "Michael Williams" },
                new Employee { Id = 1004, Name = "Emily Brown" },
                new Employee { Id = 1005, Name = "David Martinez" }
            );

            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem { Id = 1, Title = "Grind and Dose", Description = "Prepare and grind coffee beans" },
                new TaskItem { Id = 2, Title = "Milk Delivery", Description = "Receive and stock milk delivery" },
                new TaskItem { Id = 3, Title = "Food Delivery", Description = "Check in food deliveries" },
                new TaskItem { Id = 4, Title = "General Coffee Knowledge", Description = "Study coffee preparation techniques" },
                new TaskItem { Id = 5, Title = "Espresso Check", Description = "Quality check espresso machines" },
                new TaskItem { Id = 6, Title = "Moments of Heart", Description = "Customer service excellence" }
            );

            // Seed task assignments
            modelBuilder.Entity<TaskAssignment>().HasData(
                // Task 1 assigned to 3 employees
                new TaskAssignment { Id = 1, TaskId = 1, EmployeeId = 1001, Status = "Not Started" },
                new TaskAssignment { Id = 2, TaskId = 1, EmployeeId = 1002, Status = "Not Started" },
                new TaskAssignment { Id = 3, TaskId = 1, EmployeeId = 1003, Status = "Not Started" },
                // Task 2
                new TaskAssignment { Id = 4, TaskId = 2, EmployeeId = 1002, Status = "Not Started" },
                // Task 3
                new TaskAssignment { Id = 5, TaskId = 3, EmployeeId = 1003, Status = "Not Started" },
                // Task 4
                new TaskAssignment { Id = 6, TaskId = 4, EmployeeId = 1004, Status = "Not Started" },
                // Task 5
                new TaskAssignment { Id = 7, TaskId = 5, EmployeeId = 1005, Status = "Not Started" },
                // Task 6
                new TaskAssignment { Id = 8, TaskId = 6, EmployeeId = 1001, Status = "Not Started" }
            );
        }
    }
}
