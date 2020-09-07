using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brandbuy_Fronent.Models;
using Brandbuy_Fronent.Models.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Brandbuy_Fronent.Controllers
{
    public class ClienteController : Controller
    {
        // GET: ClienteController
        public IConfiguration Configuration { get; }

        public ClienteController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: ClienteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        public IActionResult IniciaSesionView()
        {
           

            return View();
        }

        public IActionResult RegistrarView()
        {
            return View();
        }


        public IActionResult RegistrarCliente(string NombreC, string Correo, string PasswordC)
        {
            ClienteBusiness clienteBusiness = new ClienteBusiness(this.Configuration);
            Cliente c = new Cliente();
            c.NombreC = NombreC;
            c.Correo = Correo;
            c.PasswordC = PasswordC;
            var salida = "";
            salida = clienteBusiness.RegistrarClientes(c);

            if (salida == "0")
            {
                return View("RegistrarView", "El usuario ya existe");
            }
            else
            {
                return View("IniciaSesionView");
            }
        }

        public IActionResult IniciarSesion(string NombreC, string PasswordC)
        {
            

            ClienteBusiness clienteBusiness = new ClienteBusiness(this.Configuration);
            Cliente c = new Cliente();
            c.NombreC = NombreC;
            c.PasswordC = PasswordC;

            var salida = "";

            salida = clienteBusiness.IniciaSesionCliente(c);

            if (salida == "0")

            {
                return View("IniciaSesionView", "El usuario no está registrado");
            }
            else
            {
                HttpContext.Session.SetString("idC", salida);
                
                return RedirectToAction("TiendaView", "Producto");
            }
            

        }


        // POST: ClienteController/Create
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

        // GET: ClienteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClienteController/Edit/5
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

        // GET: ClienteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClienteController/Delete/5
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
