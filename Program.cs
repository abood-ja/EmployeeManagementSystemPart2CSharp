using EmployeeManagementSystemProject2.Common;
using EmployeeManagementSystemProject2.Models;
using EmployeeManagementSystemProject2.Services;

namespace EmployeeManagementSystemProject2
{
    public class Program
    {
        static void SeedData(Company company)
        {
            Department it = new Department
            {
                Id = 1,
                Name = "IT"
            };

            Department hr = new Department
            {
                Id = 2,
                Name = "HR"
            };

            Department finance = new Department
            {
                Id = 3,
                Name = "Finance"
            };


            company.AddDepartment(it);
            company.AddDepartment(hr);
            company.AddDepartment(finance);


            Employee employee1 = new Employee
            {
                Id = 1,
                Name = "Ahmad",
                HireDate = new DateOnly(2024, 1, 10),
                DepartmentId = 1,
                Salary = 5000
            };

            employee1.Skills.Add("C#");
            employee1.Skills.Add("SQL");


            Employee employee2 = new Employee
            {
                Id = 2,
                Name = "Omar",
                HireDate = new DateOnly(2023, 5, 20),
                DepartmentId = 1,
                Salary = 6500
            };

            employee2.Skills.Add("C#");
            employee2.Skills.Add("ASP.NET");


            Employee employee3 = new Employee
            {
                Id = 3,
                Name = "Sara",
                HireDate = new DateOnly(2022, 8, 15),
                DepartmentId = 2,
                Salary = 4000
            };

            employee3.Skills.Add("Recruitment");
            employee3.Skills.Add("Communication");


            Employee employee4 = new Employee
            {
                Id = 4,
                Name = "Lina",
                HireDate = new DateOnly(2021, 3, 1),
                DepartmentId = 3,
                Salary = 7000
            };

            employee4.Skills.Add("Accounting");
            employee4.Skills.Add("Excel");


            company.AddEmployee(employee1);
            company.AddEmployee(employee2);
            company.AddEmployee(employee3);
            company.AddEmployee(employee4);


            // Process the employees so they become active.
            company.ProcessNextEmployeeInOnBoarding();
            company.ProcessNextEmployeeInOnBoarding();
            company.ProcessNextEmployeeInOnBoarding();
            company.ProcessNextEmployeeInOnBoarding();
        }
        static void HandleEmployeePromoted(object? sender, EmployeeEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("***** EVENT *****");
            Console.WriteLine(
                $"Employee {e.employee.Name} was promoted to Manager."
            );
            Console.WriteLine("*****************");
        }
        static void HandleEmployeeProcessed(object? sender, EmployeeEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("***** EVENT *****");
            Console.WriteLine(
                $"Employee {e.employee.Name} has been onboarded and is now active."
            );
            Console.WriteLine("*****************");
        }
        static void AddEmployeeMenu(Company company)
        {
            Console.WriteLine("===== Add Employee =====");

            int id = ReadInt("ID: ");

            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "";

            DateOnly hireDate = ReadDate("Hire Date (yyyy-MM-dd): ");

            int departmentId = ReadInt("Department ID: ");

            decimal salary = ReadDecimal("Salary: ");


            Employee employee = new Employee
            {
                Id = id,
                Name = name,
                HireDate = hireDate,
                DepartmentId = departmentId,
                Salary = salary
            };


            var result = company.AddEmployee(employee);

            DisplayResult(result);

            if (result.Success)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Employee was added to the onboarding queue."
                );
            }
        }
        static void ProcessEmployeeMenu(Company company)
        {
            Console.WriteLine("===== Process Next Employee =====");

            var result = company.ProcessNextEmployeeInOnBoarding();

            DisplayResult(result);
        }
        static void PromoteEmployeeMenu(Company company)
        {
            Console.WriteLine("===== Promote Employee =====");

            int id = ReadInt("Employee ID: ");

            var result = company.PromoteEmployee(id);

            DisplayResult(result);

            if (result.Success && result.Data is Manager manager)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"{manager.Name} is now a Manager."
                );
            }
        }
        static void AddDepartmentMenu(Company company)
        {
            Console.WriteLine("===== Add Department =====");

            int id = ReadInt("Department ID: ");

            Console.Write("Department Name: ");
            string name = Console.ReadLine() ?? "";


            Department department = new Department
            {
                Id = id,
                Name = name
            };


            var result = company.AddDepartment(department);

            DisplayResult(result);
        }
        static void AddSkillMenu(Company company)
        {
            Console.WriteLine("===== Add Skill =====");

            int employeeId = ReadInt("Employee ID: ");

            Console.Write("Skill: ");
            string skill = Console.ReadLine() ?? "";


            var result = company.AddSkillToEmployee(
                employeeId,
                skill
            );

            DisplayResult(result);
        }
        static void SearchEmployeeByIdMenu(Company company)
        {
            Console.WriteLine("===== Search Employee By ID =====");

            int id = ReadInt("Employee ID: ");

            Employee? employee = company.FindEmployeeById(id);

            if (employee is null)
            {
                Console.WriteLine("Employee not found.");
                return;
            }

            DisplayEmployee(employee);
        }
        static void SearchEmployeeByNameMenu(Company company)
        {
            Console.WriteLine("===== Search Employee By Name =====");

            Console.Write("Employee Name: ");
            string name = Console.ReadLine() ?? "";


            Employee? employee = company.FindEmployeeByName(name);

            if (employee is null)
            {
                Console.WriteLine("Employee not found.");
                return;
            }

            DisplayEmployee(employee);
        }
        static void ShowDepartmentEmployeesMenu(Company company)
        {
            Console.WriteLine("===== Department Employees =====");

            int departmentId = ReadInt("Department ID: ");

            try
            {
                List<Employee> employees =
                    company.GetAllEmployeesOfDepartmentById(
                        departmentId
                    );

                if (employees.Count == 0)
                {
                    Console.WriteLine(
                        "There are no employees in this department."
                    );

                    return;
                }

                Console.WriteLine();
                Console.WriteLine(
                    $"Employees in Department {departmentId}:"
                );

                foreach (Employee employee in employees)
                {
                    DisplayEmployee(employee);
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void FilterEmployeesMenu(Company company)
        {
            Console.WriteLine("===== Filter Employees =====");
            Console.WriteLine("1. Managers only");
            Console.WriteLine("2. Salary above 5000");
            Console.WriteLine("3. Salary below 5000");
            Console.WriteLine("4. Department ID");
            Console.WriteLine("5. Name starts with a letter");

            int choice = ReadInt("Choose filter: ");

            List<Employee> employees;


            switch (choice)
            {
                case 1:

                    employees = company.FilterEmployee(
                        employee => employee is Manager
                    );

                    break;


                case 2:

                    employees = company.FilterEmployee(
                        employee => employee.Salary > 5000
                    );

                    break;


                case 3:

                    employees = company.FilterEmployee(
                        employee => employee.Salary < 5000
                    );

                    break;


                case 4:

                    int departmentId =
                        ReadInt("Department ID: ");

                    employees = company.FilterEmployee(
                        employee =>
                            employee.DepartmentId == departmentId
                    );

                    break;


                case 5:

                    Console.Write("Enter first letter: ");
                    string letter =
                        Console.ReadLine() ?? "";

                    if (string.IsNullOrEmpty(letter))
                    {
                        Console.WriteLine(
                            "Invalid letter."
                        );

                        return;
                    }

                    employees = company.FilterEmployee(
                        employee =>
                            employee.Name.StartsWith(
                                letter,
                                StringComparison.OrdinalIgnoreCase
                            )
                    );

                    break;


                default:

                    Console.WriteLine("Invalid filter.");
                    return;
            }


            Console.WriteLine();

            if (employees.Count == 0)
            {
                Console.WriteLine(
                    "No employees matched the filter."
                );

                return;
            }


            Console.WriteLine(
                $"Found {employees.Count} employee(s):"
            );

            foreach (Employee employee in employees)
            {
                DisplayEmployee(employee);
            }
        }
        static void ShowAverageSalary(Company company)
        {
            Console.WriteLine("===== Average Salary =====");

            decimal average = company.CalculateAverageSalary();

            Console.WriteLine(
                $"Average salary: {average:F2}"
            );
        }
        static void DisplayEmployee(Employee employee)
        {
            Console.WriteLine("------------------------------");

            Console.WriteLine($"ID: {employee.Id}");
            Console.WriteLine($"Name: {employee.Name}");
            Console.WriteLine(
                $"Hire Date: {employee.HireDate:yyyy-MM-dd}"
            );
            Console.WriteLine(
                $"Department ID: {employee.DepartmentId}"
            );
            Console.WriteLine(
                $"Salary: {employee.Salary:F2}"
            );

            if (employee is Manager manager)
            {
                Console.WriteLine("Position: Manager");
                Console.WriteLine(
                    $"Team Members: {manager.TeamMembers.Count}"
                );
            }
            else
            {
                Console.WriteLine("Position: Employee");
            }

            Console.WriteLine("Skills:");

            if (employee.Skills.Count == 0)
            {
                Console.WriteLine("  None");
            }
            else
            {
                foreach (string skill in employee.Skills)
                {
                    Console.WriteLine($"  - {skill}");
                }
            }

            Console.WriteLine("------------------------------");
        }
        static void DisplayResult<T>(Result<T> result)
        {
            Console.WriteLine();

            if (result.Success)
            {
                Console.WriteLine("SUCCESS");
            }
            else
            {
                Console.WriteLine("FAILED");
            }

            Console.WriteLine($"Message: {result.Message}");

            if (result.Data is not null)
            {
                Console.WriteLine($"Data: {result.Data}");
            }
        }
        static int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);

                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out int value))
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid input. Please enter an integer."
                );
            }
        }
        static decimal ReadDecimal(string message)
        {
            while (true)
            {
                Console.Write(message);

                string input = Console.ReadLine() ?? "";

                if (decimal.TryParse(input, out decimal value))
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid input. Please enter a number."
                );
            }
        }
        static DateOnly ReadDate(string message)
        {
            while (true)
            {
                Console.Write(message);

                string input = Console.ReadLine() ?? "";

                if (DateOnly.TryParse(input, out DateOnly value))
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid date. Example: 2026-08-12"
                );
            }
        }
        static void Main(string[] args)
        {
            Company company = new Company();
            company.OnEmployeePromoted += HandleEmployeePromoted;
            company.OnEmployeeProcessed += HandleEmployeeProcessed;
            SeedData(company);
            bool running = true;

            do
            {
                Console.Clear();

                Console.WriteLine("==========================================");
                Console.WriteLine("       EMPLOYEE MANAGEMENT SYSTEM");
                Console.WriteLine("==========================================");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Process Next Employee");
                Console.WriteLine("3. Add Department");
                Console.WriteLine("4. Promote Employee");
                Console.WriteLine("5. Add Skill To Employee");
                Console.WriteLine("6. Search Employee By ID");
                Console.WriteLine("7. Search Employee By Name");
                Console.WriteLine("8. Show Department Employees");
                Console.WriteLine("9. Filter Employees");
                Console.WriteLine("10. Calculate Average Salary");
                Console.WriteLine("11. Department Report");
                Console.WriteLine("12. Show Action History");
                Console.WriteLine("13. Show Company Skills");
                Console.WriteLine("0. Exit");
                Console.WriteLine("==========================================");

                int choice = ReadInt("Enter your choice: ");

                Console.Clear();

                switch (choice)
                {
                    case 1:
                        AddEmployeeMenu(company);
                        break;

                    case 2:
                        ProcessEmployeeMenu(company);
                        break;

                    case 3:
                        AddDepartmentMenu(company);
                        break;

                    case 4:
                        PromoteEmployeeMenu(company);
                        break;

                    case 5:
                        AddSkillMenu(company);
                        break;

                    case 6:
                        SearchEmployeeByIdMenu(company);
                        break;

                    case 7:
                        SearchEmployeeByNameMenu(company);
                        break;

                    case 8:
                        ShowDepartmentEmployeesMenu(company);
                        break;

                    case 9:
                        FilterEmployeesMenu(company);
                        break;

                    case 10:
                        ShowAverageSalary(company);
                        break;

                    case 11:
                        company.DisplayDepartmentsReport();
                        break;

                    case 12:
                        company.DisplayActionHistory();
                        break;

                    case 13:
                        company.DisplayCompanySkills();
                        break;

                    case 0:
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine();
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ReadLine();
                }

            } while (running);
            company.OnEmployeePromoted -= HandleEmployeePromoted;
            company.OnEmployeeProcessed -= HandleEmployeeProcessed;
        }
    }
}

