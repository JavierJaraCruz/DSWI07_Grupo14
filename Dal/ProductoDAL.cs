using Dal;
using Entities;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;



namespace DAL

{

    public class ProductoDAL

    {


        private readonly ConexionBD _bd;
        public ProductoDAL(ConexionBD bd)
        {
            _bd = bd;
        }



        public List<Producto> Listar()

        {

            var lista = new List<Producto>();

            using (SqlConnection conn = _bd.ObtenerConexion())

            {

                string query = @"SELECT
                        p.*,
                        c.Nombre AS CategoriaNombre
                    FROM Productos p
                    INNER JOIN Categorias c
                        ON p.CategoriaId = c.CategoriaId
                    WHERE p.Activo = 1";

                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())

                {

                    lista.Add(new Producto

                    {

                        ProductoId = (int)reader["ProductoId"],

                        Nombre = reader["Nombre"].ToString(),

                        Descripcion = reader["Descripcion"].ToString(),

                        Precio = (decimal)reader["Precio"],

                        Stock = (int)reader["Stock"],

                        CategoriaId = (int)reader["CategoriaId"],

                        Activo = (bool)reader["Activo"],

                        ImagenUrl = reader["ImagenUrl"].ToString(),
                        CategoriaNombre = reader["CategoriaNombre"].ToString()

                    });

                }

            }

            return lista;

        }

        public Producto ObtenerPorId(int id)

        {

            Producto producto = null;

            using (SqlConnection conn = _bd.ObtenerConexion())

            {

                string query = @"SELECT
                        Productos.*,
                        Categorias.Nombre AS CategoriaNombre
                    FROM Productos
                    INNER JOIN Categorias
                        ON Productos.CategoriaId = Categorias.CategoriaId
                    WHERE Productos.ProductoId = @id
                      AND Productos.Activo = 1";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())

                {

                    producto = new Producto

                    {

                        ProductoId = (int)reader["ProductoId"],

                        Nombre = reader["Nombre"].ToString(),

                        Descripcion = reader["Descripcion"].ToString(),

                        Precio = (decimal)reader["Precio"],

                        Stock = (int)reader["Stock"],

                        CategoriaId = (int)reader["CategoriaId"],

                        Activo = (bool)reader["Activo"],

                        ImagenUrl = reader["ImagenUrl"].ToString(),
                        CategoriaNombre = reader["CategoriaNombre"].ToString()

                    };

                }

            }

            return producto;

        }



        public int Insertar(Producto p)

        {

            using (SqlConnection conn = _bd.ObtenerConexion())

            {

                string query = @"INSERT INTO Productos (Nombre,Descripcion,Precio,Stock,CategoriaId,Activo,ImagenUrl)

             VALUES (@Nombre,@Desc,@Precio,@Stock,@Cat,@Activo,@Img);

             SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nombre", p.Nombre);

                cmd.Parameters.AddWithValue("@Desc", p.Descripcion);

                cmd.Parameters.AddWithValue("@Precio", p.Precio);

                cmd.Parameters.AddWithValue("@Stock", p.Stock);

                cmd.Parameters.AddWithValue("@Cat", p.CategoriaId);

                cmd.Parameters.AddWithValue("@Activo", p.Activo);

                cmd.Parameters.AddWithValue("@Img", p.ImagenUrl ?? (object)DBNull.Value);

                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar());

            }

        }
        public void Actualizar(Producto p)

        {

            using (SqlConnection conn = _bd.ObtenerConexion())

            {

                string query = @"UPDATE Productos SET Nombre=@Nombre,Descripcion=@Desc,Precio=@Precio,

             Stock=@Stock,CategoriaId=@Cat,Activo=@Activo,ImagenUrl=@Img WHERE ProductoId=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nombre", p.Nombre);

                cmd.Parameters.AddWithValue("@Desc", p.Descripcion);

                cmd.Parameters.AddWithValue("@Precio", p.Precio);

                cmd.Parameters.AddWithValue("@Stock", p.Stock);

                cmd.Parameters.AddWithValue("@Cat", p.CategoriaId);

                cmd.Parameters.AddWithValue("@Activo", p.Activo);

                cmd.Parameters.AddWithValue("@Img", p.ImagenUrl ?? (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@Id", p.ProductoId);

                conn.Open();

                cmd.ExecuteNonQuery();

            }

        }



        public void ActualizarStock(int productoId, int cantidad, string tipoMovimiento, string referencia)

        {

            using (SqlConnection conn = _bd.ObtenerConexion())

            {

                conn.Open();

                SqlTransaction tx = conn.BeginTransaction();

                try

                {

                    string insertMov = @"INSERT INTO InventarioMovimiento (ProductoId,TipoMovimiento,Cantidad,Referencia)

             VALUES (@Prod,@Tipo,@Cant,@Ref)";

                    SqlCommand cmdMov = new SqlCommand(insertMov, conn, tx);

                    cmdMov.Parameters.AddWithValue("@Prod", productoId);

                    cmdMov.Parameters.AddWithValue("@Tipo", tipoMovimiento);

                    cmdMov.Parameters.AddWithValue("@Cant", cantidad);

                    cmdMov.Parameters.AddWithValue("@Ref", referencia);

                    cmdMov.ExecuteNonQuery();



                    string updateStock = tipoMovimiento == "Entrada"

                      ? "UPDATE Productos SET Stock = Stock + @Cant WHERE ProductoId=@Prod"

                      : "UPDATE Productos SET Stock = Stock - @Cant WHERE ProductoId=@Prod";



                    SqlCommand cmdStock = new SqlCommand(updateStock, conn, tx);

                    cmdStock.Parameters.AddWithValue("@Prod", productoId);

                    cmdStock.Parameters.AddWithValue("@Cant", cantidad);

                    cmdStock.ExecuteNonQuery();



                    tx.Commit();

                }

                catch

                {

                    tx.Rollback();

                    throw;

                }

            }

        }





        public void Eliminar(int id)

        {

            using (SqlConnection conn = _bd.ObtenerConexion())

            {

                string query = "UPDATE Productos SET Activo = 0 WHERE ProductoId = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();

                cmd.ExecuteNonQuery();

            }

        }
    }
}
