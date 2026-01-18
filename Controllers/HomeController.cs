using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var assignments = await _context.TaskAssignments
                .Include(ta => ta.Task)
                .Include(ta => ta.Employee)
                .ToListAsync();
            var employees = await _context.Employees.ToListAsync();
            
            ViewBag.Assignments = assignments;
            ViewBag.Employees = employees;
            
            return View();
        }

        public async Task<IActionResult> EmployeeTasks(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            var assignments = await _context.TaskAssignments
                .Include(ta => ta.Task)
                .Where(ta => ta.EmployeeId == id)
                .ToListAsync();
            
            ViewBag.Employee = employee;
            ViewBag.Assignments = assignments;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTaskStatus(int id, string status, int? employeeId = null)
        {
            var assignment = await _context.TaskAssignments.FindAsync(id);
            if (assignment != null)
            {
                assignment.Status = status;
                await _context.SaveChangesAsync();
            }
            
            if (employeeId.HasValue)
                return RedirectToAction("EmployeeTasks", new { id = employeeId.Value });
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(string title, List<int> assignedToId)
        {
            var task = new TaskItem
            {
                Title = title,
                Description = ""
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            
            // Create assignments for each employee
            if (assignedToId != null && assignedToId.Count > 0)
            {
                foreach (var employeeId in assignedToId)
                {
                    var assignment = new TaskAssignment
                    {
                        TaskId = task.Id,
                        EmployeeId = employeeId,
                        Status = "Not Started"
                    };
                    _context.TaskAssignments.Add(assignment);
                }
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Index");
        }
    }
}
