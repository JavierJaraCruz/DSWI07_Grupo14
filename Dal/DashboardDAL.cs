using Dal;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Configuration;


namespace Web.DAL
{
    public class DashboardDAL
    {
        private readonly ConexionBD _bd;
        public DashboardDAL(ConexionBD bd)
        {
            _bd = bd;
        }

        public Dashboard ObtenerMetricas()
        {
            var model = new Dashboard();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                conn.Open();

                model.CantidadProductos = (int)new SqlCommand(
                    "SELECT COUNT(*) FROM Productos WHERE Activo = 1", conn
                ).ExecuteScalar();

                model.CantidadVentas = (int)new SqlCommand(
                    "SELECT COUNT(*) FROM Ordenes", conn
                ).ExecuteScalar();

                model.StockBajo = (int)new SqlCommand(
                    "SELECT COUNT(*) FROM Productos WHERE Stock <= 5 AND Activo = 1", conn
                ).ExecuteScalar();

                model.TotalVentasMonto = Convert.ToDecimal(new SqlCommand(
                    "SELECT ISNULL(SUM(Monto),0) FROM Pagos", conn
                ).ExecuteScalar());
            }

            return model;
        }
    }
}