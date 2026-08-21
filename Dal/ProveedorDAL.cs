
using Dal;
using Entities;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;


namespace DAL
{
    public class ProveedorDAL
    {
        private readonly ConexionBD _bd;
        public ProveedorDAL(ConexionBD bd)
        {
            _bd = bd;
        }

        public async Task<List<Proveedor>> ListarAsync()
        {
            var lista = new List<Proveedor>();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Proveedores";

                SqlCommand cmd = new SqlCommand(query, conn);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new Proveedor
                    {
                        ProveedorId = (int)reader["ProveedorId"],
                        Nombre = reader["Nombre"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Direccion = reader["Direccion"].ToString()
                    });
                }
            }

            return lista;
        }

        public async Task<Proveedor> ObtenerProveedorAsync(int id)
        {
            Proveedor proveedor = null;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Proveedores WHERE ProveedorId=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    proveedor = new Proveedor
                    {
                        ProveedorId = (int)reader["ProveedorId"],
                        Nombre = reader["Nombre"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Direccion = reader["Direccion"].ToString()
                    };
                }
            }

            return proveedor;
        }

        public async Task<int> InsertarAsync(Proveedor p)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                await conn.OpenAsync();

                SqlTransaction tx = conn.BeginTransaction();

                try
                {
                    string query = @"INSERT INTO Proveedores 
                             (Nombre,Email,Telefono,Direccion)
                             VALUES 
                             (@Nombre,@Email,@Tel,@Dir);
                             SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(query, conn, tx);

                    cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                    cmd.Parameters.AddWithValue("@Email",
                        p.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tel",
                        p.Telefono ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Dir",
                        p.Direccion ?? (object)DBNull.Value);

                    object resultado = await cmd.ExecuteScalarAsync();

                    int id = Convert.ToInt32(resultado);

                    tx.Commit();

                    return id;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public async Task ActualizarAsync(Proveedor p)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"UPDATE Proveedores 
                         SET Nombre=@Nombre,
                             Email=@Email,
                             Telefono=@Tel,
                             Direccion=@Dir
                         WHERE ProveedorId=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@Email",
                    p.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Tel",
                    p.Telefono ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Dir",
                    p.Direccion ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", p.ProveedorId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task EliminarAsync(int id)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "DELETE FROM Proveedores WHERE ProveedorId=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
