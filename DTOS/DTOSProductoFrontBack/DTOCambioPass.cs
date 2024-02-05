using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.DTOSProductoFrontBack
{
    public class DTOCambioPass
    {
        public long Id { get; set; } = 0;
        public string NombreDeUsuario { get; set; } = "";
        public string Contrasenia { get; set; } = "";
        public string ContraseniaNueva { get; set; } = "";
    }
}
