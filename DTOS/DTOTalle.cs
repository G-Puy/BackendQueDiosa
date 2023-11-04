using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS
{
    public class DTOTalle
    {
        public long IdTalle { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public bool BajaLogica { get; set; } = false;
    }
}
