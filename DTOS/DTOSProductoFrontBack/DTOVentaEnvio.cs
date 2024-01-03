using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.DTOSProductoFrontBack
{
    public class DTOVentaEnvio
    {
        public long Id { get; set; } = -1;
        public int Cantidad { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; } = -1;
    }
}
