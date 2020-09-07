using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brandbuy_Fronent.Models.Data
{
    public class ProductosData
    {
        public IConfiguration Configuration { get; }

        public ProductosData(IConfiguration configuration)
        {
            Configuration = configuration;
        }

       
        public List<Productos> ListarProductos() //modificar
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
                            idCont=idCont+1;
                        }

                    }
                }
                connection.Close();
            }

            return productos;
        }

        public Productos DetalleProductos(string idP, string idE) //modificar
        {
            Productos productos = new Productos();

            NpgsqlConnection conexion = new NpgsqlConnection();

            string cadenaDeConexion = Configuration["ConnectionStrings:DefaultConnection"];
            
            using (var connection = new NpgsqlConnection(cadenaDeConexion))
            {
                connection.Open();

                string sql = $"Select * from detalleProducto('{idP}','{idE}')";

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
                            temp.cantstock = Convert.ToInt32(dataReader["cantidadProductosT"].ToString());
                            temp.total = (int) temp.precio * (int)temp.cantstock;
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

                string sql = $"select from realizarCompra('{idC}')";

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

    }
}