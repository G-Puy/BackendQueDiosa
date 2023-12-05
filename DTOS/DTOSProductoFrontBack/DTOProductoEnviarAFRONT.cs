using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.DTOSProductoFrontBack
{
    public  class DTOProductoEnviarAFRONT
    {
        public long Id { get; set; } = -1;
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public double PrecioActual { get; set; } = -1;
        public double PrecioAnterior { get; set; } = -1;
        public long IdTipoProducto { get; set; } = -1;
        public bool VisibleEnWeb { get; set; } = true;
        public bool Nuevo { get; set; } = false;
        public bool BajaLogica { get; set; } = false;
        public string GuiaTalles { get; set; } = "";
        public DTOStockEnvio Stock { get; set; } = new DTOStockEnvio();
        public List<DTOImagen> Imagenes { get; set; } = new List<DTOImagen>();
        public string TipoTalle { get; set; } = "";
    }
}
