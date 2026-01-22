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

        // Helper method to check admin access
        private bool IsAdminAuthenticated()
        {
            return HttpContext.Session.GetString("AdminAccess") == "true";
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

        public async Task<IActionResult> AdminDashboard()
        {
            // Check if user has entered correct PIN
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("EnterPinScreen");
            }

            var tasks = await _context.Tasks
                .Include(t => t.Assignments)
                .ThenInclude(a => a.Employee)
                .ToListAsync();
            var employees = await _context.Employees.ToListAsync();
            
            ViewBag.Tasks = tasks;
            ViewBag.Employees = employees;
            
            return View();
        }

        public IActionResult EnterPinScreen()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyPin(string pin)
        {
            const string ADMIN_PIN = "1234"; // Change this to your desired PIN
            
            if (pin == ADMIN_PIN)
            {
                HttpContext.Session.SetString("AdminAccess", "true");
                return RedirectToAction("AdminDashboard");
            }
            
            ViewBag.Error = "Incorrect PIN. Please try again.";
            return View("EnterPinScreen");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int id)
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("EnterPinScreen");
            }

            var task = await _context.Tasks
                .Include(t => t.Assignments)
                .FirstOrDefaultAsync(t => t.Id == id);
            
            if (task != null)
            {
                _context.TaskAssignments.RemoveRange(task.Assignments);
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("EnterPinScreen");
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                var assignments = await _context.TaskAssignments
                    .Where(ta => ta.EmployeeId == id)
                    .ToListAsync();
                
                _context.TaskAssignments.RemoveRange(assignments);
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(string name)
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("EnterPinScreen");
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                var employee = new Employee { Name = name };
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("AdminDashboard");
        }

        public async Task<IActionResult> EditEmployee(int id)
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("EnterPinScreen");
            }

            var employee = await _context.Employees.FindAsync(id);
            var allTasks = await _context.Tasks.ToListAsync();
            var employeeAssignments = await _context.TaskAssignments
                .Where(ta => ta.EmployeeId == id)
                .ToListAsync();
            
            ViewBag.Employee = employee;
            ViewBag.AllTasks = allTasks;
            ViewBag.EmployeeAssignments = employeeAssignments;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(int id, string name)
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("EnterPinScreen");
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee != null && !string.IsNullOrWhiteSpace(name))
            {
                employee.Name = name;
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> AssignTaskToEmployee(int employeeId, int taskId)
        {
            var existingAssignment = await _context.TaskAssignments
                .FirstOrDefaultAsync(ta => ta.EmployeeId == employeeId && ta.TaskId == taskId);
            
            if (existingAssignment == null)
            {
                var assignment = new TaskAssignment
                {
                    TaskId = taskId,
                    EmployeeId = employeeId,
                    Status = "Not Started"
                };
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("EnterPinScreen");
            }

                _context.TaskAssignments.Add(assignment);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("EditEmployee", new { id = employeeId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTaskFromEmployee(int assignmentId, int employeeId)
        {
            var assignment = await _context.TaskAssignments.FindAsync(assignmentId);
            if (assignment != null)
            {
                _context.TaskAssignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("EditEmployee", new { id = employeeId });
        }
    }
}
