using Dal;
using Microsoft.AspNetCore.Mvc;

namespace Burgos0._2.Controllers
{
    public class TestController : Controller
    {
        private readonly ConexionBD _bd;

        public TestController(ConexionBD bd)
        {
            _bd = bd;
        }

        public IActionResult Conexion()
        {
            _bd.ProbarConexion(out string mensaje);
             

            return Content(mensaje);
        }
    }
}
