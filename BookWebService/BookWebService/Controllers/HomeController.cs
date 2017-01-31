using System.Web.Mvc;

namespace BookWebService.Controllers
{
    /// <summary>
    /// Домашний контроллер
    /// </summary>
    public class HomeController : Controller
    {

        /// <summary>
        /// Отображение главной страницы
        /// </summary>
        /// <returns>Возвращает вид</returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
    }
}
