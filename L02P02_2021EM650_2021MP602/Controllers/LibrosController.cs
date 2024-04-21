using L02P02_2021EM650_2021MP602.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

            string nombreAutor = "PEPE";
            var libros = (from l in _librosContext.Libros
                          join a in _librosContext.Autores on l.IdAutor equals a.Id
                          
                          select new
                          {
                              id = l.Id,
                              nombre = l.Nombre
                          }).ToList();

            ViewData["autor"] = nombreAutor;
            ViewData["libros"] = libros;
            return View();

        }
    }
}
