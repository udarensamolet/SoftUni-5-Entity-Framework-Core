namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using System.Globalization;
    using System.Text;
    using Z.EntityFramework.Plus;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            // Problem 02: Age Restriction
            string command2 = Console.ReadLine().ToLower();
            Console.WriteLine(GetBooksByAgeRestriction(db, command2));

            // Problem 03: Golden Books
            Console.WriteLine(GetGoldenBooks(db));

            // Problem 04: Books by Price
            Console.WriteLine(GetBooksByPrice(db));

            // Problem 05: Not Released In 
            int command5 = int.Parse(Console.ReadLine());
            Console.WriteLine(GetBooksNotReleasedIn(db, command5));

            //Problem 06: Book Titles by Category
            string command6 = Console.ReadLine();
            Console.WriteLine(GetBooksByCategory(db, command6));

            // Problem 07: Released Before Date
            string command7 = Console.ReadLine();
            Console.WriteLine(GetBooksReleasedBefore(db, command7));

            // Problem 08: Author Search
            string command8 = Console.ReadLine();
            Console.WriteLine(GetAuthorNamesEndingIn(db, command8));

            // Problem 09: Book Search
            string command9 = Console.ReadLine();
            Console.WriteLine(GetBookTitlesContaining(db, command9));

            // Problem 10: Book Search by Author
            string command10 = Console.ReadLine();
            Console.WriteLine(GetBooksByAuthor(db, command10));

            // Problem 11: Count Books
            int command11 = int.Parse(Console.ReadLine());
            Console.WriteLine(CountBooks(db, command11));

            // Problem 12: Total Book Copies
            Console.WriteLine(CountCopiesByAuthor(db));

            // Problem 13: Profit by Category
            Console.WriteLine(GetTotalProfitByCategory(db));

            // Problem 14: Most Recent Books
            Console.WriteLine(GetMostRecentBooks(db));

            // Problem 15: Increase Prices
            IncreasePrices(db);

            // Problem 16: Remove Books
            Console.WriteLine(RemoveBooks(db));
        }

        // Problem 02: Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);
            var books = context
                .Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .OrderBy(b => b.Title)
                .AsNoTracking()
                .ToList();

            var sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }
            return sb.ToString().Trim();
        }

        // Problem 03: Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context
                .Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .AsNoTracking()
                .ToList();

            return string.Join(Environment.NewLine, goldenBooks);
        }

        // Problem 04: Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context
                .Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .AsNoTracking()
                .ToList();

            var sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }
            return sb.ToString().Trim();
        }

        // Problem 05: Not Released In 
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context
                .Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        //Problem 06: Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            List<string> categories = input
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var books = context
                .Books
                .Where(b => b.BookCategories.Any(b => categories.Contains(b.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        // Problem 07: Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var books = context
                .Books
                .Where(b => b.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .Select(b => new
                {
                    BookTitle = b.Title,
                    BookEditionType = b.EditionType.ToString(),
                    BookPrice = b.Price,
                    BookReleaseDate = b.ReleaseDate    
                })
                .OrderByDescending(b => b.BookReleaseDate)
                .ToList();

            var sb = new StringBuilder();
            foreach (var book in books) 
            {
                sb.AppendLine($"{book.BookTitle} - {book.BookEditionType} - ${book.BookPrice:f2}");
            }
            return sb.ToString().Trim();
        }

        // Problem 08: Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context
                .Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    AuthorFullName = a.FirstName + " " + a.LastName,
                })
                .OrderBy(a => a.AuthorFullName)
                .ToList();

            var sb = new StringBuilder();
            foreach(var author in authors)
            {
                sb.AppendLine(author.AuthorFullName);
            }
            return sb.ToString().Trim();
        }

        // Problem 09: Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context
                .Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        // Problem 10: Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(b => new
                {
                    BookId = b.BookId,
                    BookTitle = b.Title,
                    BookAuthorsNames = b.Author.FirstName + " " + b.Author.LastName
                })
                .OrderBy(b => b.BookId)
                .ToList();


            var sb = new StringBuilder();
            foreach(var book in books)
            {
                sb.AppendLine($"{book.BookTitle} ({book.BookAuthorsNames})");
            }
            return sb.ToString().Trim();    
        }

        // Problem 11: Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context
                .Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();

            return books;
        }

        // Problem 12: Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context
                .Authors
                .Select(a => new
                {
                    AuthorFullName = a.FirstName + " " + a.LastName,
                    AuthorBooksCount = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.AuthorBooksCount)
                .ToList();

            var sb = new StringBuilder();
            foreach (var author in authors)
            {
                sb.AppendLine($"{author.AuthorFullName} - {author.AuthorBooksCount}");
            }
            return sb.ToString().Trim();
        }

        // Problem 13: Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context
                .Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    CategoryProfit = c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
                })
                .OrderByDescending(c => c.CategoryProfit)
                .ThenBy(c => c.CategoryName)
                .ToList();

            var sb = new StringBuilder();
            foreach (var category in categories)
            {
                sb.AppendLine($"{category.CategoryName} ${category.CategoryProfit:f2}");
            }
            return sb.ToString().Trim();
        }

        // Problem 14: Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context
                .Categories
                .Select(c => new 
                {
                    CategoryName = c.Name,
                    CategoryBooks = c.CategoryBooks
                        .Select(cb => new
                        {
                            BookReleaseDate = cb.Book.ReleaseDate,
                            BookReleaseYear = cb.Book.ReleaseDate.Value.Year,
                            BookTitle = cb.Book.Title
                        })
                        .OrderByDescending(cb => cb.BookReleaseDate)
                        .Take(3)
                        .ToList()
                })
                .OrderBy(c => c.CategoryName)
                .ToList();

            var sb = new StringBuilder();
            foreach(var category in categories)
            {
                sb.AppendLine($"--{category.CategoryName}");
                foreach(var book in category.CategoryBooks)
                {
                    sb.AppendLine($"{book.BookTitle} ({book.BookReleaseYear})");
                }
            }
            return sb.ToString().Trim();
        }

        // Problem 15: Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            /*var books = context
                .Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .Update(b => new Book { Price = 5 + b.Price });

            Console.WriteLine(books);*/
            int yearLessThan = 2010;

            IQueryable<Book> booksToUpdate = context.Books
                .Where(x => x.ReleaseDate.Value.Year < yearLessThan);

            int priceToIncrease = 5;
            foreach (Book book in booksToUpdate)
            {
                book.Price += priceToIncrease;
            }

            context.SaveChanges();
        }

        // Problem 16: Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var booksDeleted = context
                .Books
                .Where(b => b.Copies < 4200)
                .Delete();

            return booksDeleted;
        }
    }
}