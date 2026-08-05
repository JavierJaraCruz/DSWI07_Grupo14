
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



        public int CrearProducto(Producto p) => _productoDAL.Insertar(p);



        public Producto ObtenerProducto(int id) => _productoDAL.ObtenerPorId(id);



        public List<Producto> ListarProductos() => _productoDAL.Listar();



        public void ActualizarProducto(Producto p) => _productoDAL.Actualizar(p);



        public void EliminarProducto(int id) => _productoDAL.Eliminar(id);



        public void ActualizarStock(int productoId, int cantidad, string tipoMovimiento, string referencia)

          => _productoDAL.ActualizarStock(productoId, cantidad, tipoMovimiento, referencia);

    }

}
