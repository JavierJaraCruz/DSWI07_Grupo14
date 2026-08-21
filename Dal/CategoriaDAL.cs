using Dal;
using Entities;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CategoriaDAL
    {
        private readonly ConexionBD _bd;
        public CategoriaDAL(ConexionBD bd)
        {
            _bd = bd;
        }

        public async Task<List<Categoria>> Listar()
        {
            var lista = new List<Categoria>();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Categorias";

                SqlCommand cmd = new SqlCommand(query, conn);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new Categoria
                    {
                        CategoriaId = (int)reader["CategoriaId"],
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Activo = (bool)reader["Activo"]
                    });
                }
            }

            return lista;
        }

        public async Task Activar(int id)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"UPDATE Categorias
                                 SET Activo = 1
                                 WHERE CategoriaId = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<Categoria>> ListarSoloActivos()
        {
            var lista = new List<Categoria>();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"SELECT CategoriaId, Nombre, Descripcion, Activo
                                 FROM Categorias
                                 WHERE Activo = 1";

                SqlCommand cmd = new SqlCommand(query, conn);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new Categoria
                    {
                        CategoriaId = (int)reader["CategoriaId"],
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Activo = (bool)reader["Activo"]
                    });
                }
            }

            return lista;
        }

        public async Task<Categoria> ObtenerPorId(int id)
        {
            Categoria categoria = null;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"SELECT CategoriaId, Nombre, Descripcion, Activo
                                 FROM Categorias
                                 WHERE CategoriaId = @id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    categoria = new Categoria
                    {
                        CategoriaId = (int)reader["CategoriaId"],
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Activo = (bool)reader["Activo"]
                    };
                }
            }

            return categoria;
        }

        public async Task<int> Insertar(Categoria categoria)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"INSERT INTO Categorias(Nombre,Descripcion)
                                 VALUES(@Nombre,@Descripcion);
                                 SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);

                await conn.OpenAsync();

                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
        }

        public async Task Actualizar(Categoria categoria)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"UPDATE Categorias 
                                 SET Nombre = @Nombre,
                                     Descripcion = @Descripcion,
                                     Activo = @Activo
                                 WHERE CategoriaId = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                cmd.Parameters.AddWithValue("@Activo", categoria.Activo);
                cmd.Parameters.AddWithValue("@Id", categoria.CategoriaId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task Eliminar(int id)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"UPDATE Categorias
                                 SET Activo = 0
                                 WHERE CategoriaId = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
