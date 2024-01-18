using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class AlertaPedido
    {
        public long Id { get; set; } = 0;
        public long IdVenta { get; set; } = 0;
        public string Descripcion { get; set; } = "";
        public bool Entregado { get; set; } = false;

        public DTOAlertaPedido darDto()
        {
            DTOAlertaPedido dto = new DTOAlertaPedido();
            dto.Id = Id;
            dto.Descripcion = Descripcion;
            dto.Entregado = Entregado;
            return dto;
        }

        public void cargarDeDto(DTOAlertaPedido dto)
        {
            Id = dto.Id;
            IdVenta = dto.Venta.IdVenta;
            Descripcion = dto.Descripcion;
            Entregado = dto.Entregado;
        }
    }
}
