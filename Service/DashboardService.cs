using Entities;
using Web.DAL;



namespace Web.Services
{
    public class DashboardService
    {
        private readonly DashboardDAL _dashboardDAL;

        public DashboardService()
        {
            _dashboardDAL = new DashboardDAL();
        }

        public Dashboard ObtenerDashboard()
        {
           
            return _dashboardDAL.ObtenerMetricas();
        }

        public string ObtenerMensajeEstado(Dashboard data)
        {
            return data.StockBajo == 0
                ? "Todo en orden. No hay alertas."
                : $"¡Atención! Tienes {data.StockBajo} productos con stock crítico.";
        }
    }
}