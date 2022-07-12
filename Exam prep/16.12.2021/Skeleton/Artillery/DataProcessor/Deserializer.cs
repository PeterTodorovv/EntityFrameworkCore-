namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Country> countries = new List<Country>();

            XmlRootAttribute root = new XmlRootAttribute("Countries");
            XmlSerializer serializer = new XmlSerializer(typeof(CountriesImportDto[]), root);

            StringReader reader = new StringReader(xmlString);

            CountriesImportDto[] countriesImports = (CountriesImportDto[])serializer.Deserialize(reader);

            foreach (var countryDto in countriesImports)
            {
                if (!IsValid(countryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = new Country()
                {
                    CountryName = countryDto.CountryName,
                    ArmySize = int.Parse(countryDto.ArmySize)
                };

                if (!IsValid(country))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
                countries.Add(country);
            }

            context.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Manufacturer> manufacturers = new List<Manufacturer>();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Manufacturers");
            XmlSerializer serializer = new XmlSerializer(typeof(ManufacturersInportDto[]), xmlRoot);
            StringReader reader = new StringReader(xmlString);

            ManufacturersInportDto[] manufacturersInports = (ManufacturersInportDto[])serializer.Deserialize(reader);

            foreach (var manifacturerDto in manufacturersInports)
            {
                if (!IsValid(manifacturerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = manifacturerDto.ManufacturerName,
                    Founded = manifacturerDto.Founded,
                };

                if (manufacturers.Any(m => m.ManufacturerName == manufacturer.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                manufacturers.Add(manufacturer);
                string[] foundedElements = manufacturer.Founded.Split(", ");
                string city = foundedElements[foundedElements.Length - 2];
                string country = foundedElements[foundedElements.Length - 1];
                sb.AppendLine(String.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, string.Join(", ", city, country)));
            }

            context.AddRange(manufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Shell> shells = new List<Shell>();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Shells");
            XmlSerializer serializer = new XmlSerializer(typeof(ShellsImportDto[]), xmlRoot);
            StringReader reader = new StringReader(xmlString);

            ShellsImportDto[] shellsImports = (ShellsImportDto[])serializer.Deserialize(reader);

            foreach (var shellDto in shellsImports)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = new Shell()
                {
                    ShellWeight = double.Parse(shellDto.ShellWeight),
                    Caliber = shellDto.Caliber
                };

                if (!IsValid(shell))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
                shells.Add(shell);
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Gun> guns = new List<Gun>();

            GunsImportDto[] gunsImports = JsonConvert.DeserializeObject<GunsImportDto[]>(jsonString);

            foreach(var gunDto in gunsImports)
            {
                if (!IsValid(gunDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var manifacturer = context.Manufacturers.FirstOrDefault(m => m.Id == gunDto.ManufacturerId);

                if(manifacturer == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var shell = context.Shells.FirstOrDefault(s => s.Id == gunDto.ShellId);

                if(shell == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool IsValidGunType = Enum.TryParse(typeof(GunType), gunDto.GunType, true, out  object gunType);

                if (!IsValidGunType)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Gun gun = new Gun()
                {
                    Manufacturer = manifacturer,
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    NumberBuild = gunDto.NumberBuild,
                    Range = gunDto.Range,
                    GunType = (GunType)Enum.Parse(typeof(GunType), gunDto.GunType),
                    Shell = shell
                };

                foreach(var countryId in gunDto.Countries)
                {
                    var country = context.Countries.Find(countryId.Id);
                    CountryGun countryGun = new CountryGun()
                    {
                        Gun = gun,
                        Country = country
                    };

                    gun.CountriesGuns.Add(countryGun);
                }

                guns.Add(gun);
                sb.AppendLine(String.Format(SuccessfulImportGun,
                    gun.GunType.ToString(), gun.GunWeight, gun.BarrelLength));
            }

            context.Guns.AddRange(guns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
