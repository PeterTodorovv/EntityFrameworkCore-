using SoftUni.Data;
using SoftUni.Models;
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
            Console.WriteLine(GetEmployee147(context));
        }

        //3
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

        //4
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

        //5
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

        //6
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var newAdress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(newAdress);

            var employee = context.Employees.Where(e => e.LastName == "Nakov").First();
            employee.Address = newAdress;
            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => e.Address.AddressText)
                .Take(10).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var employeeAdress in employees)
            {
                sb.AppendLine(employeeAdress);
            }

            return sb.ToString();
        }

        //7
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                
                .Where(e => e.EmployeesProjects
                .Any( ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects.Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        ProjectStartDate = ep.Project.StartDate,
                        ProjectEndDate = ep.Project.EndDate
                    })
                }).Take(10);

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {
                    if(project.ProjectEndDate != null)
                    {
                        sb.AppendLine($"--{project.ProjectName} - {project.ProjectStartDate} - {project.ProjectEndDate}");
                    }
                    else
                    {
                        sb.AppendLine($"--{project.ProjectName} - {project.ProjectStartDate} - not finished");
                    }
                }
            }

            return sb.ToString();
        }

        //8
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var adresses = context.Addresses
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = (context.Employees.Where(e => e.AddressId == a.AddressId)).Count()
                })
                .OrderByDescending( a => a.EmployeeCount).ThenBy( a => a.TownName).ThenBy(a => a.AddressText);

            var sb = new StringBuilder();

            foreach(var adress in adresses)
            {
                sb.AppendLine($"{adress.AddressText}, {adress.TownName} - {adress.EmployeeCount} employees");
            }

            return sb.ToString();
        }

        //9
        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees.Where(e => e.EmployeeId == 147)
                .Select( e => new {
                    e.FirstName, 
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects.Select( ep => ep.Project.Name)
                }).First();

            var sb = new StringBuilder();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var project in employee.Projects.OrderBy(e => e))
            {
                sb.AppendLine(project);
            }

            return sb.ToString();
        }

        //10
    } 
}
