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



        /// <summary>
        /// Получает конкретную книгу по идентификатору (GET)
        /// </summary>
        /// <param name="id">Идентификатор книги</param>
        /// <returns>Возвращает объет книги</returns>
        [ResponseType(typeof(Books))]
        public async Task<IHttpActionResult> GetBooks(int id)
        {
            var books = await _db.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }

            return Ok(books);
        }



        /// <summary>
        /// Метод сохраняет значения в БД по опред. книги (PUT)
        /// </summary>
        /// <param name="id">Идентификатор книги</param>
        /// <param name="books">Объект класса Books</param>
        /// <returns>Сохраняет данные в БД</returns>
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
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }



        /// <summary>
        /// Метод добавляет в БД запись с данными (POST)
        /// </summary>
        /// <param name="books">Объект класса Books</param>
        /// <returns>Возвращает результат запроса</returns>
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



        /// <summary>
        /// Метод удаляет из БД запись (DELETE)
        /// </summary>
        /// <param name="id">Идентификатор книги</param>
        /// <returns>Возвращает результат запроса</returns>
        [ResponseType(typeof(Books))]
        public async Task<IHttpActionResult> DeleteBooks(int id)
        {
            var books = await _db.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }

            _db.Books.Remove(books);
            await _db.SaveChangesAsync();

            return Ok(books);
        }



        /// <summary>
        /// Метод освобождения контекста БД
        /// </summary>
        /// <param name="disposing">Признак</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Метод для получения кол-во книг в БД
        /// </summary>
        /// <param name="id">Идентификатор книги</param>
        /// <returns>Возвращает True если книги больше одной штуки</returns>
        private bool BooksExists(int id)
        {
            return _db.Books.Count(e => e.Id == id) > 0;
        }
    }
}