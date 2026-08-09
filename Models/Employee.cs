using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystemProject2.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int DepartmentId { get; set; }
        public DateOnly HireDate { get; set; }
        public decimal Salary { get; set; }
        public List<string> Skills { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"Employee[{Id}]: {Name}";
        }

    }
}
