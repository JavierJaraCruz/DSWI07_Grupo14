using Dal;
using Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public class InventarioDAL
    {
        private readonly ConexionBD _bd;

        public InventarioDAL(ConexionBD bd)
        {
            _bd = bd;
        }


        public async Task<List<InventarioMovimiento>> ListarMovimientosAsync()
        {
            var lista = new List<InventarioMovimiento>();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM InventarioMovimiento";

                SqlCommand cmd = new SqlCommand(query, conn);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new InventarioMovimiento
                    {
                        MovimientoId = (int)reader["MovimientoId"],
                        ProductoId = (int)reader["ProductoId"],
                        TipoMovimiento = reader["TipoMovimiento"].ToString(),
                        Cantidad = (int)reader["Cantidad"],
                        FechaMovimiento = (DateTime)reader["FechaMovimiento"],
                        Referencia = reader["Referencia"].ToString()
                    });
                }
            }

            return lista;
        }


        public async Task<List<InventarioMovimiento>> ListarMovimientosAsync(int productoId)
        {
            var lista = new List<InventarioMovimiento>();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"
                    SELECT *
                    FROM InventarioMovimiento
                    WHERE ProductoId = @ProductoId";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@ProductoId",
                    productoId
                );

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new InventarioMovimiento
                    {
                        MovimientoId = (int)reader["MovimientoId"],
                        ProductoId = (int)reader["ProductoId"],
                        TipoMovimiento = reader["TipoMovimiento"].ToString(),
                        Cantidad = (int)reader["Cantidad"],
                        FechaMovimiento = (DateTime)reader["FechaMovimiento"],
                        Referencia = reader["Referencia"].ToString()
                    });
                }
            }

            return lista;
        }


        public async Task<InventarioMovimiento> ObtenerPorIdAsync(int id)
        {
            InventarioMovimiento mov = null;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"
                    SELECT *
                    FROM InventarioMovimiento
                    WHERE MovimientoId = @id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    mov = new InventarioMovimiento
                    {
                        MovimientoId = (int)reader["MovimientoId"],
                        ProductoId = (int)reader["ProductoId"],
                        TipoMovimiento = reader["TipoMovimiento"].ToString(),
                        Cantidad = (int)reader["Cantidad"],
                        FechaMovimiento = (DateTime)reader["FechaMovimiento"],
                        Referencia = reader["Referencia"].ToString()
                    };
                }
            }

            return mov;
        }


        public async Task<int> InsertarMovimientoAsync(InventarioMovimiento mov)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                await conn.OpenAsync();

                SqlTransaction tx = conn.BeginTransaction();

                try
                {
                    string query = @"
                        INSERT INTO InventarioMovimiento
                        (
                            ProductoId,
                            TipoMovimiento,
                            Cantidad,
                            FechaMovimiento,
                            Referencia
                        )
                        VALUES
                        (
                            @Prod,
                            @Tipo,
                            @Cant,
                            @Fecha,
                            @Ref
                        );

                        SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(query, conn, tx);

                    cmd.Parameters.AddWithValue("@Prod", mov.ProductoId);
                    cmd.Parameters.AddWithValue("@Tipo", mov.TipoMovimiento);
                    cmd.Parameters.AddWithValue("@Cant", mov.Cantidad);
                    cmd.Parameters.AddWithValue("@Fecha", mov.FechaMovimiento);
                    cmd.Parameters.AddWithValue(
                        "@Ref",
                        mov.Referencia ?? (object)DBNull.Value
                    );

                    int id = Convert.ToInt32(
                        await cmd.ExecuteScalarAsync()
                    );

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


        // KARDEX BORRARLO NO ESTA EN USO
        public async Task<List<KardexItem>> ObtenerKardexPorProductoAsync(int productoId)
        {
            var lista = new List<KardexItem>();

            int saldo = 0;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"
                    SELECT
                        FechaMovimiento,
                        TipoMovimiento,
                        Cantidad,
                        Referencia
                    FROM InventarioMovimiento
                    WHERE ProductoId = @Prod
                    ORDER BY FechaMovimiento ASC";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Prod", productoId);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    string tipo = reader["TipoMovimiento"].ToString();

                    int cantidad = (int)reader["Cantidad"];

                    if (tipo == "Entrada")
                        saldo += cantidad;
                    else
                        saldo -= cantidad;

                    lista.Add(new KardexItem
                    {
                        Fecha = (DateTime)reader["FechaMovimiento"],
                        TipoMovimiento = tipo,
                        Cantidad = cantidad,
                        Referencia = reader["Referencia"].ToString(),
                        Saldo = saldo
                    });
                }
            }

            return lista;
        }


        public async Task InsertarKardexAsync(KardexItem item)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                await conn.OpenAsync();

                SqlTransaction tx = conn.BeginTransaction();

                try
                {
                    string query = @"
                        INSERT INTO InventarioMovimiento
                        (
                            ProductoId,
                            Fecha,
                            TipoMovimiento,
                            Cantidad,
                            Referencia,
                            Saldo
                        )
                        VALUES
                        (
                            @Prod,
                            @Fecha,
                            @Tipo,
                            @Cant,
                            @Ref,
                            @Saldo
                        )";

                    SqlCommand cmd = new SqlCommand(query, conn, tx);

                    cmd.Parameters.AddWithValue("@Prod", item.ProductoId);
                    cmd.Parameters.AddWithValue("@Fecha", item.Fecha);
                    cmd.Parameters.AddWithValue("@Tipo", item.TipoMovimiento);
                    cmd.Parameters.AddWithValue("@Cant", item.Cantidad);

                    cmd.Parameters.AddWithValue(
                        "@Ref",
                        item.Referencia ?? (object)DBNull.Value
                    );

                    cmd.Parameters.AddWithValue("@Saldo", item.Saldo);

                    await cmd.ExecuteNonQueryAsync();

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}