using L02P02_2021EM650_2021MP602.Models;
using Microsoft.AspNetCore.Mvc;

namespace L02P02_2021EM650_2021MP602.Controllers
{
    public class LibrosController : Controller
    {
        private readonly LibreriaDbContext _librosContext;

        public LibrosController(LibreriaDbContext context)
        {
            _librosContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
