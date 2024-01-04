using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.DTOSProductoFrontBack
{
    public class DTOOrderData
    {
        public DTOOrderDataPersona DTOOrderDataPersona { get; set; } = new DTOOrderDataPersona();
        public List<DTOOrderDataProducto> DTOOrderDataProductos { get; set; } = new List<DTOOrderDataProducto>();
    }
}
