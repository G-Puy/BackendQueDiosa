using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    internal class Venta
    {
        public long IdVenta { get; set; } = 0;
        public double MontoTotal { get; set; } = 0;
        public string NombreComprador { get; set; } = "";
        public string CorreoComprador { get; set; } = "";
        public List<Producto> ProductosVendidos { get; set; } = new List<Producto>();
    }
}
