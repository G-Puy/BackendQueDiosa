using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    internal class Producto
    {
        public long IdProducto { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public double PrecioActual { get; set; } = 0;
        public double PrecioAnterior { get; set; } = 0;
        public long IdTipoProducto { get; set; } = 0;
        public bool VisibleEnWeb { get; set; } = false;
        public bool Nuevo { get; set; } = false;
        public bool BajaLogica { get; set; } = false;
    }
}
