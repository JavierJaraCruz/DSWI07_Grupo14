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
    public class RolDAL
    {
        private readonly ConexionBD _bd;
        public RolDAL(ConexionBD bd)
        {
            _bd = bd;
        }

        public async Task<List<Rol>> ListarAsync()
        {
            var lista = new List<Rol>();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Roles";

                SqlCommand cmd = new SqlCommand(query, conn);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new Rol
                    {
                        RolId = (int)reader["RolId"],
                        NombreRol = reader["NombreRol"].ToString()
                    });
                }
            }

            return lista;
        }
        public async Task<Rol> ObtenerPorIdAsync(int id)
        {
            Rol rol = null;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Roles WHERE RolId = @id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    rol = new Rol
                    {
                        RolId = (int)reader["RolId"],
                        NombreRol = reader["NombreRol"].ToString()
                    };
                }
            }

            return rol;
        }

        public async Task<int> InsertarAsync(Rol r)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"INSERT INTO Roles (NombreRol)
                         VALUES (@NombreRol);
                         SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@NombreRol", r.NombreRol);

                await conn.OpenAsync();

                object resultado = await cmd.ExecuteScalarAsync();

                return Convert.ToInt32(resultado);
            }
        }

        public async Task ActualizarAsync(Rol rol)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"UPDATE Roles
                         SET NombreRol = @NomR
                         WHERE RolId = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@NomR", rol.NombreRol);
                cmd.Parameters.AddWithValue("@Id", rol.RolId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task EliminarAsync(int id)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "DELETE FROM Roles WHERE RolId = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }



}
