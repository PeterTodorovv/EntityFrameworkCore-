using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext context = new CarDealerContext();
            //ResetDb(context);

            //string xmlText = File.ReadAllText("../../../Datasets/sales.xml");
            Console.WriteLine(GetTotalSalesByCustomer(context));
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Suppliers");
            XmlSerializer serializer = new XmlSerializer(typeof(SuppliersImportDto[]), root);

            StringReader reader = new StringReader(inputXml);
            SuppliersImportDto[] inputDto = (SuppliersImportDto[])serializer.Deserialize(reader);

            ICollection<Supplier> suppliers = new HashSet<Supplier>();

            foreach (var supplierDto in inputDto)
            {
                Supplier supplier = new Supplier()
                {
                    Name = supplierDto.Name,
                    IsImporter = bool.Parse(supplierDto.IsImporter)
                };

                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Parts");
            XmlSerializer serializer = new XmlSerializer(typeof(PartsImportDto[]), root);

            StringReader reader = new StringReader(inputXml);
            PartsImportDto[] inputDto = (PartsImportDto[])serializer.Deserialize(reader);

            ICollection<Part> parts = new HashSet<Part>();

            foreach (var partDto in inputDto)
            {
                if (context.Suppliers.Any(s => s.Id == partDto.SupplierId))
                {
                    Part part = new Part()
                    {
                        Name = partDto.Name,
                        Price = partDto.Price,
                        Quantity = partDto.Quantity,
                        SupplierId = partDto.SupplierId
                    };

                    parts.Add(part);
                }
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Cars");
            XmlSerializer serializer = new XmlSerializer(typeof(CarsImportDto[]), root);

            StringReader reader = new StringReader(inputXml);
            CarsImportDto[] inputDto = (CarsImportDto[])serializer.Deserialize(reader);

            ICollection<Car> cars = new HashSet<Car>();

            foreach (var cardto in inputDto)
            {
                ICollection<Part> parts = new HashSet<Part>();

                foreach (var partDto in cardto.carPartImports.Distinct())
                {
                    Part part = context.Parts.Find(partDto.PartId);

                    if(part == null)
                    {
                        continue;
                    }

                    parts.Add(part);
                }

                Car car = new Car()
                {
                    Make = cardto.Make,
                    Model = cardto.Model,
                    TravelledDistance = cardto.TravelledDistance,
                    PartCars = parts.Select(p => new PartCar()
                    {
                        PartId = p.Id
                    }).ToArray()
                };

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Customers");
            XmlSerializer serializer = new XmlSerializer(typeof(CustomersImportDto[]), root);

            StringReader reader = new StringReader(inputXml);
            CustomersImportDto[] inputDto = (CustomersImportDto[])serializer.Deserialize(reader);

            ICollection<Customer> customers = new HashSet<Customer>();

            foreach (var cistomerDto in inputDto)
            {
                Customer customer = new Customer()
                {
                    Name = cistomerDto.Name,
                    BirthDate = DateTime.Parse(cistomerDto.BirthDate),
                    IsYoungDriver = bool.Parse(cistomerDto.IsYoungDriver)
                };

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Sales");
            XmlSerializer serializer = new XmlSerializer(typeof(SalesImportDto[]), root);

            StringReader reader = new StringReader(inputXml);
            SalesImportDto[] inputDto = (SalesImportDto[])serializer.Deserialize(reader);

            ICollection<Sale> sales = new HashSet<Sale>();

            foreach(var saleDto in inputDto)
            {
                Car car = context.Cars.Find(saleDto.CarId);
                if(car == null)
                {
                    continue;
                }

                Sale sale = new Sale()
                {
                    CarId = saleDto.CarId,
                    CustomerId = saleDto.CustomerId,
                    Discount = saleDto.Discount,
                };

                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("cars");
            XmlSerializer serializer = new XmlSerializer(typeof(CarsWithDistanceExportDto[]), xmlRoot);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(String.Empty, string.Empty);

            CarsWithDistanceExportDto[] carsDto = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make).ThenBy(c => c.Model).Take(10)
                .Select(c => new CarsWithDistanceExportDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance.ToString()
                }).ToArray();

            serializer.Serialize(writer, carsDto, xmlSerializerNamespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            XmlRootAttribute root = new XmlRootAttribute("cars");
            XmlSerializer serializer = new XmlSerializer(typeof(BMWExportDto[]), root);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(String.Empty, string.Empty);

            BMWExportDto[] BMWs = context.Cars.Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model).ThenByDescending(c => c.TravelledDistance)
                .Select(c => new BMWExportDto()
                {
                    Id = c.Id.ToString(),
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance.ToString()
                }).ToArray();

            serializer.Serialize(writer, BMWs, xmlSerializerNamespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            XmlRootAttribute root = new XmlRootAttribute("suppliers");
            XmlSerializer serializer = new XmlSerializer(typeof(LocalSuppliersExportDto[]), root);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(String.Empty, string.Empty);

            LocalSuppliersExportDto[] localSuppliers = context.Suppliers.Where(s => s.IsImporter == false)
                .Select(s => new LocalSuppliersExportDto()
                {
                    Id =s.Id.ToString(),
                    Name = s.Name,
                    PartsCount = s.Parts.Count.ToString()
                }).ToArray();

            serializer.Serialize(writer, localSuppliers, xmlSerializerNamespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            XmlRootAttribute root = new XmlRootAttribute("cars");
            XmlSerializer serializer = new XmlSerializer(typeof(CarsExportDto[]), root);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(String.Empty, string.Empty);

            CarsExportDto[] carsExports = context.Cars
                .OrderByDescending(c => c.TravelledDistance).ThenBy(c => c.Model).Take(5)
                .Select(c => new CarsExportDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance.ToString(),
                    Parts = c.PartCars.OrderByDescending(cp => cp.Part.Price).Select(pt => new CarPartsDto()
                    {
                        Name = pt.Part.Name,
                        Price = pt.Part.Price.ToString()
                    }).ToArray()
                }).ToArray();

            serializer.Serialize(writer, carsExports, xmlSerializerNamespaces);
            return sb.ToString().TrimEnd();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            XmlRootAttribute root = new XmlRootAttribute("customers");
            XmlSerializer serializer = new XmlSerializer(typeof(CustomersTotalSalesExportDto[]), root);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(String.Empty, string.Empty);

            CustomersTotalSalesExportDto[] customers = context.Customers.Where(c => c.Sales.Count >= 1)
                .Select(c => new CustomersTotalSalesExportDto()
            {
                FullName = c.Name,
                BoughtCars = c.Sales.Count.ToString(),
                SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price)).ToString()
            }).OrderBy(c => c.SpentMoney).ToArray();

            serializer.Serialize(writer, customers, xmlSerializerNamespaces);

            return sb.ToString().TrimEnd();
        }

        private static void ResetDb(DbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            Console.WriteLine("Db reseted!");
        }
    }
}