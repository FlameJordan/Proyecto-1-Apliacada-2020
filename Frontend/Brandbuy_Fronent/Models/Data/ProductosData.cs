using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Brandbuy_Fronent.Models.Data
{
    public class ProductosData
    {
        public IConfiguration Configuration { get; }

        private string url { get; set; }
        private WebRequest web { get; set; }
        private HttpWebResponse resp { get; set; }
        private HttpWebRequest req { get; set; }

        public ProductosData(IConfiguration configuration)
        {
            Configuration = configuration;
            url = "http://localhost:55124/api/Producto";
        }

        public void conexionApi(string parametros, string metodo)
        {
            string dir = String.Format(url + parametros);
            req = (HttpWebRequest)WebRequest.Create(dir);
            req.ContentType = "application/json";
            req.Method = metodo;
        }


        public List<Productos> ListarProductos()
        {
            List<Productos> productos = new List<Productos>();
            NpgsqlConnection conexion = new NpgsqlConnection();
            var idCont = 1;
            string cadenaDeConexion = Configuration["ConnectionStrings:DefaultConnection"];

            using (var connection = new NpgsqlConnection(cadenaDeConexion))
            {
                connection.Open();

                string sql = $"Select * from obtenerProductosAprobados ()";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            Productos temp = new Productos();
                            System.Diagnostics.Debug.WriteLine("repuesta*************" + dataReader["idproductoT"].ToString());
                            temp.idproducto = dataReader["idproductoT"].ToString();
                            temp.idempresa = dataReader["idempresaT"].ToString();
                            temp.nombre = dataReader["nombreT"].ToString();
                            temp.precio = Convert.ToInt32(dataReader["precioT"].ToString());
                            temp.imagen = "\\img\\" + dataReader["imagenT"].ToString();
                            temp.idCont = "idCont"+idCont;

                            temp.total = 0;
                            System.Diagnostics.Debug.WriteLine("repuesta****VEEEEEEEEEEEEEEEEEEEEER*********" + temp.idCont);

                            productos.Add(temp);
                            idCont = idCont+1;
                        }

                    }
                }
                connection.Close();
            }

            return productos;
        }

        public List<Categoria> ListarCategorias()
        {
            List<Categoria> categoria = new List<Categoria>();
            NpgsqlConnection conexion = new NpgsqlConnection();

            string cadenaDeConexion = Configuration["ConnectionStrings:DefaultConnection"];

            using (var connection = new NpgsqlConnection(cadenaDeConexion))
            {
                connection.Open();

                string sql = $"Select * from obtenerCategorias()";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            Categoria temp = new Categoria();
                            temp.idproducto = dataReader["idproductoT"].ToString();
                            temp.NombreT = dataReader["nombreTipoT"].ToString();

                            categoria.Add(temp);

                        }

                    }
                }
                connection.Close();
            }

            return categoria;
        }
        public Productos DetalleProductos(string idP, string idE, string idC) //modificar
        {
            Productos productos = new Productos();

            NpgsqlConnection conexion = new NpgsqlConnection();

            string cadenaDeConexion = Configuration["ConnectionStrings:DefaultConnection"];
            
            using (var connection = new NpgsqlConnection(cadenaDeConexion))
            {
                connection.Open();

                string sql = $"Select * from detalleProducto('{idP}','{idE}','{idC}')";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            System.Diagnostics.Debug.WriteLine("repuesta*************" + dataReader["idproductoT"].ToString());
                            productos.idproducto = dataReader["idproductoT"].ToString();
                            productos.idempresa = dataReader["idempresaT"].ToString();
                            productos.nombre = dataReader["nombreT"].ToString();
                            productos.precio = Convert.ToInt32(dataReader["precioT"].ToString());
                            productos.descripcion = dataReader["descripcionT"].ToString();
                            productos.tipo = dataReader["tipoT"].ToString();
                            productos.imagen = "\\img\\"+dataReader["imagenT"].ToString();
                            productos.cantstock = Convert.ToInt32(dataReader["cantstockT"].ToString());
                            productos.estado = dataReader["estadoT"].ToString();
                        }

                    }
                }
                connection.Close();
            }

            return productos;
        }

        public void AgregarCarrito(string idP, string idE, string idC, string cant) //modificar
        {
           

            NpgsqlConnection conexion = new NpgsqlConnection();

            string cadenaDeConexion = Configuration["ConnectionStrings:DefaultConnection"];
           
            using (var connection = new NpgsqlConnection(cadenaDeConexion))
            {
                connection.Open();

                string sql = $"Select * from AgregarCarrito('{idP}','{idE}','{idC}','{cant}')";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            System.Diagnostics.Debug.WriteLine("repuesta*************" + dataReader["idproductoT"].ToString());
                        }

                    }
                }
                connection.Close();
            }

        
        }

        public CarritoProductos verCarrito(string idC) //modificar
        {

            CarritoProductos cp= new CarritoProductos() ;
            List<Productos> productos = new List<Productos>();
            NpgsqlConnection conexion = new NpgsqlConnection();
            var montoTotal = 0.0;
            var idCont = 1;
            string cadenaDeConexion = Configuration["ConnectionStrings:DefaultConnection"];

            using (var connection = new NpgsqlConnection(cadenaDeConexion))
            {
                connection.Open();

                string sql = $"Select * from obtenerProductosCarritos('{idC}')";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            Productos temp = new Productos();
                            System.Diagnostics.Debug.WriteLine("repuesta*************" + dataReader["idproductoT"].ToString());
                            temp.idproducto = dataReader["idproductoT"].ToString();
                            temp.idempresa = dataReader["idempresaT"].ToString();
                            temp.nombre = dataReader["nombreT"].ToString();
                            temp.precio = Convert.ToInt32(dataReader["precioT"].ToString());
                            temp.imagen = "\\img\\" + dataReader["imagenT"].ToString();
                            System.Diagnostics.Debug.WriteLine("repuesta*************" + temp.imagen);
                            temp.idCont = "idCont" + idCont;
                           
                            //temp.descripcion = dataReader["descripcionT"].ToString();
                            temp.cantSolicit = Convert.ToInt32(dataReader["cantidadProductosT"].ToString());
                            temp.cantstock = Convert.ToInt32(dataReader["cantidadstockT"].ToString());
                            temp.total = (int) temp.precio * (int)temp.cantSolicit;
                            //temp.estado = dataReader["estadoT"].ToString();
                            montoTotal += temp.total;

                            productos.Add(temp);
                            idCont++;
                        }

                    }
                }
                connection.Close();
            }
            cp.productos = productos;
            cp.totalTodosProductos = (float)montoTotal;
            return cp;
        }

        public void AumentaDisminuyeCarrito(string idP, string idE, string idC, string asc)
        {

            NpgsqlConnection conexion = new NpgsqlConnection();

            string cadenaDeConexion = Configuration["ConnectionStrings:DefaultConnection"];

            using (var connection = new NpgsqlConnection(cadenaDeConexion))
            {
                connection.Open();

                string sql = $"select from cantDelCarrito('{idP}','{idE}','{idC}','{asc}')";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            System.Diagnostics.Debug.WriteLine("repuesta*************");
                           
                        }

                    }
                }
                connection.Close();
            }
        }

        public void EliminaCarrito(string idP, string idE, string idC)
        {
            NpgsqlConnection conexion = new NpgsqlConnection();
            System.Diagnostics.Debug.WriteLine("repuesta*****"+idP+ "******" + idE + "**"+idC);
            string cadenaDeConexion = Configuration["ConnectionStrings:DefaultConnection"];

            using (var connection = new NpgsqlConnection(cadenaDeConexion))
            {
                connection.Open();

                string sql = $"select from eliminarDelCarrito('{idP}','{idE}','{idC}')";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            System.Diagnostics.Debug.WriteLine("repuesta*************");

                        }

                    }
                }
                connection.Close();
            }
        }

        public void ComprarCarrito(string idC)
        {
            NpgsqlConnection conexion = new NpgsqlConnection();

            string cadenaDeConexion = Configuration["ConnectionStrings:DefaultConnection"];

            using (var connection = new NpgsqlConnection(cadenaDeConexion))
            {
                connection.Open();

                string sql = $"select * from realizarCompra('{idC}')";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            Productos temp = new Productos();
                            temp.idproducto = dataReader["idproductoT"].ToString();
                            temp.idempresa = dataReader["idempresaT"].ToString();
                            temp.cantSolicit = Convert.ToInt32(dataReader["cantT"].ToString());

                            if(temp.idempresa != "4") {
                                string responseBody = "";
                                conexionApi("/RebajaStock", "POST");
                                string json = JsonSerializer.Serialize(temp);
                                System.Diagnostics.Debug.WriteLine("json*************" + json);
                                StreamWriter stream = new StreamWriter(req.GetRequestStream());

                                stream.Write(json);
                                stream.Flush();
                                stream.Close();

                                try
                                {
                                    using (WebResponse response = req.GetResponse())
                                    {
                                        using (Stream strReader = response.GetResponseStream())
                                        {
                                            if (strReader == null) System.Diagnostics.Debug.WriteLine("NULL"); ;
                                            using (StreamReader objReader = new StreamReader(strReader))
                                            {
                                                responseBody = objReader.ReadToEnd();
                                                // Do something with responseBody
                                                System.Diagnostics.Debug.WriteLine("repuesta*************" + responseBody);
                                            }
                                        }
                                    }
                                }
                                catch (WebException ex)
                                {
                                    System.Diagnostics.Debug.WriteLine("**********error*************");
                                }
                            }

                        }

                    }
                }
                connection.Close();
            }
           
        }

        public List<Productos> ListarProductosBuscados(string idT, string idC)
        {
            List<Productos> productos = new List<Productos>();
            NpgsqlConnection conexion = new NpgsqlConnection();
            var idCont = 1;
            string cadenaDeConexion = Configuration["ConnectionStrings:DefaultConnection"];

            using (var connection = new NpgsqlConnection(cadenaDeConexion))
            {
                connection.Open();

                string sql = $"Select * from obtenerProductosPorTipo('{idT}')";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            Productos temp = new Productos();
                            System.Diagnostics.Debug.WriteLine("repuesta*************" + dataReader["idproductoT"].ToString());
                            temp.idproducto = dataReader["idproductoT"].ToString();
                            temp.idempresa = dataReader["idempresaT"].ToString();
                            temp.nombre = dataReader["nombreT"].ToString();
                            temp.precio = Convert.ToInt32(dataReader["precioT"].ToString());
                            temp.imagen = "\\img\\" + dataReader["imagenT"].ToString();
                            temp.idCont = "idCont" + idCont;

                            temp.total = 0;
                
                            productos.Add(temp);
                            idCont = idCont + 1;
                        }

                    }
                }
                connection.Close();
            }

            return productos;
        }

        public List<Productos> ListarProductosSugeridos(string idC)
        {
            List<Productos> productos = new List<Productos>();
            NpgsqlConnection conexion = new NpgsqlConnection();
            var idCont = 1;
            string cadenaDeConexion = Configuration["ConnectionStrings:DefaultConnection"];

            using (var connection = new NpgsqlConnection(cadenaDeConexion))
            {
                connection.Open();

                string sql = $"Select * from obtenerProductosSugeridos('{idC}')";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            Productos temp = new Productos();
                            System.Diagnostics.Debug.WriteLine("repuesta*************" + dataReader["idproductoT"].ToString());
                            temp.idproducto = dataReader["idproductoT"].ToString();
                            temp.idempresa = dataReader["idempresaT"].ToString();
                            temp.nombre = dataReader["nombreT"].ToString();
                            temp.precio = Convert.ToInt32(dataReader["precioT"].ToString());
                            temp.imagen = "\\img\\" + dataReader["imagenT"].ToString();
                            temp.idCont = "idContaS" + idCont;

                            temp.total = 0;

                            productos.Add(temp);
                            idCont = idCont + 1;
                        }

                    }
                }
                connection.Close();
            }

            return productos;
        }


    }
}