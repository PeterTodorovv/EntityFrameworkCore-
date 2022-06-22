namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //int length = int.Parse(Console.ReadLine());

            Console.WriteLine(RemoveBooks(db));

            //IncreasePrices(db);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books.Select(b => new
            {
                b.AgeRestriction
                ,b.Title
            })
                .Where(b => ageRestriction == b.AgeRestriction)
                .OrderBy(b => b.Title).ToArray();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {

            var books = context.Books.Select(b => new 
            {
                b.Title,
                b.Copies,
                b.BookId,
                b.EditionType
            })
                .Where(b => b.Copies < 5000 && b.EditionType == EditionType.Gold)
                .OrderBy(b => b.BookId).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books.Select(b => new
            {
                b.Title,
                b.Price
            }
            ).Where(b => b.Price > 40)
            .OrderByDescending(b => b.Price).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books.Select(b => new
            {
                b.Title,
                b.ReleaseDate,
                b.BookId
            }).ToArray().Where(b => DateTime.Parse(b.ReleaseDate.ToString()).Year != year)
            .OrderBy(b => b.BookId).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(" ");

            var books = context.Books
                .Where(b => categories.Any(c => c == b.BookCategories.Select(bc => bc.Category.Name).First()))
                .OrderBy(b => b.Title).Select(b => b.Title).ToArray();

            StringBuilder sb = new StringBuilder();


            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dateToDatetime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books.Where(b => b.ReleaseDate < dateToDatetime)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new 
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                }).ToArray();

            StringBuilder sb = new StringBuilder();


            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            Regex regex = new Regex(@".*" + input +@"{1}$"); 
            var authors = context.Authors.Select(a => new
            {
                a.FirstName,
                a.LastName
            }).ToArray().Where(a => regex.IsMatch(a.FirstName))
            .OrderBy( a => a.FirstName ).ThenBy(a => a.LastName)
                .ToArray();

            StringBuilder sb = new StringBuilder();


            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            Regex regex = new Regex(@".*" + input.ToLower() + @".*");
            var books = context.Books.Select(b => b.Title).ToArray()
                .Where(b => regex.IsMatch(b.ToLower()))
                .OrderBy(b => b).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var title in books)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            Regex regex = new Regex(@"^" + input.ToLower() + @".*");
            var books = context.Books.Select(b => new
            {
                b.BookId,
                b.Title,
                AuthorFirstName = b.Author.FirstName,
                AuthorLastName = b.Author.LastName
            }).ToArray()
                .Where(b => regex.IsMatch(b.AuthorLastName.ToLower()))
                .OrderBy(b => b.BookId).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorFirstName} {book.AuthorLastName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books.Where(b => b.Title.Length > lengthCheck)
                .Select(b => b.BookId).ToArray();

            return books.Length;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors.Select(a => new 
            {
                FullName = a.FirstName + " " + a.LastName,
                TotalCopiesSold = a.Books.Sum(b => b.Copies)
            }).OrderByDescending(a => a.TotalCopiesSold);

            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FullName} - {author.TotalCopiesSold}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories.Select(c => new
            {
                c.Name,
                TotalProfit = c.CategoryBooks.Sum(b => b.Book.Copies * b.Book.Price)
            }).OrderByDescending(c => c.TotalProfit).ThenBy(c => c.Name).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var categorie in categories)
            {
                sb.AppendLine($"{categorie.Name} ${categorie.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories.OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    LatestBooks = c.CategoryBooks.Select(cb => new 
                    {
                        BookTitle = cb.Book.Title,
                        ReleaseDate = cb.Book.ReleaseDate
                    }).OrderByDescending( b => b.ReleaseDate).Take(3)
                }).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var categorie in categories)
            {
                sb.AppendLine($"--{categorie.Name}");

                foreach (var book in categorie.LatestBooks)
                {
                    sb.AppendLine($"{book.BookTitle} ({book.ReleaseDate:yyyy})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books.Where(b => b.ReleaseDate < DateTime.Parse("2010-01-01"));

            foreach(var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            IQueryable<Book> books = context.Books.Where(b => b.Copies < 4200);

            int removedBooks = books.Count();
            context.Books.BulkDelete(books);

            context.SaveChanges();
            return removedBooks;
        }
    }
}
