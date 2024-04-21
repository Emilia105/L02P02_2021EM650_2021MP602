using Microsoft.AspNetCore.Mvc;

namespace L02P02_2021EM650_2021MP602.Controllers
{
    public class PedidosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
