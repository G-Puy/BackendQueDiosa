using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.DTOSProductoFrontBack
{
    public class DTOTalleEnvio
    {
        public long Id { get; set; } = -1;
        public long IdProducto { get; set; } = -1;
        public long Cantidad { get; set; } = 0;
        public List<DTOColorEnvio> Colores { get; set; } = new List<DTOColorEnvio>();
        public string NombreTalle { get; set; } = "";
    }
}
