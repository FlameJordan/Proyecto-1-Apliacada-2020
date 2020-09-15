using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaComputacion.Data;
using TiendaComputacion.Models;
using PagedList;

namespace TiendaComputacion.Controllers
{
    public class ProductosController : Controller
    {
        string conexionStr = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        MySqlConnection conexion = new MySqlConnection();

        public ActionResult RegistrarProducto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarProducto(HttpPostedFileBase imagen, String nombre, int cantidadStock, int precio, String descripcion, String tipo)
        {
            String imagenNombre = Path.GetFileName(imagen.FileName);
            try
            {
                string path = Server.MapPath("~/img/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                imagen.SaveAs(path + Path.GetFileName(imagen.FileName));
            }
            catch (Exception e) { }

            Producto producto = new Producto();
            producto.Nombre = nombre;
            producto.CantidadStock = cantidadStock;
            producto.Precio = precio;
            producto.Descripcion = descripcion;
            producto.Tipo = int.Parse(tipo);
            producto.Imagen = imagenNombre;

            DataProducto dataProducto = new DataProducto();
            dataProducto.insertarProducto(producto);
            return View();
        }

        public ActionResult EliminarProducto(int id, int? page)
        {
            DataProducto dataProducto = new DataProducto();
            dataProducto.eliminarProducto(id);
            var lista = dataProducto.obtenerTodos();

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View("VerProductos", lista.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ActualizarProducto(int id)
        {
            DataProducto dataProducto = new DataProducto();
            Producto producto = dataProducto.obtenerProducto(id);
            ViewBag.nombre = producto.Nombre;
            ViewBag.cantidad = producto.CantidadStock;
            ViewBag.precio = producto.Precio;
            ViewBag.descripcion = producto.Descripcion;
            ViewBag.tipo = producto.Tipo;
            ViewBag.imagen = producto.Imagen;
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public ActionResult ActualizarProducto(HttpPostedFileBase imagen, String nombre, int cantidadStock, int precio, string descripcion, string tipo, string id, string imagenVieja)
        {
            String imagenNombre = "";
            if (imagen == null)
            {
                imagenNombre = imagenVieja;
            }
            else
            {
                imagenNombre = Path.GetFileName(imagen.FileName);
                try
                {
                    string path = Server.MapPath("~/img/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    imagen.SaveAs(path + Path.GetFileName(imagen.FileName));
                }
                catch (Exception e) { }
            }


            Producto producto = new Producto();
            producto.Id = int.Parse(id);
            producto.Nombre = nombre;
            producto.CantidadStock = cantidadStock;
            producto.Precio = precio;
            producto.Descripcion = descripcion;
            producto.Tipo = int.Parse(tipo);
            producto.Imagen = imagenNombre;

            try
            {
                DataProducto dataProducto = new DataProducto();
                dataProducto.actualizarProducto(producto);
                var lista = dataProducto.obtenerTodos();

                return RedirectToAction("VerProductos", lista);
            }
            catch
            {
                DataProducto dataProducto = new DataProducto();
                producto = dataProducto.obtenerProducto(int.Parse(id));
                ViewBag.nombre = producto.Nombre;
                ViewBag.cantidad = producto.CantidadStock;
                ViewBag.precio = producto.Precio;
                ViewBag.descripcion = producto.Descripcion;
                ViewBag.tipo = producto.Tipo;
                ViewBag.imagen = producto.Imagen;
                ViewBag.id = id;
                return View();
            }
        }

        public ActionResult VerProductos(string sortOrder, string currentFilter, string searchString, int? page)
        {
            DataProducto dataProducto = new DataProducto();
            var lista = dataProducto.obtenerTodos();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Cant" ? "cant_desc" : "Cant";
            ViewBag.PrecioSortParm = sortOrder == "CantP" ? "cantp_desc" : "CantP";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var producto = from s in lista
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                producto = producto.Where(s => s.Nombre.Contains(searchString)
                                       || s.CantidadStock.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    producto = producto.OrderByDescending(s => s.Id);
                    break;
                case "Cant":
                    producto = producto.OrderBy(s => s.Nombre);
                    break;
                case "cant_desc":
                    producto = producto.OrderByDescending(s => s.CantidadStock);
                    break;
                case "CantP":
                    producto = producto.OrderBy(s => s.Nombre);
                    break;
                case "cantp_desc":
                    producto = producto.OrderByDescending(s => s.Precio);
                    break;
                default:
                    producto = producto.OrderBy(s => s.Nombre);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(producto.ToPagedList(pageNumber, pageSize));
        }
    }
}