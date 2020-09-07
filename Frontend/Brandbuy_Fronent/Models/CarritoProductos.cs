using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brandbuy_Fronent.Models
{
    public class CarritoProductos
    {
        public List<Productos> productos { get; set; }
        public float totalTodosProductos { get; set; }
    }
}
