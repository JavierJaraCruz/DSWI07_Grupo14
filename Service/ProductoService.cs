
using DAL;
using Entities;
using System.Collections.Generic;



namespace Services

{

    public class ProductoService

    {

        private readonly ProductoDAL _productoDAL;

        public ProductoService(ProductoDAL productoDAL)
        {
            _productoDAL = productoDAL;
        }



        public async Task<int> CrearProductoAsync(Producto p)
            => await _productoDAL.InsertarAsync(p);

        public async Task<Producto> ObtenerProductoAsync(int id)
            => await _productoDAL.ObtenerPorIdAsync(id);

        public async Task<List<Producto>> ListarProductosAsync()
            => await _productoDAL.ListarAsync();

        public async Task<List<Producto>> ListarTodosLosProductosAsync()
            => await _productoDAL.ListarTodosAsync();

        public async Task ActualizarProductoAsync(Producto p)
            => await _productoDAL.ActualizarAsync(p);

        public async Task EliminarProductoAsync(int id)
            => await _productoDAL.EliminarAsync(id);

        public async Task ActualizarStockAsync(
            int productoId,
            int cantidad,
            string tipoMovimiento,
            string referencia)
            => await _productoDAL.ActualizarStockAsync(
                productoId,
                cantidad,
                tipoMovimiento,
                referencia);

        public async Task<Producto> ObtenerProductoAdminAsync(int id)
            => await _productoDAL.ObtenerPorIdAdminAsync(id);

    }

}
