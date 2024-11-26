using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_5.Data;

namespace Lab_5.Services
{
    public interface ILibraryService
    {
        public static List<Book> books;
        public static List<User> users;
        public static Dictionary<User, List<Book>> borrowedBooks;
        List<Book> GetBooks();
        List<User> GetUsers();
        Dictionary<User, List<Book>> GetBorrowedBooks();
        void AddBook(Book book);
        void EditBook(Book book);
        void DeleteBook(int bookId);
        void AddUser(User user);
        void EditUser(User user);
        void DeleteUser(int userId);
        void BorrowBook(int userId, int bookId);
        void ReturnBook(int userId, int bookId);
    }
}