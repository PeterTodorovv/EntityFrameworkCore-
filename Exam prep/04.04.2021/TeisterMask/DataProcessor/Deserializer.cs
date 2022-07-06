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
            XmlRootAttribute root = new XmlRootAttribute("Projects");
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectImportDto[]), root);

            StringReader reader = new StringReader(xmlString);

            ProjectImportDto[] projectsDtos = (ProjectImportDto[])serializer.Deserialize(reader);
            HashSet<Project> projects = new HashSet<Project>();

            StringBuilder message = new StringBuilder();

            foreach (var projectDto in projectsDtos)
            {
                if (!IsValid(projectDto))
                {
                    message.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(projectDto.Name))
                {
                    message.AppendLine(ErrorMessage);
                    continue;
                }

                bool isOpenDateValid = DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy"
                    , CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime projectOpenDateValue);

                if (!isOpenDateValid)
                {
                    message.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? projectDueDateValue = null;
                if (!string.IsNullOrWhiteSpace(projectDto.DueDate))
                {
                    bool isDueDateValid = DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy"
                    , CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDateValue);

                    if (!isDueDateValid)
                    {
                        message.AppendLine(ErrorMessage);
                        continue;
                    }

                    projectDueDateValue = dueDateValue;
                }

                Project project = new Project()
                {
                    Name = projectDto.Name,
                    OpenDate = DateTime.Parse(projectDto.OpenDate),
                    DueDate = DateTime.Parse(projectDto.DueDate)
                };


                foreach (var taskDto in projectDto.Tasks)
                {
                    if (!IsValid(taskDto))
                    {
                        message.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (string.IsNullOrEmpty(taskDto.Name))
                    {
                        message.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(string.IsNullOrEmpty(taskDto.DueDate) || string.IsNullOrEmpty(taskDto.OpenDate))
                    {
                        message.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isTAskOpenDateValid = DateTime.TryParseExact(taskDto.OpenDate, "dd/MM/yyyy"
                    , CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskOpenDateValue);

                    if (!isOpenDateValid)
                    {
                        message.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isDueDateValid = DateTime.TryParseExact(taskDto.OpenDate, "dd/MM/yyyy"
                    , CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDueDateValue);

                    if (!isDueDateValid)
                    {
                        message.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(taskDueDateValue > projectDueDateValue || taskOpenDateValue > projectOpenDateValue)
                    {
                        message.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task task = new Task()
                    {
                        Name = taskDto.Name,
                        OpenDate = taskOpenDateValue,
                        DueDate = taskDueDateValue,
                        ExecutionType = (ExecutionType) taskDto.ExecutionType,
                        LabelType = (LabelType)taskDto.LabelType,
                        Project = project
                    };

                    project.Tasks.Add(task);
                }

                projects.Add(project);
                message.Append(String.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            context.Projects.AddRange(projects);
            return message.ToString();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}