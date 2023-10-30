using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    internal class Stock
    {
        public long IdStock { get; set; } = 0;
        public long IdProducto { get; set; } = 0;
        public long IdColor { get; set; } = 0;
        public long IdTalle { get; set; } = 0;
    }
}
