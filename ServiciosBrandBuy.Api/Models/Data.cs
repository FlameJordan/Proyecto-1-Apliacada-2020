using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ServiciosBrandBuy.Api.Models
{
    public class Data
    {
        NpgsqlConnection conn;
        MySqlConnection conectarMySQL;
        public Data()
        {
            conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["conexionPostgres"].ConnectionString);
            conectarMySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["conexionMySQL"].ConnectionString);
        }

        public Boolean validezClave(String clave) 
        {
            

            this.conn.Open();

            // Define a query
            NpgsqlCommand cmd = new NpgsqlCommand("verificarValidezClave", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@param_clave", NpgsqlTypes.NpgsqlDbType.Text, clave);
            var ret = (bool)cmd.ExecuteScalar();

            // Close connection
            this.conn.Close();
            return ret;
        }

        public List<Clave> buscarClaveExistente(String nombreEmpresa, String correoEmpresa) 
        {
            this.conn.Open();

            // Define a query
            NpgsqlCommand cmd = new NpgsqlCommand("obtenerClaveExistente", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@param_nombre_empresa", NpgsqlTypes.NpgsqlDbType.Text, nombreEmpresa);
            cmd.Parameters.AddWithValue("@param_correo_empresa", NpgsqlTypes.NpgsqlDbType.Text, correoEmpresa);

            Clave existente = new Clave();

            existente.claveGeneradaField = (String)cmd.ExecuteScalar();

            List<Clave> listaClave = new List<Clave>();
            listaClave.Add(existente);

            this.conn.Close();
            return listaClave;
        }

        public Boolean registrarClave(String clave, String nombreEmpresa, String correo) 
        {

            this.conn.Open();

            // Define a query
            NpgsqlCommand cmd = new NpgsqlCommand("registrarClaveDeEmpresa", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@param_clave", NpgsqlTypes.NpgsqlDbType.Text, clave);
            cmd.Parameters.AddWithValue("@param_empresa", NpgsqlTypes.NpgsqlDbType.Text, nombreEmpresa);
            cmd.Parameters.AddWithValue("@param_correo", NpgsqlTypes.NpgsqlDbType.Text, correo);
            var ret = (bool)cmd.ExecuteScalar();

            // Close connection
            this.conn.Close();
            return ret;
        }

        public Boolean validarConexionConClave(String nombreEmpresa, String clave) 
        {
            this.conn.Open();

            // Define a query
            NpgsqlCommand cmd = new NpgsqlCommand("validarConexionConClave", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@param_clave", NpgsqlTypes.NpgsqlDbType.Text, clave);
            cmd.Parameters.AddWithValue("@param_empresa", NpgsqlTypes.NpgsqlDbType.Text, nombreEmpresa);
            var ret = (bool)cmd.ExecuteScalar();

            // Close connection
            this.conn.Close();

            return ret;
        }

        public String registrarProductos(DatosEmpresa datosEmpresa)
        {

            this.conn.OpenAsync();

            //Obtener el id de la empresa que está registrando los productos en la BD de BrandBuy
            NpgsqlCommand cmd = new NpgsqlCommand("obtenerIdEmpresa", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@param_clave", NpgsqlTypes.NpgsqlDbType.Text, datosEmpresa.ClaveGenerada);
            int idEmpresa = (int)cmd.ExecuteScalar();

            //Obtener los productos que ya están registrados para comparar los cambios con los que se están ingresando
            cmd = new NpgsqlCommand("obtenerProductosProveedorParaValidacionDeCambios", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@param_id_empresa", NpgsqlTypes.NpgsqlDbType.Integer, idEmpresa);

            
            NpgsqlDataReader dr = cmd.ExecuteReader();

            List<Producto> listaProductosEnBaseDatos = new List<Producto>();

            // Output the rows of the first result set
            while (dr.Read()) 
            {
                Producto productoTemp = new Producto();
                productoTemp.Id = (int)dr[0];
                productoTemp.Nombre = (String)dr[1];
                productoTemp.Precio = (int)dr[2];
                productoTemp.Descripcion = (String)dr[3];
                productoTemp.Tipo = (int)dr[4];
                productoTemp.Imagen = (String)dr[5];
                productoTemp.CantidadStock = (int)dr[6];

                listaProductosEnBaseDatos.Add(productoTemp);
            }
            this.conn.CloseAsync();

            if (listaProductosEnBaseDatos.Count == 0)
            {
                this.conn.OpenAsync();
                foreach (Producto producto in datosEmpresa.Productos)
                {
                    // Define a query
                    cmd = new NpgsqlCommand("registrarProductoDeProveedor", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param_producto_id", NpgsqlTypes.NpgsqlDbType.Integer, producto.Id);
                    cmd.Parameters.AddWithValue("@param_producto_nombre", NpgsqlTypes.NpgsqlDbType.Text, producto.Nombre);
                    cmd.Parameters.AddWithValue("@param_producto_cantidad", NpgsqlTypes.NpgsqlDbType.Integer, producto.CantidadStock);
                    cmd.Parameters.AddWithValue("@param_producto_imagen", NpgsqlTypes.NpgsqlDbType.Text, producto.Imagen);
                    cmd.Parameters.AddWithValue("@param_producto_precio", NpgsqlTypes.NpgsqlDbType.Integer, producto.Precio);
                    cmd.Parameters.AddWithValue("@param_producto_descripcion", NpgsqlTypes.NpgsqlDbType.Text, producto.Descripcion);
                    cmd.Parameters.AddWithValue("@param_tipo", NpgsqlTypes.NpgsqlDbType.Integer, producto.Tipo);
                    cmd.Parameters.AddWithValue("@param_id_empresa", NpgsqlTypes.NpgsqlDbType.Integer, idEmpresa);

                    cmd.ExecuteScalar();
                }
                this.conn.CloseAsync();
            }
            else 
            {
                List<Producto> listaProductosParaActualizar = new List<Producto>();
                List<Producto> listaProductosParaCrear = new List<Producto>();

                foreach (Producto producto in datosEmpresa.Productos)
                {
                    Producto productoTemporal = listaProductosEnBaseDatos.Find(x => x.Id == producto.Id);

                    //primero debo verificar si se encontro en la lista de la BD para proseguir con estos ifs
                    if (productoTemporal == null)
                    {
                        listaProductosParaCrear.Add(producto);
                    }
                    else 
                    {
                        if (producto.Equals(productoTemporal))
                        {
                            listaProductosEnBaseDatos.Remove(productoTemporal);
                        }
                        else
                        {
                            listaProductosParaActualizar.Add(producto);
                            listaProductosEnBaseDatos.Remove(productoTemporal);
                        }
                    }
                    
                }
                //this.conn.CloseAsync();

                if (listaProductosEnBaseDatos.Count > 0) 
                {
                    this.eliminarProductos(listaProductosEnBaseDatos, idEmpresa);
                }
                if (listaProductosParaCrear.Count > 0) 
                {
                    this.agregarProductosNuevos(listaProductosParaCrear, idEmpresa);
                }
                if (listaProductosParaActualizar.Count > 0) 
                {
                    this.actualizarProductos(listaProductosParaActualizar, idEmpresa);
                }

            }
            return "Correcto";
        }

        public bool eliminarProductos(List<Producto> listaProductosParaEliminar, int idEmpresa) 
        {
            this.conn.OpenAsync();

            foreach (Producto producto in listaProductosParaEliminar) 
            {
                // Define a query
                NpgsqlCommand cmd = new NpgsqlCommand("eliminarProductoDeProveedor", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@param_producto_id", NpgsqlTypes.NpgsqlDbType.Integer, producto.Id);
                cmd.Parameters.AddWithValue("@param_id_empresa", NpgsqlTypes.NpgsqlDbType.Integer, idEmpresa);

                cmd.ExecuteScalar();
            }

            this.conn.CloseAsync();
            return true;
        }
        public bool agregarProductosNuevos(List<Producto> listaProductosParaCrear, int idEmpresa) 
        {
            this.conn.OpenAsync();

            foreach (Producto producto in listaProductosParaCrear)
            {
                // Define a query
                NpgsqlCommand cmd = new NpgsqlCommand("registrarProductoDeProveedor", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@param_producto_id", NpgsqlTypes.NpgsqlDbType.Integer, producto.Id);
                cmd.Parameters.AddWithValue("@param_producto_nombre", NpgsqlTypes.NpgsqlDbType.Text, producto.Nombre);
                cmd.Parameters.AddWithValue("@param_producto_cantidad", NpgsqlTypes.NpgsqlDbType.Integer, producto.CantidadStock);
                cmd.Parameters.AddWithValue("@param_producto_imagen", NpgsqlTypes.NpgsqlDbType.Text, producto.Imagen);
                cmd.Parameters.AddWithValue("@param_producto_precio", NpgsqlTypes.NpgsqlDbType.Integer, producto.Precio);
                cmd.Parameters.AddWithValue("@param_producto_descripcion", NpgsqlTypes.NpgsqlDbType.Text, producto.Descripcion);
                cmd.Parameters.AddWithValue("@param_tipo", NpgsqlTypes.NpgsqlDbType.Integer, producto.Tipo);
                cmd.Parameters.AddWithValue("@param_id_empresa", NpgsqlTypes.NpgsqlDbType.Integer, idEmpresa);

                cmd.ExecuteScalar();
            }
            this.conn.CloseAsync();
            return true;
        }
        public bool actualizarProductos(List<Producto> listaProductosParaActualizar, int idEmpresa) 
        {
            this.conn.OpenAsync();

            foreach (Producto producto in listaProductosParaActualizar)
            {
                // Define a query
                NpgsqlCommand cmd = new NpgsqlCommand("actualizarProductoDeProveedor", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@param_producto_id", NpgsqlTypes.NpgsqlDbType.Integer, producto.Id);
                cmd.Parameters.AddWithValue("@param_producto_nombre", NpgsqlTypes.NpgsqlDbType.Text, producto.Nombre);
                cmd.Parameters.AddWithValue("@param_producto_cantidad", NpgsqlTypes.NpgsqlDbType.Integer, producto.CantidadStock);
                cmd.Parameters.AddWithValue("@param_producto_imagen", NpgsqlTypes.NpgsqlDbType.Text, producto.Imagen);
                cmd.Parameters.AddWithValue("@param_producto_precio", NpgsqlTypes.NpgsqlDbType.Integer, producto.Precio);
                cmd.Parameters.AddWithValue("@param_producto_descripcion", NpgsqlTypes.NpgsqlDbType.Text, producto.Descripcion);
                cmd.Parameters.AddWithValue("@param_tipo", NpgsqlTypes.NpgsqlDbType.Integer, producto.Tipo);
                cmd.Parameters.AddWithValue("@param_id_empresa", NpgsqlTypes.NpgsqlDbType.Integer, idEmpresa);

                cmd.ExecuteScalar();
            }
            this.conn.CloseAsync();
            return true;
        }

        public Boolean actualizarStockProveedor(Productos datosDeStock) 
        {

            

            if (Convert.ToInt32(datosDeStock.idempresa) == 2)
            {
                this.conectarMySQL.Open();
                MySqlCommand com = new MySqlCommand("sp_actualizarStockBazar", conectarMySQL);
                com.CommandType = System.Data.CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@idi", datosDeStock.idproducto);
                com.Parameters.AddWithValue("@cantidad", datosDeStock.cantSolicit);

                com.ExecuteNonQuery();
                this.conectarMySQL.Close();
            }
            else 
            {
                if (Convert.ToInt32(datosDeStock.idempresa) == 3)
                {
                    this.conectarMySQL.Open();
                    MySqlCommand com = new MySqlCommand("sp_actualizarStockLibreria", conectarMySQL);
                    com.CommandType = System.Data.CommandType.StoredProcedure;

                    com.Parameters.AddWithValue("@idi", datosDeStock.idproducto);
                    com.Parameters.AddWithValue("@cantidad", datosDeStock.cantSolicit);

                    com.ExecuteNonQuery();
                    this.conectarMySQL.Close();
                }
                else if (Convert.ToInt32(datosDeStock.idempresa) == 4)
                {
                    this.conectarMySQL.Open();
                    MySqlCommand com = new MySqlCommand("sp_actualizarStockComputadora", conectarMySQL);
                    com.CommandType = System.Data.CommandType.StoredProcedure;

                    com.Parameters.AddWithValue("@idi", datosDeStock.idproducto);
                    com.Parameters.AddWithValue("@cantidad", datosDeStock.cantSolicit);

                    com.ExecuteNonQuery();
                    this.conectarMySQL.Close();
                }
            }

            
            return true;
        }
    }
}