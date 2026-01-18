using System.Collections.Generic;

namespace TaskManager
{
    public class Employee
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    public class EmployeeData
    {
        public required List<Employee> Employees { get; set; }
    }
}
