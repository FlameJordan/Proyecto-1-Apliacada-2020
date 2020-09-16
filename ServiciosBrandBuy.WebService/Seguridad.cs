using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiciosBrandBuy.WebService
{
    public static class Seguridad
    {
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