
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var Shells = context.Shells.Where(s => s.ShellWeight > shellWeight)
                .ToArray()
                .Select(s => new
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns.Where(g => g.GunType.ToString() == "AntiAircraftGun")
                    .Select(g => new
                    {
                        GunType = g.GunType.ToString(),
                        GunWeight = g.GunWeight,
                        BarrelLength = g.BarrelLength,
                        Range = g.Range > 3000 ? "Long-range" : "Regular range"
                    }).OrderByDescending(g => g.GunWeight).ToArray(),
                }).OrderBy(s => s.ShellWeight).ToArray();

            string jsonString = JsonConvert.SerializeObject(Shells, Formatting.Indented);

            return jsonString;
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            XmlRootAttribute root = new XmlRootAttribute("Guns");
            XmlSerializer serializer = new XmlSerializer(typeof(GunsExportDto[]), root);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            GunsExportDto[] Guns = context.Guns.Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .OrderBy(g => g.BarrelLength).ToArray()
                .Select(g => new GunsExportDto()
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    GunWeight = g.GunWeight.ToString(),
                    BarrelLength = g.BarrelLength.ToString(),
                    Range = g.Range.ToString(),
                    Countries = g.CountriesGuns.Where(cg => cg.Country.ArmySize > 4500000)
                    .Select(cg => new CountryExportDto()
                    {
                        Country = cg.Country.CountryName,
                        ArmySize = cg.Country.ArmySize.ToString()
                    }).OrderBy(cg => cg.ArmySize).ToArray()
                }).ToArray();
            
            serializer.Serialize(writer, Guns, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
