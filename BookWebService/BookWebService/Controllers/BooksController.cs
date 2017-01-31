using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BookWebService.Models;

namespace BookWebService.Controllers
{
    /// <summary>
    /// Контроллер управление книгами
    /// </summary>
    public class BooksController : ApiController
    {
        private readonly BooksLibraryEntities _db = new BooksLibraryEntities();

        /// <summary>
        /// Получает все книги (GET)
        /// </summary>
        /// <returns>Массив класса books</returns>
        public IQueryable<Books> GetBooks()
        {
            return _db.Books;
        }

        // GET: api/Books/5
        [ResponseType(typeof(Books))]
        public async Task<IHttpActionResult> GetBooks(int id)
        {
            Books books = await _db.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }

            return Ok(books);
        }

        // PUT: api/Books/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBooks(int id, Books books)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != books.Id)
            {
                return BadRequest();
            }

            _db.Entry(books).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Books
        [ResponseType(typeof(Books))]
        public async Task<IHttpActionResult> PostBooks(Books books)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Books.Add(books);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = books.Id }, books);
        }

        // DELETE: api/Books/5
        [ResponseType(typeof(Books))]
        public async Task<IHttpActionResult> DeleteBooks(int id)
        {
            Books books = await _db.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }

            _db.Books.Remove(books);
            await _db.SaveChangesAsync();

            return Ok(books);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BooksExists(int id)
        {
            return _db.Books.Count(e => e.Id == id) > 0;
        }
    }
}