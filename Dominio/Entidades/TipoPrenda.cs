using DTOS;

namespace Dominio.Entidades
{
    public class TipoPrenda
    {
        public long Id { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public bool BajaLogica { get; set; } = false;

        public void cargarDeDTO(DTOTipoPrenda dtoCat)
        {
            this.Id = dtoCat.Id;
            this.Nombre = dtoCat.Nombre;
            this.BajaLogica = dtoCat.BajaLogica;
        }
        public DTOTipoPrenda darDto()
        {
            DTOTipoPrenda dtoRetorno = new DTOTipoPrenda();
            dtoRetorno.Id = this.Id;
            dtoRetorno.Nombre = this.Nombre;
            dtoRetorno.BajaLogica= this.BajaLogica;
            return dtoRetorno;

        }

    }
}