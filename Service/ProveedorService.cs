using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProveedorService
    {
        private readonly ProveedorDAL proveedorDAL = new ProveedorDAL();

        public int CrearProveedor(Proveedor p) => proveedorDAL.Insertar(p);

        public List<Proveedor> ListarProveedores() => proveedorDAL.Listar();

        public Proveedor ObtenerProveedor(int id) => proveedorDAL.ObtenerProveedor((int)id);

        public void ActualizarProveedor(Proveedor p) => proveedorDAL.Actualizar(p);

        public void EliminarProveedor(int id) => proveedorDAL.Eliminar(id);
    }
}
