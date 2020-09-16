using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TiendaLibreria.Data;
using TiendaLibreria.Models;
using System.Net.Http.Formatting;

namespace TiendaLibreria.Controllers
{
    public class HomeController : Controller
    {
        private string nombre = "Libreria";
        private string correo = "rocilin411@gmail.com";
        private String url = "https://serviciosbrandbuyapi20200914152105.azurewebsites.net/";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Key()
        {
            ViewBag.mensaje = "";
            return View();
        }

        [HttpPost]
        public JsonResult obtenerKey()
        {
            //consultar api-invocar servicio rest
            HttpClient clienteHttp = new HttpClient();
            clienteHttp.BaseAddress = new Uri(this.url);

            var request = clienteHttp.GetAsync("api/Clave?nombreEmpresa=" + this.nombre + "&correo=" + this.correo).Result;

            if (request.IsSuccessStatusCode)//consulta si la peticion es 200 es decir que estuvo bien
            {
                var resultString = request.Content.ReadAsStringAsync().Result;
                var lista = JsonConvert.DeserializeObject<List<KeyRequest>>(resultString);
                String claveDesencriptada = DesEncriptar(lista[0].claveGeneradaField);
                return Json(claveDesencriptada);
            }
            else
            {
                return Json("Ocurrio un error");
            }


        }

        [HttpPost]
        public ActionResult enviarDatos(String Key) //recibimos la imagen como parametro en el metodo
        {
            String claveEncritpda = Encriptar(Key);
            DataProducto dataProducto = new DataProducto();
            List<Producto> productos = dataProducto.obtenerTodos();
            DatosEmpresa datosEmpresa = new DatosEmpresa();
            datosEmpresa.ClaveGenerada = claveEncritpda;
            datosEmpresa.NombreEmpresa = this.nombre;

            datosEmpresa.Productos = productos;

            //enviar al api
            var datosenvio = JsonConvert.SerializeObject(datosEmpresa);
            //HttpContent contenido = new StringContent(datosenvio, Encoding.UTF8, "application/json"); ;

            HttpClient clienteHttp = new HttpClient();
            clienteHttp.BaseAddress = new Uri(this.url);

            var request = clienteHttp.PostAsync("api/producto", datosEmpresa, new JsonMediaTypeFormatter()).Result;

            if (request.IsSuccessStatusCode)//consulta si la peticion es 200 es decir que estuvo bien
            {
                var resultString = request.Content.ReadAsStringAsync().Result;
                //
                var respuesta = JsonConvert.DeserializeObject<Mensaje>(resultString);

                ViewBag.mensaje = respuesta.Respuesta;
                return View("Key");

            }
            else
            {
                //Solicitud no se pudo realizar
                ViewBag.mensaje = "La solicitud no se pudo realizar";
                return View("Key");
            }
        }

        /// Encripta una cadena
        public static string Encriptar(string clave)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(clave);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
        public static string DesEncriptar(string clave)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(clave);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }
    }
}