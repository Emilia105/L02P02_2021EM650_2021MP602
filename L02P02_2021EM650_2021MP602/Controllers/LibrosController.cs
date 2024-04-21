using L02P02_2021EM650_2021MP602.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

            string nombreAutor = "Autor";
            var libros = (from l in _librosContext.Libros
                          join a in _librosContext.Autores on l.IdAutor equals a.Id

                          select new
                          {
                              id = l.Id,
                              nombre = l.Nombre,
                              nombreAutor = a.Autor,
                              precioL = l.Precio
                          }).ToList();

            ViewData["autor"] = nombreAutor;
            ViewData["libros"] = libros;
            return View();

        }
        public IActionResult AgregarLibros(int IdPedido, int IdLibro)
        {
            // Obtener el pedido encabezado
            var pedidoEncabezado = _librosContext.PedidoEncabezados.Find(IdPedido);
            if (pedidoEncabezado == null)
            {
                return NotFound("valiendo");
            }
            // Obtener el libro
            var libro = _librosContext.Libros.Find(IdLibro);
            if (libro == null)
            {
                return NotFound("madre");
            }

            // Crear un nuevo detalle de pedido
            PedidoDetalle detalle = new PedidoDetalle
            {
                Id = IdPedido,
                IdLibro = IdLibro
            };

            _librosContext.PedidoDetalles.Add(detalle);
            _librosContext.SaveChanges();


            return View("Index");
        }
    }    
}
