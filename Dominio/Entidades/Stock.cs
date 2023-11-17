using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Stock
    {
        public long Id { get; set; } = 0;
        public long IdProducto { get; set; } = 0;
        public long IdColor { get; set; } = 0;
        public long IdTalle { get; set; } = 0;

        public void cargarDeDTO(DTOStock dtoStock)
        {
            this.Id = dtoStock.Id;
            this.IdProducto = dtoStock.IdProducto;
            this.IdColor = dtoStock.IdColor;
            this.IdTalle = dtoStock.IdTalle;
        }

        public DTOStock darDto()
        {
            DTOStock dtoStock = new DTOStock();
            dtoStock.Id = this.Id;
            dtoStock.IdProducto = this.IdProducto;
            dtoStock.IdColor = this.IdColor;
            dtoStock.IdTalle = this.IdTalle;
            return dtoStock;
        }
    }
}
