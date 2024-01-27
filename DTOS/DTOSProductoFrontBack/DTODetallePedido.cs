using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.DTOSProductoFrontBack
{
    public class DTODetallePedido
    {
        public int Cantidad { get; set; } = 0;
        public long IdProducto { get; set; } = 0;
        public string NombreProducto { get; set; } = string.Empty;
        public string NombreTalle { get; set; } = string.Empty;
        public string NombreColor { get; set; } = string.Empty;
    }
}
