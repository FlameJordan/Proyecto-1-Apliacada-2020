using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brandbuy_Fronent.Models
{
    public class Productos_Categorias
    {
        public List<Productos> productos { get; set; }
        public List<Categoria> categorias { get; set; }
        public List<Productos> productosSug { get; set; }
    }
}
