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
        public IActionResult Index(int id)
        {
            var librosA = (from p in _librosContext.PedidoDetalles
                           join l in _librosContext.Libros on p.IdLibro equals l.Id
                           join a in _librosContext.Autores on l.IdAutor equals a.Id
                           where p.IdPedido == id
                           group l by p.IdPedido into g
                           select new
                           {
                               suma = g.Sum(i => i.Precio),
                               total = g.Count()
                           }).ToList();

            var libros = (from l in _librosContext.Libros
                          join a in _librosContext.Autores on l.IdAutor equals a.Id

                          select new
                          {
                              id = l.Id,
                              nombre = l.Nombre,
                              nombreAutor = a.Autor,
                              precioL = l.Precio
                          }).ToList();

            ViewData["libros"] = libros;
            ViewData["IdPedido"] = id;
            ViewData["TotalLibros"] = librosA.Count() > 0 ? librosA[0].total : 0;
            ViewData["Total"] = librosA.Count() > 0 ? librosA[0].suma : 0;
            return View();

        }
        public IActionResult AgregarLibros(int IdPedido, int IdLibro)
        {
            // Obtener el pedido encabezado
            var pedidoEncabezado = _librosContext.PedidoEncabezados.FirstOrDefault(p => p.Id == IdPedido);
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
                IdPedido = IdPedido,
                IdLibro = IdLibro,
                CreatedAt = DateTime.Now,

            };

            _librosContext.PedidoDetalles.Add(detalle);

            decimal precioLibro = _librosContext.Libros.FirstOrDefault(l => l.Id == IdLibro)?.Precio ?? 0;
            PedidoEncabezado? pe = _librosContext.PedidoEncabezados.SingleOrDefault(p => p.Id == IdPedido);
            pe.Total += precioLibro;

            _librosContext.SaveChanges();

            return RedirectToAction("Index", "Libros", new { id = IdPedido });
        }

        public IActionResult CompletarPedido(CompletarPedidoModel model)
        {
            return RedirectToAction("Index", "Pedidos", new { id = model.id_pedido });
        }
    }

    public class CompletarPedidoModel
    {
        public int id_pedido { get; set; }
    }
}
