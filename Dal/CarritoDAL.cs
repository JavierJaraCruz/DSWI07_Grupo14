using Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace Dal

{
    public class CarritoDAL
    {
        private readonly ConexionBD _bd;
        public CarritoDAL(ConexionBD bd)
        {
            _bd = bd;
        }

        public Carrito ObtenerPorUsuario(int usuarioId)
        {
            Carrito? carrito = null;
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Carrito WHERE UsuarioId=@UsuarioId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    carrito = new Carrito
                    {
                        CarritoId = (int)reader["CarritoId"],
                        UsuarioId = (int)reader["UsuarioId"],
                        FechaCreacion = (DateTime)reader["FechaCreacion"],
                        Estado = reader["Estado"].ToString()
                    };
                }
            }
            return carrito;
        }

        public Carrito ObtenerCarrito(int carritoId)
        {
            Carrito? carrito = null;
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Carrito WHERE CarritoId=@CarritoId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CarritoId", carritoId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    carrito = new Carrito
                    {
                        CarritoId = (int)reader["CarritoId"],
                        UsuarioId = (int)reader["UsuarioId"],
                        FechaCreacion = (DateTime)reader["FechaCreacion"],
                        Estado = reader["Estado"].ToString()
                    };
                }
            }
            return carrito;
        }

        public int CrearCarrito(int usuarioId)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"INSERT INTO Carrito (UsuarioId,FechaCreacion,Estado)
                                 VALUES (@UsuarioId,GETDATE(),'Activo'); SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void AgregarProducto(int carritoId, int productoId, int cantidad, decimal precioUnitario)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                conn.Open();
                SqlTransaction tx = conn.BeginTransaction();
                try
                {
                    string query = @"INSERT INTO CarritoDetalle
                            (
                                CarritoId,
                                ProductoId,
                                Cantidad,
                                PrecioUnitario
                            )
                            VALUES
                            (
                                @CarritoId,
                                @ProductoId,
                                @Cantidad,
                                @Precio
                            )";
                    SqlCommand cmd = new SqlCommand(query, conn, tx);
                    cmd.Parameters.AddWithValue("@CarritoId", carritoId);
                    cmd.Parameters.AddWithValue("@ProductoId", productoId);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@Precio", precioUnitario);

                    cmd.ExecuteNonQuery();

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public void EliminarProducto(int detalleId)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "DELETE FROM CarritoDetalle WHERE CarritoDetalleId=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", detalleId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void VaciarCarrito(int carritoId)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "DELETE FROM CarritoDetalle WHERE CarritoId=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", carritoId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Eliminar(int id)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "DELETE FROM Carrito WHERE CarritoId=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<CarritoDetalle> ObtenerDetalles(int carritoId)
        {
            List<CarritoDetalle> lista = new List<CarritoDetalle>();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"SELECT *
                         FROM CarritoDetalle
                         WHERE CarritoId = @CarritoId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CarritoId", carritoId);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new CarritoDetalle
                    {
                        CarritoDetalleId = (int)reader["CarritoDetalleId"],
                        CarritoId = (int)reader["CarritoId"],
                        ProductoId = (int)reader["ProductoId"],
                        Cantidad = (int)reader["Cantidad"],
                        PrecioUnitario = (decimal)reader["PrecioUnitario"],
                        Subtotal = (decimal)reader["Subtotal"]
                    });
                }
            }

            return lista;
        }
    }
}