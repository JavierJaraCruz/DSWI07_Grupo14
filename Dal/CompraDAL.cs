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
    public class CompraDAL
    {
        private readonly ConexionBD _bd;
        public CompraDAL(ConexionBD bd)
        {
            _bd = bd;
        }

        public async Task<List<CompraProveedor>> ListarCompras()
        {
            var lista = new List<CompraProveedor>();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM ComprasProveedor";

                SqlCommand cmd = new SqlCommand(query, conn);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new CompraProveedor
                    {
                        CompraId = (int)reader["CompraId"],
                        ProveedorId = (int)reader["ProveedorId"],
                        FechaCompra = (DateTime)reader["FechaCompra"],
                        Total = (decimal)reader["Total"],
                        Estado = reader["Estado"].ToString()
                    });
                }
            }

            return lista;
        }

        public async Task<CompraProveedor> ObtenerPorId(int id)
        {
            CompraProveedor compra = null;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM ComprasProveedor WHERE CompraId=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    compra = new CompraProveedor
                    {
                        CompraId = (int)reader["CompraId"],
                        ProveedorId = (int)reader["ProveedorId"],
                        FechaCompra = (DateTime)reader["FechaCompra"],
                        Total = (decimal)reader["Total"],
                        Estado = reader["Estado"].ToString()
                    };
                }
            }

            return compra;
        }

        public async Task<int> InsertarCompra(
            int proveedorId,
            List<CompraDetalle> detalles)
        {
            int compraId;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                await conn.OpenAsync();

                SqlTransaction tx = (SqlTransaction)await conn.BeginTransactionAsync();

                try
                {
                    decimal total = 0;

                    foreach (var d in detalles)
                    {
                        total += d.Subtotal;
                    }

                    string queryCompra = @"
                        INSERT INTO ComprasProveedor
                        (
                            ProveedorId,
                            FechaCompra,
                            Total,
                            Estado
                        )
                        VALUES
                        (
                            @Prov,
                            GETDATE(),
                            @Total,
                            'Recibido'
                        );

                        SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdCompra =
                        new SqlCommand(queryCompra, conn, tx);

                    cmdCompra.Parameters.AddWithValue(
                        "@Prov",
                        proveedorId
                    );

                    cmdCompra.Parameters.AddWithValue(
                        "@Total",
                        total
                    );

                    compraId = Convert.ToInt32(
                        await cmdCompra.ExecuteScalarAsync()
                    );

                    foreach (var d in detalles)
                    {
                        string queryDet = @"
                            INSERT INTO CompraDetalle
                            (
                                CompraId,
                                ProductoId,
                                Cantidad,
                                PrecioUnitario,
                                Subtotal
                            )
                            VALUES
                            (
                                @CompraId,
                                @Prod,
                                @Cant,
                                @Precio,
                                @Subtotal
                            )";

                        SqlCommand cmdDet =
                            new SqlCommand(queryDet, conn, tx);

                        cmdDet.Parameters.AddWithValue(
                            "@CompraId",
                            compraId
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

                        cmdDet.Parameters.AddWithValue(
                            "@Subtotal",
                            d.Subtotal
                        );

                        await cmdDet.ExecuteNonQueryAsync();
                    }

                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }

            return compraId;
        }

        public async Task ActualizarEstado(
            int compraId,
            string estado)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"
                    UPDATE ComprasProveedor
                    SET Estado=@Estado
                    WHERE CompraId=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Estado", estado);
                cmd.Parameters.AddWithValue("@Id", compraId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
