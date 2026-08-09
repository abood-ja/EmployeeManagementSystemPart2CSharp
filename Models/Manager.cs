using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystemProject2.Models
{
    public class Manager:Employee
    {
        public List<Employee> TeamMembers { get; set; } = new List<Employee>();

        public override string ToString()
        {
            return $"Manager[{Id}]: {Name}";
        }
    }
}
