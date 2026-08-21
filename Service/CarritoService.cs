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


    public async Task<Carrito?> ObtenerPorUsuario(int usuarioId)
              => await _carritoDAL.ObtenerPorUsuario(usuarioId);

    public async Task<Carrito?> ObtenerCarrito(int id)
        => await _carritoDAL.ObtenerCarrito(id);

    public async Task<int> CrearCarrito(int usuarioId)
        => await _carritoDAL.CrearCarrito(usuarioId);

    public async Task AgregarProducto(
        int carritoId,
        int productoId,
        int cantidad,
        decimal precioUnitario)
        => await _carritoDAL.AgregarProducto(
            carritoId,
            productoId,
            cantidad,
            precioUnitario);

    public async Task EliminarProducto(int detalleId)
        => await _carritoDAL.EliminarProducto(detalleId);

    public async Task EliminarCarrito(int id)
        => await _carritoDAL.Eliminar(id);

    public async Task<List<CarritoDetalle>> ObtenerDetalles(int carritoId)
        => await _carritoDAL.ObtenerDetalles(carritoId);

    public async Task VaciarCarrito(int carritoId)
        => await _carritoDAL.VaciarCarrito(carritoId);
}