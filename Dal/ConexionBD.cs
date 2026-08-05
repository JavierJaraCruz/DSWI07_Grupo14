

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Dal;

/// <summary>
/// Centraliza la creación de la conexión a la base de datos.
/// Así no repetimos la cadena de conexión en cada controlador.
/// </summary>
public class ConexionBD
{
    private readonly string _cadena;

    public ConexionBD(IConfiguration configuration)
    {
        // Lee la cadena de conexión desde appsettings.json
        _cadena = configuration.GetConnectionString("burgos")
                  ?? throw new InvalidOperationException("No se encontró la cadena 'burgos'.");
    }

    /// <summary>Devuelve una conexión NUEVA (aún cerrada).</summary>
    public SqlConnection ObtenerConexion() => new SqlConnection(_cadena);

    /// <summary>Prueba la conexión: devuelve true si logra abrirla.</summary>
    public bool ProbarConexion(out string mensaje)
    {
        try
        {
            using var cn = ObtenerConexion();
            cn.Open();                                  // aquí ocurre la conexión real
            mensaje = $"Conexión exitosa a '{cn.Database}' en '{cn.DataSource}'.";
            return true;                                // el using cierra la conexión solo
        }
        catch (SqlException ex)
        {
            mensaje = $"Error de SQL Server: {ex.Message}";
            return false;
        }
        catch (Exception ex)
        {
            mensaje = $"Error inesperado: {ex.Message}";
            return false;
        }
    }

    /// <summary>Cuenta cuántos productos hay (consulta simple de verificación).</summary>
    public int ContarProductos()
    {
        using var cn = ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Producto", cn);
        return (int)cmd.ExecuteScalar();
    }
}
