using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystemProject2.Common
{
    public class Result<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = "";

        public T? Data {  get; set; }

        public override string ToString()
        {
            return $"Result: {(Success ? "Success":"Fail")}" +
                $"Message: {Message}" +
                $"Data: {(Success?Data?.ToString():"NoData")}";
        }
    }
}
