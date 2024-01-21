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
        public bool Leida { get; set; } = false;
        public string NombreProducto { get; set; } = "";
        public string NombreTalle { get; set; } = "";
        public string NombreColor { get; set; } = "";
        public int Cantidad { get; set; } = 0;

        public DTOAlertaStock darDto()
        {
            DTOAlertaStock alerta = new DTOAlertaStock();
            alerta.Id = Id;
            alerta.Leida = Leida;
            alerta.NombreColor = NombreColor;
            alerta.NombreProducto = NombreProducto;
            alerta.NombreTalle = NombreTalle;
            alerta.Cantidad = Cantidad;
            return alerta;
        }

        public void cargarDeDto(DTOAlertaStock alerta)
        {
            Id = alerta.Id;
            Leida = alerta.Leida;
            NombreTalle = alerta.NombreTalle;
            NombreColor = alerta.NombreColor;
            NombreProducto = alerta.NombreProducto;
            Cantidad = alerta.Cantidad;
        }
    }
}
