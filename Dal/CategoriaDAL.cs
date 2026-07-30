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

        public List<Categoria> Listar()
        {
            var lista = new List<Categoria>();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Categorias";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Categoria
                    {
                        CategoriaId = (int)reader["CategoriaId"],
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),

                    });
                }
            }

            return lista;
        }

        public Categoria ObtenerPorId(int id)
        {
            Categoria categoria = null;
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Categorias WHERE CategoriaId = @id";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    categoria = new Categoria
                    {
                        CategoriaId = (int)reader["CategoriaId"],
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),

                    };
                }

            }



            return categoria;
        }
        public int Insertar(Categoria categoria)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"INSERT INTO Categorias(Nombre,Descripcion)
                                  VALUES(@Nombre,@Descripcion);
                                     SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);


                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void Actualizar(Categoria categoria)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"UPDATE Categorias SET Nombre=@Nombre,Descripcion=@Descripcion
                         WHERE CategoriaId=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);

                cmd.Parameters.AddWithValue("@Id", categoria.CategoriaId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }
        public void Eliminar(int id)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "DELETE FROM Categorias WHERE CategoriaId=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
