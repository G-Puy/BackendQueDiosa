using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS
{
    public class DTOVentaProducto
    {
        public long IdVenta { get; set; } = 0;
        public long IdProducto { get; set; } = 0;
        public long IdColor { get; set; } = 0;
        public long IdTalle { get; set; } = 0;
        public int Cantidad { get; set; } = 0;
        public decimal Precio { get; set; } = 0;
    }
}
