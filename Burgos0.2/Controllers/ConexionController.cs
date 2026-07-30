using Microsoft.AspNetCore.Mvc;
using Dal;

namespace TiendaWeb.Controllers;

public class ConexionController : Controller
{
    private readonly ConexionBD _bd;

    // La conexión llega por inyección de dependencias (registrada en Program.cs)
    public ConexionController(ConexionBD bd)
    {
        _bd = bd;
    }

    // GET: /Conexion
    public IActionResult Index()
    {
        bool ok = _bd.ProbarConexion(out string mensaje);

        ViewBag.Exito = ok;
        ViewBag.Mensaje = mensaje;

        if (ok)
        {
            try { ViewBag.Total = _bd.ContarProductos(); }
            catch (Exception ex) { ViewBag.Mensaje += $" | Pero falló la consulta: {ex.Message}"; }
        }

        return View();
    }
}
