using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS
{
    public class DTOAlertaStock
    {
        public long Id { get; set; } = 0;
        public bool Leida { get; set; } = false;
        public string NombreProducto { get; set; } = "";
        public string NombreTalle { get; set; } = "";
        public string NombreColor { get; set; } = "";
        public int Cantidad { get; set; } = 0;
        public long IdProducto { get; set; } = 0;
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
