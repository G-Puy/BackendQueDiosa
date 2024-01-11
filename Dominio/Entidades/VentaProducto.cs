using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class VentaProducto
    {
        public long IdVenta {  get; set; } = 0;
        public long IdProducto { get; set; } = 0;
        public long IdColor { get; set; } = 0;
        public long IdTalle { get; set; } = 0;
        public int Cantidad { get; set; } = 0;
        public decimal Precio { get; set; } = 0;


        public void cargarDeDTO(DTOVentaProducto dto)
        {
            this.IdVenta = dto.IdVenta;
            this.IdProducto = dto.IdProducto;
            this.IdColor = dto.IdColor;
            this.IdTalle = dto.IdTalle;
            this.Cantidad = dto.Cantidad;
            this.Precio = dto.Precio;
        }

        public DTOVentaProducto darDto()
        {
            DTOVentaProducto dto = new DTOVentaProducto();
            dto.IdVenta = this.IdVenta;
            dto.IdProducto  = this.IdProducto ;
            dto.IdColor  = this.IdColor ;
            dto.IdTalle  = this.IdTalle ;
            dto.Cantidad  = this.Cantidad ;
            dto.Precio  = this.Precio;
            return dto;
        }
    }
}
