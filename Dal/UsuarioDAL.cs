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
    public class UsuarioDAL
    {       //Preparar la cadema de conexion o TEXTO DE CONEXION
        private readonly ConexionBD _bd;
        public UsuarioDAL(ConexionBD bd)
        {
            _bd = bd;
        }


        //creamos el metodo o funcion a utilizar del tipo List porque devolveremos una lista
        public async Task<List<Usuario>> ListarAsync()
        {
            var lista = new List<Usuario>();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Usuarios";

                SqlCommand cmd = new SqlCommand(query, conn);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new Usuario
                    {
                        UsuarioId = (int)reader["UsuarioId"],
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        Email = reader["Email"].ToString(),
                        PasswordHash = reader["PasswordHash"].ToString(),
                        Salt = reader["Salt"].ToString(),
                        FechaRegistro = (DateTime)reader["FechaRegistro"],
                        Estado = (bool)reader["Estado"]
                    });
                }
            }

            return lista;
        }


        public async Task<Usuario> ObtenerPorIdAsync(int id)
        {
            Usuario usuario = null;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Usuarios WHERE UsuarioId=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    usuario = new Usuario
                    {
                        UsuarioId = (int)reader["UsuarioId"],
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        Email = reader["Email"].ToString(),
                        PasswordHash = reader["PasswordHash"].ToString(),
                        Salt = reader["Salt"].ToString(),
                        FechaRegistro = (DateTime)reader["FechaRegistro"],
                        Estado = (bool)reader["Estado"]
                    };
                }
            }

            return usuario;
        }

        //insertar
        public async Task<int> InsertarAsync(Usuario u)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"INSERT INTO Usuarios 
                        (NombreUsuario, Email, PasswordHash, Salt, FechaRegistro, Estado)
                        VALUES 
                        (@NombreUsuario, @Email, @PasswordHash, @Salt, @FechaRegistro, @Estado);
                        SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@NombreUsuario", u.NombreUsuario);
                cmd.Parameters.AddWithValue("@Email", u.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", u.PasswordHash);
                cmd.Parameters.AddWithValue("@Salt", u.Salt);
                cmd.Parameters.AddWithValue("@FechaRegistro", u.FechaRegistro);
                cmd.Parameters.AddWithValue("@Estado", u.Estado);

                await conn.OpenAsync();

                object resultado = await cmd.ExecuteScalarAsync();

                return Convert.ToInt32(resultado);
            }
        }
        public async Task ActualizarAsync(Usuario usuario)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"UPDATE Usuarios 
                         SET NombreUsuario=@Nom,
                             Email=@Ema,
                             PasswordHash=@Pass,
                             Salt=@Sal,
                             FechaRegistro=@Fecha,
                             Estado=@Estad
                         WHERE UsuarioId=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nom", usuario.NombreUsuario);
                cmd.Parameters.AddWithValue("@Ema", usuario.Email);
                cmd.Parameters.AddWithValue("@Pass", usuario.PasswordHash);
                cmd.Parameters.AddWithValue("@Sal", usuario.Salt);
                cmd.Parameters.AddWithValue("@Fecha", usuario.FechaRegistro);
                cmd.Parameters.AddWithValue("@Estad", usuario.Estado);
                cmd.Parameters.AddWithValue("@Id", usuario.UsuarioId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task EliminarAsync(int id)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "DELETE FROM Usuarios WHERE UsuarioId=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<Usuario> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
        {
            Usuario usuario = null;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Usuarios WHERE NombreUsuario = @nombreUsuario";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    usuario = new Usuario
                    {
                        UsuarioId = (int)reader["UsuarioId"],
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        Email = reader["Email"].ToString(),
                        PasswordHash = reader["PasswordHash"].ToString(),
                        Salt = reader["Salt"].ToString(),
                        FechaRegistro = (DateTime)reader["FechaRegistro"],
                        Estado = (bool)reader["Estado"]
                    };
                }
            }

            return usuario;
        }


        public async Task AsignarRolAsync(int usuarioId, int rolId)
        {
            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"INSERT INTO UsuarioRoles 
                         (UsuarioId, RolId)
                         VALUES (@UsuarioId, @RolId)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                cmd.Parameters.AddWithValue("@RolId", rolId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }


        public async Task<List<Rol>> ListarRolesAsync()
        {
            var lista = new List<Rol>();

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = "SELECT * FROM Roles";

                SqlCommand cmd = new SqlCommand(query, conn);

                await conn.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new Rol
                    {
                        RolId = (int)reader["RolId"],
                        NombreRol = reader["NombreRol"].ToString()
                    });
                }
            }

            return lista;
        }
        public async Task<string> ObtenerNombreRolPorUsuarioAsync(int usuarioId)
        {
            string nombreRol = null;

            using (SqlConnection conn = _bd.ObtenerConexion())
            {
                string query = @"SELECT r.NombreRol 
                         FROM Roles r 
                         INNER JOIN UsuarioRoles ur 
                             ON r.RolId = ur.RolId 
                         WHERE ur.UsuarioId = @UsuarioId";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                await conn.OpenAsync();

                object resultado = await cmd.ExecuteScalarAsync();

                if (resultado != null && resultado != DBNull.Value)
                {
                    nombreRol = resultado.ToString();
                }
            }

            return nombreRol;
        }

    }
}
