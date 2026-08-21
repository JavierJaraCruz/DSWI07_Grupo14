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
        public async Task<int> CrearCategoria(Categoria c)
             => await _categoriaDAL.Insertar(c);

        public async Task<Categoria> ObtenerCategoria(int id)
            => await _categoriaDAL.ObtenerPorId(id);

        public async Task<List<Categoria>> ListarCategorias()
            => await _categoriaDAL.Listar();

        public async Task ActivarCategoria(int id)
            => await _categoriaDAL.Activar(id);

        public async Task<List<Categoria>> ListarSoloActivos()
            => await _categoriaDAL.ListarSoloActivos();

        public async Task ActualizarCategoria(Categoria c)
            => await _categoriaDAL.Actualizar(c);

        public async Task EliminarCategoria(int id)
            => await _categoriaDAL.Eliminar(id);
    }
}
