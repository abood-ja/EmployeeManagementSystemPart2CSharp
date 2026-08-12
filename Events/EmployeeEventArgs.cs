using EmployeeManagementSystemProject2.Models;

namespace EmployeeManagementSystemProject2.Services
{
    public class EmployeeEventArgs
    {
        public Employee employee {  get; set; }
        public EmployeeEventArgs(Employee emp)
        {
            employee =emp;
        }
    }
}