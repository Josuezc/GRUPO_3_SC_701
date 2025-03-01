namespace GRUPO_3_SC_701.ViewModels
{
    public class DashboardViewModel
    {
        public List<UserTicketInfo> UserTickets { get; set; }
        public int TotalActiveRoutes { get; set; }
        public int TotalGoodVehicles { get; set; }
        public int TotalRegularVehicles { get; set; }
        public int TotalMaintenanceVehicles { get; set; }
        public int TotalTicketsCurrentMonth { get; set; }
    }

    public class UserTicketInfo
    {
        public string Usuario { get; set; }
        public string Ruta { get; set; }
        public TimeSpan Horario { get; set; }
        public DateTime FechaCompra { get; set; }
    }
}
