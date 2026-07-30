using Dal;
using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoriaService
    {
        
        
        private readonly CategoriaDAL _categoriaDAL;

        public CategoriaService(CategoriaDAL categoriaDAL)
        {
            _categoriaDAL = categoriaDAL;
        }
        public int CrearCategoria(Categoria c) => _categoriaDAL.Insertar(c);

        public Categoria ObtenerCategoria(int id) => _categoriaDAL.ObtenerPorId(id);

        public List<Categoria> ListarCategorias() => _categoriaDAL.Listar();

        public void ActualizarCategoria(Categoria c) => _categoriaDAL.Actualizar(c);

        public void EliminarCategoria(int id) => _categoriaDAL.Eliminar(id);
    }
}
