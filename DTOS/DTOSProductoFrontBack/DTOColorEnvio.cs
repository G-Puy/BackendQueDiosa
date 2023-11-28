using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.DTOSProductoFrontBack
{
    public class DTOColorEnvio
    {
        public long Id { get; set; } = -1;
        public int Cantidad { get; set; } = 0;
        public string NombreColor { get; set; } = "";
    }
}
