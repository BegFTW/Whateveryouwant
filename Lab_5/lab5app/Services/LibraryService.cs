using Lab_5.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab_5.Services
{
    public class LibraryService : ILibraryService
    {
        public List<Book> books;
        public List<User> users;
        public Dictionary<User, List<Book>> borrowedBooks = new Dictionary<User, List<Book>>();

        public LibraryService()
        {
            books = LoadBooksFromCsv();
            users = LoadUsersFromCsv();
        }

        public List<Book> GetBooks() => books;
        public List<User> GetUsers() => users;
        public Dictionary<User, List<Book>> GetBorrowedBooks() => borrowedBooks;

        public void AddBook(Book book)
        {
            book.Id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
            books.Add(book);
            SaveBooksToCsv();
        }

        public void EditBook(Book book)
        {
            var existing = books.FirstOrDefault(b => b.Id == book.Id);
            if (existing != null)
            {
                existing.Title = book.Title;
                existing.Author = book.Author;
                existing.ISBN = book.ISBN;
                SaveBooksToCsv();
            }
        }

        public void DeleteBook(int bookId)
        {
            books.RemoveAll(b => b.Id == bookId);
            SaveBooksToCsv();
        }

        public void AddUser(User user)
        {
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);
            SaveUsersToCsv();
        }

        public void EditUser(User user)
        {
            var existing = users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                existing.Name = user.Name;
                existing.Email = user.Email;
                SaveUsersToCsv();
            }
        }

        public void DeleteUser(int userId)
        {
            users.RemoveAll(u => u.Id == userId);
            SaveUsersToCsv();
        }

        public void BorrowBook(int userId, int bookId)
        {
            User user = users.FirstOrDefault(u => u.Id == userId);
            Book book = books.FirstOrDefault(b => b.Id == bookId);

            if (user != null && book != null)
            {
                if (!borrowedBooks.ContainsKey(user))
                    borrowedBooks[user] = new List<Book>();

                borrowedBooks[user].Add(book);
                books.Remove(book);
                SaveBooksToCsv();
            }
        }

        public void ReturnBook(int userId, int bookId)
        {
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user != null && borrowedBooks.ContainsKey(user))
            {
                var book = borrowedBooks[user].FirstOrDefault(b => b.Id == bookId);
                if (book != null)
                {
                    borrowedBooks[user].Remove(book);
                    books.Add(book);
                    SaveBooksToCsv();
                }
            }
        }

        public List<Book> LoadBooksFromCsv()
        {
            var bookList = new List<Book>();
            if (File.Exists("./Data/Books.csv"))
            {
                foreach (var line in File.ReadLines("Data\\Books.csv"))
                {
                    var fields = line.Split(',');
                    if (fields.Length >= 4)
                    {
                        bookList.Add(new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = fields[1].Trim(),
                            Author = fields[2].Trim(),
                            ISBN = fields[3].Trim()
                        });
                    }
                }
            }
            return bookList;
        }

        public void SaveBooksToCsv()
        {
            using (var writer = new StreamWriter("Data\\Books.csv"))
            {
                foreach (var book in books)
                {
                    writer.WriteLine($"{book.Id},{book.Title},{book.Author},{book.ISBN}");
                }
            }
        }

        public List<User> LoadUsersFromCsv()
        {
            var userList = new List<User>();
            if (File.Exists("Data\\Users.csv"))
            {
                foreach (var line in File.ReadLines("Data\\Users.csv"))
                {
                    var fields = line.Split(',');
                    if (fields.Length >= 3)
                    {
                        userList.Add(new User
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Name = fields[1].Trim(),
                            Email = fields[2].Trim()
                        });
                    }
                }
            }
            return userList;
        }

        async void SaveUsersToCsv()
        {
            using (var writer = new StreamWriter("Data\\Users.csv"))
                foreach (User user in users)
                {
                    string str = $"{user.Id.ToString()}, {user.Name}, {user.Email}";
                    await writer.WriteLineAsync(str);
                }
            
        }
    }
}