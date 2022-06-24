using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            //DatabaseReset(context);
            var xmlInput = File.ReadAllText("../../../Datasets/categories.xml");
            Console.WriteLine(ImportCategories(context, xmlInput));
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Users");
            XmlSerializer serializer = new XmlSerializer(typeof(UsersDto[]), xmlRoot);

            StringReader reader = new StringReader(inputXml);
            UsersDto[] usersDtos = (UsersDto[])serializer.Deserialize(reader);

            ICollection<User> Users = new HashSet<User>();

            foreach(var userDtos in usersDtos)
            {
                var user = new User()
                {
                    FirstName = userDtos.FirstName,
                    LastName = userDtos.LastName,
                    Age = userDtos.Age
                };

                Users.Add(user);
            }

            context.Users.AddRange(Users);
            context.SaveChanges();

            return $"Successfully imported {Users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Products");
            XmlSerializer serializer = new XmlSerializer(typeof(ProductsImportDto[]), root);

            StringReader reader = new StringReader(inputXml);
            ProductsImportDto[] productsImportDto = (ProductsImportDto[])serializer.Deserialize(reader);

            ICollection<Product> products = new HashSet<Product>();

            foreach(var productDto in productsImportDto)
            {
                var product = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    SellerId = productDto.SellerId,
                    BuyerId = productDto.BuyerId
                };

                products.Add(product);
            }

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Categories");
            XmlSerializer serializer = new XmlSerializer(typeof(CategoriesInputDto[]), root);

            StringReader reader = new StringReader(inputXml);
            CategoriesInputDto[] categoriesInputDto = (CategoriesInputDto[])serializer.Deserialize(reader);

            ICollection<Category> categories = new HashSet<Category>();

            foreach(var categoryDto in categoriesInputDto)
            {
                Category category = new Category()
                {
                    Name = categoryDto.Name
                };

                categories.Add(category);
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        private static void DatabaseReset(ProductShopContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            System.Console.WriteLine("Db reseted!");
        }
    }
}