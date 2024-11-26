using Lab_5.Data;
using Lab_5.Services;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddBook_ShouldAddBookToList()
        {
            var service = new LibraryService();
            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "12345"
            };

            service.AddBook(book);
            var books = service.GetBooks();

            Assert.IsTrue(books.Contains(book));
        }
        [TestMethod]

        public void EditBook_ShouldUpdateBookDetails()
        {
            LibraryService service = new LibraryService();
            int id = service.books.Count + 1;
            Book book = new Book { Id = id, Title = "Old Title", Author = "Old Author", ISBN = "12345" };
            service.AddBook(book);

            Book updatedBook = new Book { Id = id, Title = "New Title", Author = "New Author", ISBN = "54321" };

            service.EditBook(updatedBook);
            var books = service.GetBooks();

            Book editedBook = books.FirstOrDefault(b => b.Id == id);
            Assert.IsTrue(editedBook.Title == "New Title" || editedBook.Author == "New Author");
        }
        [TestMethod]

        public void DeleteBook_ShouldRemoveBookFromList()
        {
            var service = new LibraryService();
            var book = new Book { Id = 1, Title = "Test Book", Author = "Test Author", ISBN = "12345" };
            service.AddBook(book);

            service.DeleteBook(1);
            var books = service.GetBooks();

            if (books.Exists(b => b.Id == 1))
            {
                throw new Exception("Book was not removed from the list.");
            }

            Console.WriteLine("DeleteBook_ShouldRemoveBookFromList passed.");
        }
        [TestMethod]

        public void BorrowBook_ShouldAddBookToUserBorrowedBooks()
        {
            var service = new LibraryService();
            var user = new User { Id = 1, Name = "Test User", Email = "test@example.com" };
            var book = new Book { Id = 1, Title = "Test Book", Author = "Test Author", ISBN = "12345" };
            service.AddUser(user);
            service.AddBook(book);

            service.BorrowBook(user.Id, book.Id);

            var borrowedBooks = service.GetBorrowedBooks();
            if (!borrowedBooks[user]?.Contains(book) == true)
            {
                throw new Exception("Book was not added to the borrowed books list.");
            }

            Console.WriteLine("BorrowBook_ShouldAddBookToUserBorrowedBooks passed.");
        }
        [TestMethod]

        public void ReturnBook_ShouldRemoveBookFromUserBorrowedBooks()
        {
            var service = new LibraryService();
            var user = new User { Id = 1, Name = "Test User", Email = "test@example.com" };
            var book = new Book { Id = 1, Title = "Test Book", Author = "Test Author", ISBN = "12345" };
            service.AddUser(user);
            service.AddBook(book);
            service.BorrowBook(user.Id, book.Id);

            service.ReturnBook(user.Id, book.Id);

            var borrowedBooks = service.GetBorrowedBooks();
            if (borrowedBooks[user]?.Contains(book) == true)
            {
                throw new Exception("Book was not removed from the borrowed books list.");
            }

            Console.WriteLine("ReturnBook_ShouldRemoveBookFromUserBorrowedBooks passed.");
        }

    }
}