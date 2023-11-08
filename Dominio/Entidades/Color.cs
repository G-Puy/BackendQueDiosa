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
        public long Id { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public bool BajaLogica { get; set; } = false;

        public void cargarDeDTO(DTOColor dtoCol)
        {
            this.Id = dtoCol.Id;
            this.Nombre = dtoCol.Nombre;
            this.BajaLogica = dtoCol.BajaLogica;
        }
        public DTOColor darDto()
        {
            DTOColor dtoRetorno = new DTOColor();
            dtoRetorno.Id = this.Id;
            dtoRetorno.Nombre = this.Nombre;
            dtoRetorno.BajaLogica = this.BajaLogica;
            return dtoRetorno;

        }
    }
}
