using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WorkProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetTasks()
        {
            try
            {
                string taskJson = System.IO.File.ReadAllText("tasks.json");
                string employeeJson = System.IO.File.ReadAllText("employees.json");
                
                var taskData = JsonSerializer.Deserialize<TaskData>(taskJson);
                var employeeData = JsonSerializer.Deserialize<EmployeeData>(employeeJson);
                
                var tasksWithEmployees = taskData?.Tasks.Select(task => new
                {
                    task.Id,
                    task.Title,
                    task.Status,
                    AssignedToId = task.AssignedToId,
                    AssignedToName = employeeData?.Employees.Find(e => e.Id == task.AssignedToId)?.Name ?? "Unknown"
                });
                
                return Ok(tasksWithEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("employee/{employeeId}")]
        public IActionResult GetTasksByEmployee(int employeeId)
        {
            try
            {
                string taskJson = System.IO.File.ReadAllText("tasks.json");
                string employeeJson = System.IO.File.ReadAllText("employees.json");
                
                var taskData = JsonSerializer.Deserialize<TaskData>(taskJson);
                var employeeData = JsonSerializer.Deserialize<EmployeeData>(employeeJson);
                
                var employee = employeeData?.Employees.Find(e => e.Id == employeeId);
                if (employee == null)
                    return NotFound($"Employee with ID {employeeId} not found.");
                
                var employeeTasks = taskData?.Tasks
                    .Where(t => t.AssignedToId == employeeId)
                    .Select(task => new
                    {
                        task.Id,
                        task.Title,
                        task.Status
                    });
                
                return Ok(new { EmployeeName = employee.Name, Tasks = employeeTasks });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
