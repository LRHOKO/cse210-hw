using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement
{
    public abstract class LibraryItem
    {
        public string Title { get; }
        public string Author { get; }
        public bool IsAvailable { get; private set; } = true;

        protected LibraryItem(string title, string author)
        {
            Title = title;
            Author = author;
        }

        public void MarkBorrowed() => IsAvailable = false;
        public void MarkReturned() => IsAvailable = true;

        public abstract void Display();
    }

    public class RegularBook : LibraryItem
    {
        public RegularBook(string title, string author) : base(title, author) { }

        public override void Display()
        {
            Console.WriteLine($"[Book] {Title} by {Author} - {(IsAvailable ? "Available" : "Checked Out")}");
        }
    }

    public class PeriodicalBook : LibraryItem
    {
        public DateTime IssueDate { get; }

        public PeriodicalBook(string title, string author, DateTime issueDate)
            : base(title, author)
        {
            IssueDate = issueDate;
        }

        public override void Display()
        {
            Console.WriteLine($"[Periodical] {Title} by {Author} ({IssueDate:yyyy-MM-dd}) - {(IsAvailable ? "Available" : "Checked Out")}");
        }
    }

    public class User
    {
        public string Name { get; }
        public int Id { get; }
        private readonly List<LibraryItem> _borrowedItems = new();

        public User(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public void Borrow(LibraryItem item)
        {
            if (!item.IsAvailable)
            {
                Console.WriteLine($"'{item.Title}' is currently unavailable.");
                return;
            }

            _borrowedItems.Add(item);
            item.MarkBorrowed();
            Console.WriteLine($"{Name} borrowed '{item.Title}'.");
        }

        public void Return(LibraryItem item)
        {
            if (!_borrowedItems.Contains(item))
            {
                Console.WriteLine($"{Name} has not borrowed '{item.Title}'.");
                return;
            }

            _borrowedItems.Remove(item);
            item.MarkReturned();
            Console.WriteLine($"{Name} returned '{item.Title}'.");
        }

        public void ShowBorrowedItems()
        {
            if (!_borrowedItems.Any())
            {
                Console.WriteLine($"{Name} has not borrowed any items.");
                return;
            }

            Console.WriteLine($"{Name}'s Borrowed Items:");
            foreach (var item in _borrowedItems)
                Console.WriteLine($"- {item.Title}");
        }
    }

    public class Transaction
    {
        public User User { get; }
        public LibraryItem Item { get; }
        public DateTime Timestamp { get; }
        public string Action { get; }

        public Transaction(User user, LibraryItem item, string action)
        {
            User = user;
            Item = item;
            Action = action;
            Timestamp = DateTime.Now;
        }

        public void Print()
        {
            Console.WriteLine($"{User.Name} {Action} '{Item.Title}' on {Timestamp:yyyy-MM-dd HH:mm}");
        }
    }

    public class TxLog
    {
        private readonly List<Transaction> _transactions = new();

        public void Record(Transaction tx) => _transactions.Add(tx);

        public void PrintAll()
        {
            if (!_transactions.Any())
            {
                Console.WriteLine("No transactions recorded.");
                return;
            }

            Console.WriteLine("Transaction History:");
            foreach (var tx in _transactions)
                tx.Print();
        }
    }

    public class UserService
    {
        private readonly List<User> _users = new();

        public User Register(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name can't be empty.");

            int id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            var user = new User(name, id);
            _users.Add(user);
            Console.WriteLine($"Registered: {name} (ID: {id})");
            return user;
        }

        public User FindByName(string name) =>
            _users.FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public class LibManager
    {
        private readonly List<LibraryItem> _items = new();

        public void AddItem(LibraryItem item)
        {
            _items.Add(item);
            Console.WriteLine($"Added: '{item.Title}'");
        }

        public void ShowAllItems()
        {
            if (!_items.Any())
            {
                Console.WriteLine("No books in library.");
                return;
            }

            Console.WriteLine("Library Collection:");
            foreach (var item in _items)
                item.Display();
        }

        public List<LibraryItem> Search(string keyword) =>
            _items.Where(i =>
                i.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                i.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

        public LibraryItem FindByTitle(string title) =>
            _items.FirstOrDefault(i => i.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }

    public class LibraryApp
    {
        private readonly UserService _userService = new();
        private readonly LibManager _libManager = new();
        private readonly TxLog _txLog = new();

        public void Run()
        {
            SeedInitialData();

            while (true)
            {
                ShowMenu();
                Console.Write("Choose an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1": RegisterUser(); break;
                    case "2": AddBook(); break;
                    case "3": SearchBooks(); break;
                    case "4": BorrowBook(); break;
                    case "5": ReturnBook(); break;
                    case "6": ShowBorrowedBooks(); break;
                    case "7": _txLog.PrintAll(); break;
                    case "8": _libManager.ShowAllItems(); break;
                    case "9": Console.WriteLine("Exiting..."); return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("\n--- Library Menu ---");
            string[] options = {
                "Register User",
                "Add Book",
                "Search Books",
                "Borrow Book",
                "Return Book",
                "Show Borrowed Books",
                "View Transactions",
                "View All Books",
                "Exit"
            };

            for (int i = 0; i < options.Length; i++)
                Console.WriteLine($"{i + 1}. {options[i]}");
        }

        private void RegisterUser()
        {
            Console.Write("Enter your name: ");
            var name = Console.ReadLine();
            try
            {
                _userService.Register(name);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AddBook()
        {
            Console.Write("Title: ");
            var title = Console.ReadLine();

            Console.Write("Author: ");
            var author = Console.ReadLine();

            Console.Write("Type (regular / periodical): ");
            var type = Console.ReadLine()?.Trim().ToLower();

            switch (type)
            {
                case "regular":
                    _libManager.AddItem(new RegularBook(title, author));
                    break;
                case "periodical":
                    Console.Write("Issue Date (YYYY-MM-DD): ");
                    if (DateTime.TryParse(Console.ReadLine(), out var date))
                        _libManager.AddItem(new PeriodicalBook(title, author, date));
                    else
                        Console.WriteLine("Invalid date format.");
                    break;
                default:
                    Console.WriteLine("Unknown book type.");
                    break;
            }
        }

        private void SearchBooks()
        {
            Console.Write("Search keyword: ");
            var keyword = Console.ReadLine();

            var results = _libManager.Search(keyword);
            if (!results.Any())
            {
                Console.WriteLine("No results found.");
                return;
            }

            Console.WriteLine("Search Results:");
            foreach (var item in results)
                item.Display();
        }

        private void BorrowBook()
        {
            Console.Write("Enter your name: ");
            var user = _userService.FindByName(Console.ReadLine());
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            Console.Write("Enter book title: ");
            var item = _libManager.FindByTitle(Console.ReadLine());
            if (item == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            user.Borrow(item);
            _txLog.Record(new Transaction(user, item, "borrowed"));
        }

        private void ReturnBook()
        {
            Console.Write("Enter your name: ");
            var user = _userService.FindByName(Console.ReadLine());
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            Console.Write("Enter book title: ");
            var item = _libManager.FindByTitle(Console.ReadLine());
            if (item == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            user.Return(item);
            _txLog.Record(new Transaction(user, item, "returned"));
        }

        private void ShowBorrowedBooks()
        {
            Console.Write("Enter your name: ");
            var user = _userService.FindByName(Console.ReadLine());
            if (user != null)
                user.ShowBorrowedItems();
            else
                Console.WriteLine("User not found.");
        }

        private void SeedInitialData()
        {
            _libManager.AddItem(new RegularBook("1984", "George Orwell"));
            _libManager.AddItem(new PeriodicalBook("Scientific American", "Various", new DateTime(2024, 10, 1)));
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            new LibraryApp().Run();
        }
    }
}