using EmployeeManagementSystemProject2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystemProject2.Delegates
{
    public class EmployeeFilter
    {
        public delegate bool EmployeeFilterByCondition(Employee employee);
    }
}
