﻿using DTOS;
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
        public decimal MontoTotal { get; set; } = 0;
        public bool Envio { get; set; } = false;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public bool Realizado { get; set; } = false;

        public DTOAlertaPedido darDto()
        {
            DTOAlertaPedido dto = new DTOAlertaPedido();
            dto.Id = Id;
            dto.IdVenta = IdVenta;
            dto.MontoTotal = MontoTotal;
            dto.Envio = Envio;
            dto.Nombre = Nombre;
            dto.Apellido = Apellido;
            dto.Telefono = Telefono;
            dto.Realizado = Realizado;
            return dto;
        }

        public void cargarDeDto(DTOAlertaPedido dto)
        {
            Id = dto.Id;
            IdVenta = dto.IdVenta;
            MontoTotal = dto.MontoTotal;
            Envio = dto.Envio;
            Nombre = dto.Nombre;
            Apellido = dto.Apellido;
            Telefono = dto.Telefono;
            Direccion = dto.Direccion;
            Realizado = dto.Realizado;
        }
    }
}
