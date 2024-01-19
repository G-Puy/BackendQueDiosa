using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.DTOSProductoFrontBack
{
    public class DTOFiltroAlertasPedidos
    {
        public long IdVenta { get; set; } = 0;
        public string realizado { get; set; } = string.Empty; //"realizado" "noRealizado"
        public string envioRetiro { get; set; } = string.Empty; //"retiro" "envio"
        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
    }
}
