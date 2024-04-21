using L02P02_2021EM650_2021MP602.Models;
using Microsoft.AspNetCore.Mvc;

namespace L02P02_2021EM650_2021MP602.Controllers
{
    public class PedidosController : Controller
    {
        private readonly LibreriaDbContext _librosContext;

        public PedidosController(LibreriaDbContext context)
        {
            _librosContext = context;
        }

        public IActionResult Index(int id, Boolean? ventaCerrada = false)
        {
            Cliente? client = (from pd in _librosContext.PedidoDetalles
                          join p in _librosContext.PedidoEncabezados on pd.IdPedido equals p.Id
                          join c in _librosContext.Clientes on p.IdCliente equals c.Id
                          where p.Id == id
                          select c).FirstOrDefault();

            var librosAgregados = (from p in _librosContext.PedidoDetalles
                                   join l in _librosContext.Libros on p.IdLibro equals l.Id
                                   join a in _librosContext.Autores on l.IdAutor equals a.Id
                                   where p.IdPedido == id
                                   select new
                                   {
                                       nombre = l.Nombre,
                                       autor = a.Autor,
                                       precio = l.Precio
                                   }).ToList();

            decimal totalUSD = _librosContext.PedidoEncabezados.FirstOrDefault(pe => pe.Id == id)?.Total ?? 0;

            ViewData["cliente"] = client;
            ViewData["libros"] = librosAgregados;
            ViewData["total_precio"] = totalUSD;
            ViewData["total"] = librosAgregados.Count();
            ViewData["id_pedido"] = id;
            ViewData["venta_cerrada"] = ventaCerrada;
            return View();
        }

        public IActionResult CerrarVenta (CompletarPedidoModel model)
        {
            PedidoEncabezado? pedido = _librosContext.PedidoEncabezados.SingleOrDefault(pe => pe.Id == model.id_pedido);
            if (pedido != null)
            {
                pedido.Estado = "C";
                _librosContext.SaveChanges();
            }

            return RedirectToAction("Index", new { id = model.id_pedido, ventaCerrada = true });
        }
    }
}
