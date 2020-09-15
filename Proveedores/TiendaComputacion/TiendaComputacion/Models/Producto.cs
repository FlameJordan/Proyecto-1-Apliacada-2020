using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiendaComputacion.Models
{
    public class Producto
    {
        private int id;
        private String nombre;
        private int cantidadStock;
        private String imagen;
        private int precio;
        private string descripcion;
        private int tipo;

        public Producto()
        {
            this.id = 0;
            this.nombre = "";
            this.cantidadStock = 0;
            this.imagen = "";
            this.precio = 0;
            this.descripcion = "";
            this.tipo = 0;
        }

        public Producto(int id, string nombre, int cantidadStock, string imagen, int precio, string descripcion, int tipo)
        {
            this.id = id;
            this.nombre = nombre;
            this.cantidadStock = cantidadStock;
            this.imagen = imagen;
            this.precio = precio;
            this.descripcion = descripcion;
            this.tipo = tipo;
        }

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public int CantidadStock { get => cantidadStock; set => cantidadStock = value; }
        public string Imagen { get => imagen; set => imagen = value; }
        public int Precio { get => precio; set => precio = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public int Tipo { get => tipo; set => tipo = value; }

    }
}