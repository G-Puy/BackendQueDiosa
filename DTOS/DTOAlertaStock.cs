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
        public string Descripcion { get; set; } = "";
        public bool Leida { get; set; } = false;
        public DTOStock stock { get; set; } = new DTOStock();
    }
}
