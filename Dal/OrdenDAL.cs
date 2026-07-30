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

        public List<Orden> ListarOrdenes()
        {
            var lista = new List<Orden>();
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Ordenes";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Orden
                    {
                        OrdenId = (int)reader["OrdenId"],
                        UsuarioId = (int)reader["UsuarioId"],
                        FechaOrden = (DateTime)reader["FechaOrden"],
                        Total = (decimal)reader["Total"],
                        Estado = reader["Estado"].ToString()
                    });
                }
            }
            return lista;
        }

        public Orden ObtenerPorId(int id)
        {
            Orden orden = null;
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Ordenes WHERE OrdenId=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
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

        public int InsertarOrden(int usuarioId, List<OrdenDetalle> detalles)
        {
            int ordenId;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                conn.Open();

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

                    ordenId =
                        Convert.ToInt32(
                            cmdOrden.ExecuteScalar()
                        );

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

                        cmdDet.ExecuteNonQuery();
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

        public void ActualizarEstado(int ordenId, string estado)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "UPDATE Ordenes SET Estado=@Estado WHERE OrdenId=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Estado", estado);
                cmd.Parameters.AddWithValue("@Id", ordenId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
