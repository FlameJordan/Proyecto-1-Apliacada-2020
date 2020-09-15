using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiendaComputacion.Models;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace TiendaComputacion.Data
{
    public class DataProducto
    {

        private MySqlConnection GetConnection()
        {
            String datosConectar = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
            MySqlConnection conectar = new MySqlConnection(datosConectar);
            return conectar;
        }
        public void insertarProducto(Producto producto)
        {
            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                MySqlCommand com = new MySqlCommand("sp_insertarProductoComputacion", conexion);
                com.CommandType = System.Data.CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@nombre", producto.Nombre);
                com.Parameters.AddWithValue("@cantidadStock", producto.CantidadStock);
                com.Parameters.AddWithValue("@precio", producto.Precio);
                com.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                com.Parameters.AddWithValue("@tipo", producto.Tipo);
                com.Parameters.AddWithValue("@imagen", producto.Imagen);

                com.ExecuteNonQuery();
            }
        }

        public List<Producto> obtenerTodos()
        {
            List<Producto> list = new List<Producto>();
            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                MySqlCommand com = new MySqlCommand("sp_obtenerProductosComputacion", conexion);
                com.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = com.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Producto()
                    {
                        Id = int.Parse(reader["id"].ToString()),
                        Nombre = reader["nombreproducto"].ToString(),
                        CantidadStock = int.Parse(reader["cantidadstock"].ToString()),
                        Precio = int.Parse(reader["precio"].ToString()),
                        Descripcion = reader["descripcion"].ToString(),
                        Tipo = int.Parse(reader["tipo"].ToString()),
                        Imagen = reader["imagen"].ToString()
                    });
                }
            }

            return list;
        }

        public void eliminarProducto(int id)
        {
            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                MySqlCommand com = new MySqlCommand("sp_eliminarProductoComputacion", conexion);
                com.CommandType = System.Data.CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@idi", id);

                com.ExecuteNonQuery();
            }
        }

        public void actualizarProducto(Producto producto)
        {
            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                MySqlCommand com = new MySqlCommand("sp_actualizarProductoComputacion", conexion);
                com.CommandType = System.Data.CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@idi", producto.Id);
                com.Parameters.AddWithValue("@nombre", producto.Nombre);
                com.Parameters.AddWithValue("@cantidadStocki", producto.CantidadStock);
                com.Parameters.AddWithValue("@precioi", producto.Precio);
                com.Parameters.AddWithValue("@descripcioni", producto.Descripcion);
                com.Parameters.AddWithValue("@tipoi", producto.Tipo);
                com.Parameters.AddWithValue("@imageni", producto.Imagen);

                com.ExecuteNonQuery();
            }
        }

        public Producto obtenerProducto(int id)
        {
            Producto producto = new Producto();
            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                MySqlCommand com = new MySqlCommand("sp_obtenerProductoComputacion", conexion);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idi", id);

                var reader = com.ExecuteReader();
                while (reader.Read())
                {
                    producto.Id = id;
                    producto.Nombre = reader["nombreproducto"].ToString();
                    producto.CantidadStock = int.Parse(reader["cantidadstock"].ToString());
                    producto.Precio = int.Parse(reader["precio"].ToString());
                    producto.Descripcion = reader["descripcion"].ToString();
                    producto.Tipo = int.Parse(reader["tipo"].ToString());
                    producto.Imagen = reader["imagen"].ToString();
                }
            }

            return producto;
        }
    }
}