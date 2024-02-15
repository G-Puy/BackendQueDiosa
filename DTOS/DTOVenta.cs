using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS
{
    public class DTOVenta
    {
        public long IdVenta { get; set; } = 0;
        public decimal MontoTotal { get; set; } = 0;
        public string NombreComprador { get; set; } = "";
        public string CorreoComprador { get; set; } = "";
        public bool BajaLogica { get; set; } = false;
        public string Direccion { get; set; } = "";
        public string Telefono { get; set; } = "";
        public bool Aprobado { get; set; } = false;
        public string ApellidoComprador { get; set; } = "";
        public bool Envio { get; set; } = false;
        public DateTime Fecha {  get; set; } = DateTime.Now;
        public List<DTOVentaProducto> ProductosVendidos { get; set; } = new List<DTOVentaProducto>();
    }
}
