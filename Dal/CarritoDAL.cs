using Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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

        public async Task<Carrito> ObtenerPorUsuario(int usuarioId)
        {
            Carrito? carrito = null;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Carrito WHERE UsuarioId=@UsuarioId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
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

        public async Task<Carrito> ObtenerCarrito(int carritoId)
        {
            Carrito? carrito = null;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Carrito WHERE CarritoId=@CarritoId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CarritoId", carritoId);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
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

        public async Task<int> CrearCarrito(int usuarioId)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"INSERT INTO Carrito (UsuarioId,FechaCreacion,Estado)
                                 VALUES (@UsuarioId,GETDATE(),'Activo'); 
                                 SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                await conn.OpenAsync();

                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
        }

        public async Task AgregarProducto(
            int carritoId,
            int productoId,
            int cantidad,
            decimal precioUnitario)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                await conn.OpenAsync();

                SqlTransaction tx = (SqlTransaction)await conn.BeginTransactionAsync();

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

                    await cmd.ExecuteNonQueryAsync();

                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task EliminarProducto(int detalleId)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "DELETE FROM CarritoDetalle WHERE CarritoDetalleId=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", detalleId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task VaciarCarrito(int carritoId)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "DELETE FROM CarritoDetalle WHERE CarritoId=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", carritoId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task Eliminar(int id)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "DELETE FROM Carrito WHERE CarritoId=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<CarritoDetalle>> ObtenerDetalles(int carritoId)
        {
            List<CarritoDetalle> lista = new List<CarritoDetalle>();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"
                    SELECT 
                        cd.CarritoDetalleId,
                        cd.CarritoId,
                        cd.ProductoId,
                        p.Nombre,
                        p.ImagenUrl,
                        cd.Cantidad,
                        cd.PrecioUnitario,
                        cd.Subtotal
                    FROM CarritoDetalle cd
                    INNER JOIN Productos p 
                        ON cd.ProductoId = p.ProductoId
                    WHERE cd.CarritoId = @CarritoId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CarritoId", carritoId);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new CarritoDetalle
                    {
                        CarritoDetalleId = (int)reader["CarritoDetalleId"],
                        CarritoId = (int)reader["CarritoId"],
                        ProductoId = (int)reader["ProductoId"],

                        Nombre = reader["Nombre"].ToString(),
                        ImagenUrl = reader["ImagenUrl"].ToString(),

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