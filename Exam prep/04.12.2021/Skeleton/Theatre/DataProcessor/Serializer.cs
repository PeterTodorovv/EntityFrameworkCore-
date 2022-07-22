namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context.Theatres.Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20).ToArray()
                .Select(t => new 
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets.Where(tck => tck.RowNumberr<=5).Sum(tck => tck.Price),
                    Tickets = t.Tickets.Where(tck => tck.RowNumberr <= 5)
                    .Select(tck => new
                    {
                        Price = decimal.Parse($"{tck.Price:f2}"),
                        RowNumber = tck.RowNumberr
                    }).OrderByDescending(tck => tck.Price).ToArray()
                }).OrderByDescending(t => t.Halls).ThenBy(t => t.Name).ToArray();

            string json = JsonConvert.SerializeObject(theatres, Formatting.Indented);
            return json;
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            XmlRootAttribute root = new XmlRootAttribute("Plays");
            XmlSerializer serializer = new XmlSerializer(typeof(PlaysExportDto[]), root);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            PlaysExportDto[] plays = context.Plays.Where(p => p.Rating <= rating).ToArray()
                .Select(p => new PlaysExportDto()
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts.Where(a => a.IsMainCharacter == true)
                    .Select(a => new ActorExportDto()
                    {
                        FullName = a.FullName,
                        MainCharacter = $"Plays main character in '{p.Title}'."
                    }).OrderByDescending(a => a.FullName).ToArray()
                }).OrderBy(p => p.Title).ThenByDescending(p => p.Genre).ToArray();

            serializer.Serialize(writer, plays, namespaces);

            return sb.ToString();
        }
    }
}
