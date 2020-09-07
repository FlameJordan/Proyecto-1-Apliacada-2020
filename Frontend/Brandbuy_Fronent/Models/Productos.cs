using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brandbuy_Fronent.Models
{
    public class Productos
    {
        public string idproducto { get; set; }
        public string idempresa { get; set; }
        public string nombre { get; set; }
        public int precio { get; set; }
        public string descripcion { get; set; }
        public string tipo { get; set; }
        public string imagen { get; set; }
        public int cantstock { get; set; }
        public string estado { get; set; }
        public string idCont { get; set; }
        public float total { get; set; }
        
    }
}
