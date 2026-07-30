
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

        public List<Proveedor> Listar()
        {
            var lista = new List<Proveedor>();
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Proveedores";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
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

        public Proveedor ObtenerProveedor(int id)
        {
            Proveedor proveedor = null;
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Proveedores WHERE ProveedorId=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
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

        public int Insertar(Proveedor p)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                conn.Open();
                SqlTransaction tx = conn.BeginTransaction();
                try
                {
                    string query = @"INSERT INTO Proveedores (Nombre,Email,Telefono,Direccion)
                             VALUES (@Nombre,@Email,@Tel,@Dir); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmd = new SqlCommand(query, conn, tx);
                    cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                    cmd.Parameters.AddWithValue("@Email", p.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tel", p.Telefono ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Dir", p.Direccion ?? (object)DBNull.Value);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());

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

        public void Actualizar(Proveedor p)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"UPDATE Proveedores SET Nombre=@Nombre,Email=@Email,Telefono=@Tel,Direccion=@Dir
                                 WHERE ProveedorId=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@Email", p.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Tel", p.Telefono ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Dir", p.Direccion ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", p.ProveedorId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Eliminar(int id)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "DELETE FROM Proveedores WHERE ProveedorId=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
