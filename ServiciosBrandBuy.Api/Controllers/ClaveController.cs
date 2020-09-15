using ServiciosBrandBuy.Api.GeneradorDeClaves;
using ServiciosBrandBuy.Api.Models;
using System;
using System.Web.Http;

namespace ServiciosBrandBuy.Api.Controllers
{
    public class ClaveController : ApiController
    {
        [HttpGet]
        public object Get(String nombreEmpresa, String correo)
        {
            Data claseData = new Data();
            var existente = claseData.buscarClaveExistente(nombreEmpresa, correo);

            if (existente[0].claveGeneradaField == "No Encontrada")
            {
                ServicioClaveSoapClient cliente = new ServicioClaveSoapClient();

                var clave = cliente.GenerarClave();

                if (claseData.validezClave(clave[0].claveGenerada))
                {
                    claseData.registrarClave(clave[0].claveGenerada, nombreEmpresa, correo);

                    return clave;
                }
                else 
                {
                    Boolean validez = false;
                    while (!validez) 
                    {
                        clave = cliente.GenerarClave();
                        if (claseData.validezClave(clave[0].claveGenerada))
                        {
                            validez = true;
                        }
                    }
                    claseData.registrarClave(clave[0].claveGenerada, nombreEmpresa, correo);

                    return clave;
                }

            }
            else 
            {
                return existente;
            }
        }


    }
}
