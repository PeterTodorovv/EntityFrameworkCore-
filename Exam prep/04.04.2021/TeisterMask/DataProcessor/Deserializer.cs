namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Project> projects = new List<Project>();

            XmlRootAttribute root = new XmlRootAttribute("Projects");
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectImportDto[]), root);
            StringReader reader = new StringReader(xmlString);

            ProjectImportDto[] projectImports = (ProjectImportDto[])serializer.Deserialize(reader);

            foreach(var projectDto in projectImports)
            {
                if (!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isOpenDateValid = DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy"
                    , CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime OpenDate);

                if (!isOpenDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? DueDate = null;

                if (!string.IsNullOrEmpty(projectDto.DueDate))
                {
                    bool isDueDateValid = DateTime.TryParseExact(projectDto.DueDate, "dd/MM/yyyy"
                        , CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime DueDateValue);

                    if (!isDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DueDate = DueDateValue;
                }

                Project project = new Project()
                {
                    Name = projectDto.Name,
                    OpenDate = OpenDate,
                    DueDate = DueDate
                };

                foreach (var taskDto in projectDto.Tasks)
                {
                    if (!IsValid(taskDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isTaskOpenDateValid = DateTime.TryParseExact(taskDto.OpenDate, "dd/MM/yyyy"
                    , CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime TaskOpenDate);

                    if (!isTaskOpenDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isTaskDueDateValid = DateTime.TryParseExact(taskDto.DueDate, "dd/MM/yyyy"
                        , CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime TaskDueDate);

                    if (!isTaskDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(TaskOpenDate < OpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(DueDate.HasValue && TaskDueDate > DueDate.Value)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task task = new Task()
                    {
                        Name = taskDto.Name,
                        OpenDate = TaskOpenDate,
                        DueDate = TaskDueDate,
                        ExecutionType = (ExecutionType)int.Parse(taskDto.ExecutionType),
                        LabelType = (LabelType)int.Parse(taskDto.LabelType)
                    };

                    project.Tasks.Add(task);
                }
                projects.Add(project);
                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }
            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Employee> employees = new List<Employee>();
            EmployeeImportDto[] employeeImports = JsonConvert.DeserializeObject<EmployeeImportDto[]>(jsonString);

            foreach(var employeeDto in employeeImports)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee()
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone
                };

                HashSet<int> tasksIds = new HashSet<int>();

                foreach(var taskId in employeeDto.Tasks)
                {
                    var task = context.Tasks.Find(taskId);
                    if(task == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (tasksIds.Contains(taskId))
                    {
                        continue;
                    }

                    EmployeeTask employeeTask = new EmployeeTask()
                    {
                        Employee = employee,
                        Task = task
                    };
                    employee.EmployeesTasks.Add(employeeTask);
                    tasksIds.Add(taskId);
                }

                sb.AppendLine(String.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
                employees.Add(employee);
            }

            context.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}