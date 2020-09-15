using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiciosBrandBuy.Api.Models
{
    public class DatosEmpresa
    {
        public string NombreEmpresa { get; set; }
        public string ClaveGenerada { get; set; }
        public string Correo { get; set; }
        public List<Producto> Productos { get; set; }
    }
}