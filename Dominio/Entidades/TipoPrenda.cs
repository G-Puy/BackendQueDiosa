using DTOS;

namespace Dominio.Entidades
{
    public class TipoPrenda
    {
        public long IdTipoPrenda { get; set; } = 0;
        public string NombreTipoPrenda { get; set; } = "";
        public bool BajaLogica { get; set; } = false;

        public void cargarDeDTO(DTOTipoPrenda dtoCat)
        {
            this.IdTipoPrenda = dtoCat.IdTipoPrenda;
            this.NombreTipoPrenda = dtoCat.NombreTipoPrenda;
            this.BajaLogica = dtoCat.BajaLogica;
        }
        public DTOTipoPrenda darDto()
        {
            DTOTipoPrenda dtoRetorno = new DTOTipoPrenda();
            dtoRetorno.IdTipoPrenda = this.IdTipoPrenda;
            dtoRetorno.NombreTipoPrenda = this.NombreTipoPrenda;
            dtoRetorno.BajaLogica= this.BajaLogica;
            return dtoRetorno;

        }

    }
}