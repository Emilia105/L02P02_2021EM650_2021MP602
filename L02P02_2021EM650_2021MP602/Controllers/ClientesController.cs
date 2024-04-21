using L02P02_2021EM650_2021MP602.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L02P02_2021EM650_2021MP602.Controllers
{
    public class ClientesController : Controller
    {
        private readonly LibreriaDbContext _clientesContext;

        public ClientesController(LibreriaDbContext context)
        {
            _clientesContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AgregarCliente(Cliente nuevoCliente) {
            _clientesContext.Add(nuevoCliente);
            _clientesContext.SaveChanges();

            PedidoEncabezado pedidoEncabezado = new PedidoEncabezado
            {
                IdCliente = nuevoCliente.Id,
                CantidadLibros = 0,
                Estado = "P",
                Total = 0
            };
            _clientesContext.PedidoEncabezados.Add(pedidoEncabezado);
            _clientesContext.SaveChanges();

            return RedirectToAction("Index" , "Libros");
        }

    }
}
