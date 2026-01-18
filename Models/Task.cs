using System.Collections.Generic;

public class WorkTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int AssignedToId { get; set; }
}

public class TaskData
{
    public List<WorkTask> Tasks { get; set; } = new List<WorkTask>();
}
