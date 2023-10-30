using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    internal class Color
    {
        public long IdColor { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public bool BajaLogica { get; set; } = false;
    }
}
