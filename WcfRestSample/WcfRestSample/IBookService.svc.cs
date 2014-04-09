using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfRestSample
{   
    public class BookService : IBookService
    {
        public List<Book> GetBooksList()
        {
            using (SampleDbEntities entities = new SampleDbEntities())
            {
                return entities.Books.ToList();
            }
        }

        public Book GetBookById(string id)
        {
            try
            {
                int bookId = Convert.ToInt32(id);

                using (SampleDbEntities entities = new SampleDbEntities())
                {
                    return entities.Books.SingleOrDefault(book => book.ID == bookId);
                }
            }
            catch
            {
                throw new FaultException("Something went wrong");
            }
        }

        public void AddBook(string name)
        {
            using (SampleDbEntities entities = new SampleDbEntities())
            {
                Book book = new Book { BookName = name };
                entities.Books.AddObject(book);
                entities.SaveChanges();
            }
        }

        public void UpdateBook(string id, string name)
        {
            try
            {
                int bookId = Convert.ToInt32(id);

                using (SampleDbEntities entities = new SampleDbEntities())
                {
                    Book book = entities.Books.SingleOrDefault(b => b.ID == bookId);
                    book.BookName = name;
                    entities.SaveChanges();
                }
            }
            catch
            {
                throw new FaultException("Something went wrong");
            }
        }

        public void DeleteBook(string id)
        {
            try
            {
                int bookId = Convert.ToInt32(id);

                using (SampleDbEntities entities = new SampleDbEntities())
                {
                    Book book = entities.Books.SingleOrDefault(b => b.ID == bookId);
                    entities.Books.DeleteObject(book);
                    entities.SaveChanges();
                }
            }
            catch
            {
                throw new FaultException("Something went wrong");
            }
        }

        public List<string> GetBooksNames()
        {
            using (SampleDbEntities entities = new SampleDbEntities())
            {
                return entities.Books.Select(book => book.BookName).ToList();
            }
        }
    }
}
