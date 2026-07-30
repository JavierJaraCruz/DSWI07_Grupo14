using Dal;
using Entities;

namespace Services;

public class CarritoService
{
    private readonly CarritoDAL _carritoDAL;

    public CarritoService(CarritoDAL carritoDAL)
    {
        _carritoDAL = carritoDAL;
    }


    public Carrito? ObtenerPorUsuario(int usuarioId)
        => _carritoDAL.ObtenerPorUsuario(usuarioId);


    public Carrito? ObtenerCarrito(int id)
        => _carritoDAL.ObtenerCarrito(id);


    public int CrearCarrito(int usuarioId)
        => _carritoDAL.CrearCarrito(usuarioId);


    public void AgregarProducto(int carritoId, int productoId, int cantidad, decimal precioUnitario)
        => _carritoDAL.AgregarProducto(carritoId, productoId, cantidad, precioUnitario);


    public void EliminarProducto(int detalleId)
        => _carritoDAL.EliminarProducto(detalleId);


    public void EliminarCarrito(int id)
        => _carritoDAL.Eliminar(id);


    public List<CarritoDetalle> ObtenerDetalles(int carritoId)
        => _carritoDAL.ObtenerDetalles(carritoId);


    public void VaciarCarrito(int carritoId)
        => _carritoDAL.VaciarCarrito(carritoId);
}