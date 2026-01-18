namespace TaskManager
{
    public class TaskAssignment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int EmployeeId { get; set; }
        public string Status { get; set; } = "Not Started";
        
        // Navigation properties
        public TaskItem Task { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
    }
}
