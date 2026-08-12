namespace Burgos0._2.Models.ApiModel
{
    public class CambiarPasswordRequest
    {
        public string NombreUsuario { get; set; }

        public string PasswordActual { get; set; }

        public string PasswordNueva { get; set; }
    }
}
