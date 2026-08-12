using EmployeeManagementSystemProject2.Common;
using EmployeeManagementSystemProject2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeManagementSystemProject2.Delegates.EmployeeFilter;

namespace EmployeeManagementSystemProject2.Services
{
    public class Company
    {
        
        public event EventHandler<EmployeeEventArgs>? OnEmployeePromoted;
        public event EventHandler<EmployeeEventArgs>? OnEmployeeProcessed;
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
                ActionHistory.Push($"a new employee was added to onboarding: Employee[{emp.Id}], EmployeeName: {emp.Name}");
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
                ActionHistory.Push($"Add new department: {dep.Name} department");
                return new Result<Department> { Success = true, Message = $"Add Department Succeeded: departmentId[{dep.Id}]", Data = dep };
            }

        }

        public Result<Employee> ProcessNextEmployeeInOnBoarding()
        {
            if (OnBoarding.Count == 0)
                return new Result<Employee> { Success = false, Message = "Process Next Employee Failed:OnBoarding Queue is Empty", Data = null };
            var emp = OnBoarding.Dequeue();
            ActiveEmployees.Add(emp);
            foreach (string skill in emp.Skills)
            {
                Skills.Add(skill);
            }
            ActionHistory.Push($"a new employee is now Active: Employee[{emp.Id}], EmployeeName: {emp.Name}");
            if(OnEmployeeProcessed != null)
            {
                OnEmployeeProcessed?.Invoke(this, new EmployeeEventArgs(emp));
            }
            return new Result<Employee> { Success = true, Message = $"Process Next Employee Succedded: EmployeeId[{emp.Id}]", Data = emp };
        }

        public Result<string> AddSkillToEmployee(int employeeId, string skill)
        {
            if (string.IsNullOrWhiteSpace(skill))
                return new Result<string> { Success = false, Message = "Adding a new skill Failed: skill name is required", Data = null };


            Employee? emp = FindEmployeeById(employeeId);

            if (emp is null)
                return new Result<string> { Success = false, Message = "Adding a new skill Failed: employee does not exist", Data = skill };



            string normalizedSkill = skill.Trim();

            if (!emp.Skills.Contains(normalizedSkill))
                emp.Skills.Add(normalizedSkill);

            Skills.Add(normalizedSkill);
            ActionHistory.Push($"Added skill {normalizedSkill} to {emp.Name}");
            return new Result<string> { Success = true, Message = $"Adding a new skill succeeded:EmployeeId[{emp.Id}], skill:{skill}" };
        }
        public List<Employee> FilterEmployee(EmployeeFilterByCondition Filter)
        {

            List<Employee> employees = new List<Employee>();
            foreach (var emp in ActiveEmployees)
            {
                if (Filter(emp))
                    employees.Add(emp);
            }
            return employees;

        }

        public decimal CalculateAverageSalary()
        {
            if (ActiveEmployees.Count == 0) return 0;
            decimal totalSalary = 0;
            foreach (var emp in ActiveEmployees)
            {
                totalSalary += emp.Salary;
            }
            return totalSalary / ActiveEmployees.Count;
        }
        public void DisplayDepartmentsReport()
        {
            Console.WriteLine("===== Department Report =====");
            Console.WriteLine();

            foreach (KeyValuePair<int, Department> pair in Departments)
            {
                int employeeCount = 0;

                foreach (Employee employee in ActiveEmployees)
                {
                    if (employee.DepartmentId == pair.Key)
                        employeeCount++;
                }

                Console.WriteLine($"{pair.Value.Name,-10}: {employeeCount} employees");
            }

            Console.WriteLine();
            Console.WriteLine("=============================");
        }

        public void DisplayActionHistory()
        {
            Console.WriteLine("Action History:");

            foreach (string action in ActionHistory)
                Console.WriteLine(action);
        }

        public void DisplayCompanySkills()
        {
            Console.WriteLine("Company Skills:");

            foreach (string skill in Skills)
                Console.WriteLine(skill);
        }

        public List<Employee> GetAllEmployeesOfDepartmentById(int DepartmentId)
        {
            List<Employee> employees = new();
            if (!Departments.TryGetValue(DepartmentId, out Department? department))
                throw new InvalidOperationException($"Department Id {DepartmentId} does not exist.");
            foreach (var emp in ActiveEmployees)
            {
                if (emp.DepartmentId == DepartmentId)
                    employees.Add(emp);
            }
            return employees;
        }

        public Result<Employee> PromoteEmployee(int Id)
        {
            var emp = FindEmployeeById(Id);
            if (emp is null)
                return new Result<Employee>() { Message = "Promoting Failed!:the employee is null", Data = null, Success = false };
            if(emp is Manager)
                return new Result<Employee>() { Message = "Promoting Failed!:the employee is already manager", Data = emp, Success = false };
            else
            {
                Manager manager = new Manager
                {
                    Id = emp.Id,
                    Name = emp.Name,
                    HireDate = emp.HireDate,
                    DepartmentId = emp.DepartmentId,
                    Salary = emp.Salary
                };

                int index = ActiveEmployees.IndexOf(emp);
                ActiveEmployees[index] = manager;
                if (OnEmployeePromoted != null)
                {
                    OnEmployeePromoted?.Invoke(this,new EmployeeEventArgs(manager));
                }
                return new Result<Employee>() { Message = "Promoting succeded!!", Data = manager, Success = true };

            }

        }
    }
}
