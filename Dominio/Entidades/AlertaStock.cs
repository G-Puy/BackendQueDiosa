using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class AlertaStock
    {
        public long Id { get; set; } = 0;
        public long IdStock { get; set; } = 0;
        public string Descripcion { get; set; } = "";
        public bool Leida { get; set; } = false;

        public DTOAlertaStock darDto()
        {
            DTOAlertaStock alerta = new DTOAlertaStock();
            alerta.Id = Id;
            alerta.IdStock = IdStock;
            alerta.Descripcion = Descripcion;
            alerta.Leida = Leida;
            return alerta;
        }

        public void cargarDeDto(DTOAlertaStock alerta)
        {
            Id = alerta.Id;
            IdStock = alerta.IdStock;
            Descripcion = alerta.Descripcion;
            Leida = alerta.Leida;
        }
    }
}
