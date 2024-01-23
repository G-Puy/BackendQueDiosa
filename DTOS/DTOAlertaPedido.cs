using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS
{
    public class DTOAlertaPedido
    {
        public long Id { get; set; } = 0;
        public long IdVenta { get; set; } = 0;
        public decimal MontoTotal { get; set; } = 0;
        public bool Envio { get; set; } = false;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public bool Realizado { get; set; } = false;
    }
}
