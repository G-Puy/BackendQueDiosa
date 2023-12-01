using DTOS.DTOSProductoFrontBack;
using Microsoft.AspNetCore.Http;

namespace DTOS
{
    public class DTOProducto
    {
        public long Id { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public double PrecioActual { get; set; } = 0;
        public double PrecioAnterior { get; set; } = 0;
        public long IdTipoProducto { get; set; } = 0;
        public bool VisibleEnWeb { get; set; } = false;
        public string GuiaTalles { get; set; } = "";
        public bool Nuevo { get; set; } = false;
        public bool BajaLogica { get; set; } = false;
        public List<DTOStock> Stocks { get; set; } = new List<DTOStock>();
        public List<DTOImagen> Imagenes { get; set; } = new List<DTOImagen>();
    }
}
