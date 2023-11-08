using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Talle
    {
        public long Id { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public bool BajaLogica { get; set; } = false;

        public void cargarDeDTO(DTOTalle dtoTal)
        {
            this.Id = dtoTal.Id;
            this.Nombre = dtoTal.Nombre;
            this.BajaLogica = dtoTal.BajaLogica;
        }
        public DTOTalle darDto()
        {
            DTOTalle dtoRetorno = new DTOTalle();
            dtoRetorno.Id = this.Id;
            dtoRetorno.Nombre = this.Nombre;
            dtoRetorno.BajaLogica = this.BajaLogica;
            return dtoRetorno;

        }
    }
}
