
using DAL;
using Entities;
using System.Collections.Generic;



namespace Services

{

    public class ProductoService

    {

        private readonly ProductoDAL productoDAL = new ProductoDAL();



        public int CrearProducto(Producto p) => productoDAL.Insertar(p);



        public Producto ObtenerProducto(int id) => productoDAL.ObtenerPorId(id);



        public List<Producto> ListarProductos() => productoDAL.Listar();



        public void ActualizarProducto(Producto p) => productoDAL.Actualizar(p);



        public void EliminarProducto(int id) => productoDAL.Eliminar(id);



        public void ActualizarStock(int productoId, int cantidad, string tipoMovimiento, string referencia)

          => productoDAL.ActualizarStock(productoId, cantidad, tipoMovimiento, referencia);

    }

}
