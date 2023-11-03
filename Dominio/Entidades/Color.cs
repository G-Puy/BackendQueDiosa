using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Color
    {
        public long IdColor { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public bool BajaLogica { get; set; } = false;

        public void cargarDeDTO(DTOColor dtoCol)
        {
            this.IdColor = dtoCol.IdColor;
            this.Nombre = dtoCol.Nombre;
            this.BajaLogica = dtoCol.BajaLogica;
        }
        public DTOColor darDto()
        {
            DTOColor dtoRetorno = new DTOColor();
            dtoRetorno.IdColor = this.IdColor;
            dtoRetorno.Nombre = this.Nombre;
            dtoRetorno.BajaLogica = this.BajaLogica;
            return dtoRetorno;

        }
    }
}
