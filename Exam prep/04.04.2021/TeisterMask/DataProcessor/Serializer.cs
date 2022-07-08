namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            XmlRootAttribute root = new XmlRootAttribute("Projects");
            XmlSerializer serializer = new XmlSerializer(typeof(ExportProjectWithTasks[]), root);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            
            ExportProjectWithTasks[] projects = context.Projects.Where(p => p.Tasks.Any())
                .ToArray()
                .Select(p => new ExportProjectWithTasks()
                {
                    TasksCount = p.Tasks.Count.ToString(),
                    ProjectName = p.Name,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    Tasks = p.Tasks.ToArray()
                    .OrderBy(t => t.Name)
                    .Select(t => new ExportTaskDto()
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString()
                    })
                    .ToArray()
                })
                .OrderByDescending(p => p.TasksCount)
                .ThenBy(p => p.ProjectName)
                .ToArray();

            serializer.Serialize(writer, projects, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .Where(e => e.EmployeesTasks.Any(t => t.Task.OpenDate >= date))
                .ToArray()
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks.Where(t => t.Task.OpenDate >= date)
                    .ToArray()
                    .OrderByDescending(et => et.Task.DueDate)
                    .ThenBy(et => et.Task.Name)
                    .Select(t => new
                    {
                        TaskName = t.Task.Name,
                        OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = t.Task.LabelType.ToString(),
                        ExecutionType = t.Task.ExecutionType.ToString()
                    })
                    .ToArray()
                })
                .OrderByDescending(e => e.Tasks.Length)
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(employees, Formatting.Indented);
        }
    }
}