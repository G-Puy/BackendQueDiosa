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
        public string Descripcion { get; set; } = "";
        public bool Entregado { get; set; } = false;
        public DTOVenta Venta { get; set; } = new DTOVenta();
    }
}
