using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            //var xmlInput = File.ReadAllText("../../../Datasets/categories-products.xml");
            Console.WriteLine(GetUsersWithProducts(context));
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
                if(categoryDto.Name == null)
                {
                    continue;
                }

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

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("CategoryProducts");
            XmlSerializer serializer = new XmlSerializer (typeof(CategoriesProductsInputDto[]), xmlRoot);

            StringReader reader = new StringReader(inputXml);
            CategoriesProductsInputDto[] CProductDto = (CategoriesProductsInputDto[])serializer.Deserialize(reader);

            ICollection<CategoryProduct> CategoryProducts = new HashSet<CategoryProduct>();

            foreach(var cproductDto in CProductDto)
            {
                if(context.Categories.Any(c => c.Id == cproductDto.CategoryId) && context.Products.Any(p => p.Id == cproductDto.ProductId))
                {
                    CategoryProduct categoryProduct = new CategoryProduct()
                    {
                        CategoryId = cproductDto.CategoryId,
                        ProductId = cproductDto.ProductId
                    };

                    CategoryProducts.Add(categoryProduct);
                }
                
            }

            context.CategoryProducts.AddRange(CategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {CategoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            XmlRootAttribute root = new XmlRootAttribute("Products");
            XmlSerializer serializer = new XmlSerializer(typeof(ProductExportDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty ,string.Empty);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            ProductExportDto[] productsInRange = context.Products.Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price).Take(10)
                .Select(p => new ProductExportDto()
                {
                    Name = p.Name,
                    Price = p.Price.ToString(),
                    BuyerFullName = $"{p.Seller.FirstName} {p.Seller.LastName}"
                }).ToArray();

            serializer.Serialize(writer, productsInRange, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            XmlRootAttribute root = new XmlRootAttribute("Users");
            XmlSerializer serializer = new XmlSerializer(typeof(SellersSoldItemsOutputDto[]), root);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            SellersSoldItemsOutputDto[] sellers = context.Users.Where(s => s.ProductsSold.Count > 1)
                .OrderBy(s => s.LastName).Take(5)
                .Select(s => new SellersSoldItemsOutputDto
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    SoldProducts = s.ProductsSold.Select(sp => new SoldProductsDto
                    {
                        Name = sp.Name,
                        Price = sp.Price.ToString()
                    }).ToArray()
                }).ToArray();

            serializer.Serialize(writer, sellers, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Category");
            XmlSerializer serializer = new XmlSerializer(typeof(CategoriesOutputDto[]), xmlRoot);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            CategoriesOutputDto[] categoriesOutputs = context.Categories
                .Select(c => new CategoriesOutputDto
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count().ToString(),
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price).ToString(),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price).ToString()
                }).OrderByDescending(c => c.Count).ToArray();

            serializer.Serialize(writer, categoriesOutputs, namespaces);
            return sb.ToString().TrimEnd();
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            XmlRootAttribute root = new XmlRootAttribute("Users");
            XmlSerializer serializer = new XmlSerializer(typeof(UsersWithSoldProductsOutputDto[]), root);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            UsersWithSoldProductsOutputDto[] usersWithSoldProducts = context.Users.Select(u => new UsersWithSoldProductsOutputDto() 
            {
                Count = u.ProductsBought.Count().ToString()
            }).ToArray();


            serializer.Serialize(writer, usersWithSoldProducts, namespaces);
            return sb.ToString().TrimEnd();
        }

        private static void DatabaseReset(ProductShopContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            System.Console.WriteLine("Db reseted!");
        }
    }
}