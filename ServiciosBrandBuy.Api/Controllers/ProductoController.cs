using ServiciosBrandBuy.Api.Models;
using System;
using System.Web.Http;

namespace ServiciosBrandBuy.Api.Controllers
{
    public class ProductoController : ApiController
    {
        [HttpPost]
        public Mensaje Post(DatosEmpresa json)
        {
            Data claseData = new Data();

            Boolean validezConexion = claseData.validarConexionConClave(json.NombreEmpresa, json.ClaveGenerada);
            if (!validezConexion)
            {
                Mensaje msj = new Mensaje();
                msj.Respuesta = "Conexión Inválida";
                return msj;
            }
            else 
            {
                claseData.registrarProductos(json);
                Mensaje msj = new Mensaje();
                msj.Respuesta = "Registro de productos exitoso.";
                return msj;
            }
        }
    }
}
