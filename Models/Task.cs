using System.Collections.Generic;

namespace TaskManager
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Navigation property
        public List<TaskAssignment> Assignments { get; set; } = new List<TaskAssignment>();
    }

    public class TaskData
    {
        public List<TaskItemDto> Tasks { get; set; } = new List<TaskItemDto>();
    }

    // DTO for JSON deserialization
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<int> AssignedToId { get; set; } = new List<int>();
    }
}
