using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkProject.Data;

namespace WorkProject.Controllers
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
            var tasks = await _context.Tasks.ToListAsync();
            var employees = await _context.Employees.ToListAsync();
            
            ViewBag.Tasks = tasks;
            ViewBag.Employees = employees;
            
            return View();
        }

        public async Task<IActionResult> EmployeeTasks(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            var tasks = await _context.Tasks.Where(t => t.AssignedToId == id).ToListAsync();
            
            ViewBag.Employee = employee;
            ViewBag.Tasks = tasks;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTaskStatus(int id, string status)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                task.Status = status;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(string title, int assignedToId)
        {
            var task = new WorkTask
            {
                Title = title,
                Status = "Not Started",
                AssignedToId = assignedToId
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
