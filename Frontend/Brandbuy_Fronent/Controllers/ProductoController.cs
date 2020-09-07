using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brandbuy_Fronent.Models;
using Brandbuy_Fronent.Models.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Brandbuy_Fronent.Controllers
{
    public class ProductoController : Controller
    {
        // GET: ProductoController

        public IConfiguration Configuration { get; }

        public ProductoController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TiendaView()
        {
            System.Diagnostics.Debug.WriteLine("repuesta******ssssssssssss*******" + (string)HttpContext.Session.GetString("idC"));
            ProductosBusiness productoBusiness = new ProductosBusiness(this.Configuration);
            List<Productos> productos = new List<Productos>();

            productos = productoBusiness.ListarProductos();

            return View("TiendaView", productos);
           

        }

        public ActionResult DetalleProductoView(string idP, string idE)
        {
            ProductosBusiness productoBusiness = new ProductosBusiness(this.Configuration);
            Productos productos = new Productos();

            productos = productoBusiness.DetalleProductos(idP, idE);

            return View("DetalleProductoView", productos);

        }

        public ActionResult AgregarCarrito(string idP, string idE, string cant)
        {
            ProductosBusiness productoBusiness = new ProductosBusiness(this.Configuration);
            var idC = (string)HttpContext.Session.GetString("idC");
            System.Diagnostics.Debug.WriteLine("repuesta*************" + idP, idE, idC, cant);
            productoBusiness.AgregarCarrito(idP, idE, idC, cant);
            

            return View();
        }

        public ActionResult CarritoView()
        {
            ProductosBusiness productoBusiness = new ProductosBusiness(this.Configuration);
            CarritoProductos productos = new CarritoProductos();

            var idC = (string)HttpContext.Session.GetString("idC");

            productos=productoBusiness.verCarrito(idC);

            return View("CarritoView", productos);
        }

        public void AumentaDisminuyeCarrito(string idP, string idE, string asc)
        {
            ProductosBusiness productoBusiness = new ProductosBusiness(this.Configuration);

            var idC = (string)HttpContext.Session.GetString("idC");

            productoBusiness.AumentaDisminuyeCarrito(idP, idE, idC, asc );
        }

        public ActionResult PagoView(string subtotal)
        {
            ProductosBusiness productoBusiness = new ProductosBusiness(this.Configuration);

            var idC = (string)HttpContext.Session.GetString("idC");

            return View("PagoView", subtotal);
        }

        public ActionResult EliminaCarrito(string idP, string idE)
        {
            ProductosBusiness productoBusiness = new ProductosBusiness(this.Configuration);

            var idC = (string)HttpContext.Session.GetString("idC");

            productoBusiness.EliminaCarrito(idP, idE, idC);

            CarritoProductos productos = new CarritoProductos();

            productos = productoBusiness.verCarrito(idC);

            return View("CarritoView", productos);

        }

        public ActionResult ComprarCarrito()
        {
            ProductosBusiness productoBusiness = new ProductosBusiness(this.Configuration);

            var idC = (string)HttpContext.Session.GetString("idC");

            productoBusiness.ComprarCarrito(idC);

            CarritoProductos productos = new CarritoProductos();

            productos = productoBusiness.verCarrito(idC);

            return View("CarritoView", productos);

        }


        // GET: ProductoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }



        // POST: ProductoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
