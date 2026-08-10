using EmployeeManagementSystemProject2.Common;
using EmployeeManagementSystemProject2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystemProject2.Services
{
    public class Company
    {
        List<Employee> ActiveEmployees = new List<Employee>();
        Dictionary<int, Department> Departments = new Dictionary<int, Department>();
        Queue<Employee> OnBoarding = new Queue<Employee>();
        Stack<string> ActionHistory = new Stack<string>();
        HashSet<string> Skills = new HashSet<string>();


        public Employee? FindEmployeeByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            foreach (var emp in ActiveEmployees)
            {
                if (emp.Name == name)
                    return emp;
            }

            return null;
        }

        public Employee? FindEmployeeById(int Id)
        {
            foreach (var emp in ActiveEmployees)
            {
                if (emp.Id == Id) return emp;
            }
            return null;
        }

        public Result<Employee> AddEmployee(Employee emp)
        {
            if (emp is null)
                return new Result<Employee> { Success = false, Message = ":Add Employee Failed :employee is null", Data = null };
            if (string.IsNullOrWhiteSpace(emp.Name))
                return new Result<Employee> { Success = false, Message = "Add Employee Failed :employee name is required", Data = null };
            if (FindEmployeeById(emp.Id) is not null)
                return new Result<Employee> { Success = false, Message = "Add Employee Failed :employee already exists", Data = null };
            else
            {
                OnBoarding.Enqueue(emp);
                return new Result<Employee> { Success = true, Message = $"Add Employee Succeeded: EmployeeId[{emp.Id}]", Data = emp };
            }

        }
        public Result<Department> AddDepartment(Department dep)
        {
            if (dep is null)
                return new Result<Department> { Success = false, Message = "Add Department Failed: department is null", Data = null };
            if (string.IsNullOrWhiteSpace(dep.Name))
                return new Result<Department> { Success = false, Message = "Add Department Failed: department Name is required", Data = null };
            if (Departments.ContainsKey(dep.Id))
                return new Result<Department> { Success = false, Message = "Add Department Failed: department already exists", Data = null };
            else
            {
                Departments.Add(dep.Id, dep);
                return new Result<Department> { Success = true, Message = $"Add Department Succeeded: departmentId[{dep.Id}]", Data = dep };
            }

        }
    }
}
