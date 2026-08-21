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
    public class OrdenDAL
    {
        private readonly ConexionBD _bd;
        public OrdenDAL(ConexionBD bd)
        {
            _bd = bd;
        }

        public async Task<List<Orden>> ListarOrdenesAsync(int pagina, int tamano)
        {
            var lista = new List<Orden>();

            int offset = (pagina - 1) * tamano;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"
            SELECT
                o.OrdenId,
                o.UsuarioId,
                u.NombreUsuario,
                o.FechaOrden,
                o.Total,
                o.Estado
            FROM Ordenes o
            INNER JOIN Usuarios u
                ON o.UsuarioId = u.UsuarioId
            ORDER BY o.OrdenId
            OFFSET @Offset ROWS
            FETCH NEXT @Tamano ROWS ONLY";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Offset", offset);
                cmd.Parameters.AddWithValue("@Tamano", tamano);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new Orden
                    {
                        OrdenId = (int)reader["OrdenId"],
                        UsuarioId = (int)reader["UsuarioId"],
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        FechaOrden = (DateTime)reader["FechaOrden"],
                        Total = (decimal)reader["Total"],
                        Estado = reader["Estado"].ToString()
                    });
                }
            }

            return lista;
        }
        public async Task<List<Orden>> ListarOrdenesDeAsync(int usuarioId)
        {
            var lista = new List<Orden>();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"
            SELECT
                o.OrdenId,
                o.UsuarioId,
                u.NombreUsuario,
                o.FechaOrden,
                o.Total,
                o.Estado
            FROM Ordenes o
            INNER JOIN Usuarios u
                ON o.UsuarioId = u.UsuarioId
            WHERE o.UsuarioId = @UsuarioId
            ORDER BY o.OrdenId DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new Orden
                    {
                        OrdenId = (int)reader["OrdenId"],
                        UsuarioId = (int)reader["UsuarioId"],
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        FechaOrden = (DateTime)reader["FechaOrden"],
                        Total = (decimal)reader["Total"],
                        Estado = reader["Estado"].ToString()
                    });
                }
            }

            return lista;
        }

        public async Task<Orden> ObtenerPorIdAsync(int id)
        {
            Orden orden = null;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Ordenes WHERE OrdenId=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    orden = new Orden
                    {
                        OrdenId = (int)reader["OrdenId"],
                        UsuarioId = (int)reader["UsuarioId"],
                        FechaOrden = (DateTime)reader["FechaOrden"],
                        Total = (decimal)reader["Total"],
                        Estado = reader["Estado"].ToString()
                    };
                }
            }

            return orden;
        }

        public async Task<int> InsertarOrdenAsync(
    int usuarioId,
    List<OrdenDetalle> detalles)
        {
            int ordenId;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                await conn.OpenAsync();

                SqlTransaction tx = conn.BeginTransaction();

                try
                {
                    decimal total = 0;

                    foreach (var d in detalles)
                    {
                        total += d.Subtotal;
                    }

                    string queryOrden = @"
                INSERT INTO Ordenes
                (
                    UsuarioId,
                    FechaOrden,
                    Total,
                    Estado
                )
                VALUES
                (
                    @UsuarioId,
                    GETDATE(),
                    @Total,
                    'Confirmada'
                );

                SELECT SCOPE_IDENTITY();
            ";

                    SqlCommand cmdOrden =
                        new SqlCommand(queryOrden, conn, tx);

                    cmdOrden.Parameters.AddWithValue(
                        "@UsuarioId",
                        usuarioId
                    );

                    cmdOrden.Parameters.AddWithValue(
                        "@Total",
                        total
                    );

                    object resultado =
                        await cmdOrden.ExecuteScalarAsync();

                    ordenId = Convert.ToInt32(resultado);

                    foreach (var d in detalles)
                    {
                        string queryDetalle = @"
                    INSERT INTO OrdenDetalle
                    (
                        OrdenId,
                        ProductoId,
                        Cantidad,
                        PrecioUnitario
                    )
                    VALUES
                    (
                        @OrdenId,
                        @Prod,
                        @Cant,
                        @Precio
                    )";

                        SqlCommand cmdDet =
                            new SqlCommand(queryDetalle, conn, tx);

                        cmdDet.Parameters.AddWithValue(
                            "@OrdenId",
                            ordenId
                        );

                        cmdDet.Parameters.AddWithValue(
                            "@Prod",
                            d.ProductoId
                        );

                        cmdDet.Parameters.AddWithValue(
                            "@Cant",
                            d.Cantidad
                        );

                        cmdDet.Parameters.AddWithValue(
                            "@Precio",
                            d.PrecioUnitario
                        );

                        await cmdDet.ExecuteNonQueryAsync();
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }

            return ordenId;
        }

        public async Task ActualizarEstadoAsync(int ordenId, string estado)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"
            UPDATE Ordenes
            SET Estado=@Estado
            WHERE OrdenId=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Estado", estado);
                cmd.Parameters.AddWithValue("@Id", ordenId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<int> ContarOrdenesAsync()
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT COUNT(*) FROM Ordenes";

                SqlCommand cmd = new SqlCommand(query, conn);

                await conn.OpenAsync();

                object resultado = await cmd.ExecuteScalarAsync();

                return Convert.ToInt32(resultado);
            }
        }
    }
}
