using SoftUni.Data;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();
            Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
                StringBuilder sb = new StringBuilder();

                var employees = context.Employees
                    .Select( e => new { e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary,
                    e.EmployeeId})
                    .OrderBy(e => e.EmployeeId);
                    
                foreach(var employee in employees)
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
                }

                return sb.ToString();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                   .Select(e => new
                   {
                       e.FirstName,
                       e.Salary
                   }
                   ).Where(e => e.Salary > 50000)
                   .OrderBy(e => e.FirstName);
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                   .Select(e => new
                   {
                       e.FirstName,
                       e.LastName,
                       e.Department.Name,
                       e.Salary
                   }
                   ).Where(d => d.Name == "Research and Development")
                   .OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName);

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Name} - ${employee.Salary:f2}");
            }

            return sb.ToString();
        }
    }
}
