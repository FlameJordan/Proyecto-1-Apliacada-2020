using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Servicios.WebService
{
    public class Clave
    {
        public Clave()
        {
            claveGenerada = "";
        }

        public Clave(string claveGenerada)
        {
            this.claveGenerada = claveGenerada;
        }

        public String claveGenerada { set; get; }

    }
}