using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    internal class AlertaPedido
    {
        public long IdAlertaPedido { get; set; } = 0;
        public long IdVenta { get; set; } = 0;
        public string Descripcion { get; set; } = "";
        public bool Entregado { get; set; } = false;
    }
}
