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
        private readonly ProveedorDAL _proveedorDAL;

        public ProveedorService(ProveedorDAL proveedorDAL)
        {
            _proveedorDAL = proveedorDAL;
        }

        public int CrearProveedor(Proveedor p) => _proveedorDAL.Insertar(p);

        public List<Proveedor> ListarProveedores() => _proveedorDAL.Listar();

        public Proveedor ObtenerProveedor(int id) => _proveedorDAL.ObtenerProveedor((int)id);

        public void ActualizarProveedor(Proveedor p) => _proveedorDAL.Actualizar(p);

        public void EliminarProveedor(int id) => _proveedorDAL.Eliminar(id);
    }
}
