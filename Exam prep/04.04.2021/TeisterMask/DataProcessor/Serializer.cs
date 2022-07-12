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
    using TeisterMask.Data.Models;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            XmlRootAttribute root = new XmlRootAttribute("Projects");
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectExportDto[]), root);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);


            ProjectExportDto[] Projects = context.Projects.Where(p => p.Tasks.Any())
                .OrderByDescending(p => p.Tasks.Count).ThenBy(p => p.Name)
                .Select(p => new ProjectExportDto
                {
                    TasksCount = p.Tasks.Count.ToString(),
                    ProjectName = p.Name,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    Tasks = p.Tasks.Select(t => new TaskExportDto()
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString()
                    }).OrderBy(t => t.Name).ToArray()
                }).ToArray();

            serializer.Serialize(writer, Projects, namespaces);
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
                    Tasks = e.EmployeesTasks
                    .Where(t => t.Task.OpenDate >= date)
                    .OrderByDescending( et => et.Task.DueDate).ThenBy(et => et.Task.Name)
                    .Select(et => new
                    {
                        TaskName = et.Task.Name,
                        OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = et.Task.LabelType.ToString(),
                        ExecutionType = et.Task.ExecutionType.ToString()
                    }).ToArray()
                }).OrderByDescending(e => e.Tasks.Count()).ThenBy(e => e.Username)
                .Take(10).ToArray();

            string json = JsonConvert.SerializeObject(employees, Formatting.Indented);
            return json;
        }
    }
}