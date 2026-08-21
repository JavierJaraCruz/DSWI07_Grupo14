using Dal;
using Entities;
using Microsoft.Data.SqlClient;

namespace Web.DAL
{
    public class DashboardDAL
    {
        private readonly ConexionBD _bd;

        public DashboardDAL(ConexionBD bd)
        {
            _bd = bd;
        }

        public async Task<Dashboard> ObtenerMetricas()
        {
            var model = new Dashboard();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                await conn.OpenAsync();

                model.CantidadProductos = Convert.ToInt32(
                    await new SqlCommand(
                        "SELECT COUNT(*) FROM Productos WHERE Activo = 1",
                        conn
                    ).ExecuteScalarAsync()
                );

                model.CantidadVentas = Convert.ToInt32(
                    await new SqlCommand(
                        "SELECT COUNT(*) FROM Ordenes",
                        conn
                    ).ExecuteScalarAsync()
                );

                model.StockBajo = Convert.ToInt32(
                    await new SqlCommand(
                        "SELECT COUNT(*) FROM Productos WHERE Stock <= 5 AND Activo = 1",
                        conn
                    ).ExecuteScalarAsync()
                );

                model.TotalVentasMonto = Convert.ToDecimal(
                    await new SqlCommand(
                        "SELECT ISNULL(SUM(Monto), 0) FROM Pagos",
                        conn
                    ).ExecuteScalarAsync()
                );
            }

            return model;
        }
    }
}