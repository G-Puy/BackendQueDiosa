using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.DTOSProductoFrontBack
{
    public class DTOStockEnvio
    {
        public long Id { get; set; } = -1;
        public long IdProducto { get; set; } = -1;
        public List<DTOTalleEnvio> Talles { get; set; } = new List<DTOTalleEnvio>();
    }
}
