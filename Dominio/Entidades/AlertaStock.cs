using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    internal class AlertaStock
    {
        public long IdAlertaStock { get; set; } = 0;
        public long IdStock { get; set; } = 0;
        public string Descripcion { get; set; } = "";
        public bool Leida { get; set; } = false;
    }
}
