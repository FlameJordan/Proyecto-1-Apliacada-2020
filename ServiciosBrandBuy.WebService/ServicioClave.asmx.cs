using Servicios.WebService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;

namespace ServiciosBrandBuy.WebService
{
    /// <summary>
    /// Summary description for ServicioClave
    /// </summary>
    [WebService(Namespace = "http://stevendlavc-001-site1.atempurl.com/", Name = "ServicioClave")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    //[ScriptService]
    public class ServicioClave : System.Web.Services.WebService
    {

        [WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Clave> GenerarClave()
        {
            List<Clave> claves = new List<Clave>();
            const string src = "abcdefghijklmnopqrstuvwxyz0123456789";
            int length = 16;
            var sb = new StringBuilder();
            Random RNG = new Random();
            for (var i = 0; i < length; i++)
            {
                var c = src[RNG.Next(0, src.Length)];
                sb.Append(c);
            }
            Clave clave = new Clave();
            clave.claveGenerada = Seguridad.Encriptar(sb.ToString());
            claves.Add(clave);
            
            return claves;
        }
    }
}
