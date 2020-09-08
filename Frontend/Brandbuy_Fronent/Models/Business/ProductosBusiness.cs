using Brandbuy_Fronent.Models.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brandbuy_Fronent.Models.Business
{
    public class ProductosBusiness
    {
        public IConfiguration Configuration { get; }

        public ProductosBusiness(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<Productos> ListarProductos()
        {

            ProductosData productoData = new ProductosData(this.Configuration);
            List<Productos> productos = new List<Productos>();
            productos = productoData.ListarProductos();
            return productos;
        }

        public List<Categoria> ListarCategorias()
        {
            ProductosData productoData = new ProductosData(this.Configuration);
            List<Categoria> categoria = new List<Categoria>();
            categoria = productoData.ListarCategorias();
            return categoria;
        }

        public Productos DetalleProductos(string idP, string idE, string idC)
        {

            ProductosData productoData = new ProductosData(this.Configuration);
            Productos productos = new Productos();
            productos = productoData.DetalleProductos(idP, idE, idC);
            return productos;
        }

        public void AgregarCarrito(string idP, string idE, string idC, string cant)
        {
            ProductosData productoData = new ProductosData(this.Configuration);
            productoData.AgregarCarrito(idP, idE, idC, cant);
        }

        public CarritoProductos verCarrito(string idC)
        {
            ProductosData productoData = new ProductosData(this.Configuration);
            CarritoProductos productos = new CarritoProductos();
            productos = productoData.verCarrito(idC);

            return productos;
           

        }

        public void AumentaDisminuyeCarrito(string idP, string idE, string idC, string asc)
        {
            ProductosData productoData = new ProductosData(this.Configuration);

            productoData.AumentaDisminuyeCarrito(idP, idE, idC, asc);
        }

        public void EliminaCarrito(string idP, string idE, string idC)
        {
            ProductosData productoData = new ProductosData(this.Configuration);

            productoData.EliminaCarrito(idP, idE, idC);
        }


        public void ComprarCarrito(string idC)
        {
            ProductosData productoData = new ProductosData(this.Configuration);

            productoData.ComprarCarrito(idC);
        }

        public List<Productos> buscarProducto(string idT, string idC)
        {
            ProductosData productoData = new ProductosData(this.Configuration);
            List<Productos> productos = new List<Productos>();
            productos=productoData.ListarProductosBuscados(idT, idC);

            return productos;
        }

        public List<Productos> ListarProductosSugeridos(string idC)
        {
            ProductosData productoData = new ProductosData(this.Configuration);
            List<Productos> productosS = new List<Productos>();
            productosS = productoData.ListarProductosSugeridos(idC);

            return productosS;
        }

    }
}
