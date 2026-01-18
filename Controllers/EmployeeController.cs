using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskManager;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetEmployees()
        {
            try
            {
                string jsonString = System.IO.File.ReadAllText("employees.json");
                var data = JsonSerializer.Deserialize<EmployeeData>(jsonString);
                return Ok(data?.Employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            try
            {
                string jsonString = System.IO.File.ReadAllText("employees.json");
                var data = JsonSerializer.Deserialize<EmployeeData>(jsonString);
                var employee = data?.Employees.Find(e => e.Id == id);
                
                if (employee == null)
                    return NotFound($"Employee with ID {id} not found.");
                
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
