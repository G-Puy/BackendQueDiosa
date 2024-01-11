using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Venta
    {
        public long IdVenta { get; set; } = 0;
        public decimal MontoTotal { get; set; } = 0;
        public string NombreComprador { get; set; } = "";
        public string CorreoComprador { get; set; } = "";
        public bool BajaLogica { get; set; } = false;
        public string Direccion { get; set; } = "";
        public string Telefono { get; set; } = "";
        public bool Aprobado { get; set; } = false;
        public List<VentaProducto> ProductosVendidos { get; set; } = new List<VentaProducto>();

        public void cargarDeDTO(DTOVenta dto)
        {
            this.IdVenta = dto.IdVenta;
            this.MontoTotal = dto.MontoTotal;
            this.NombreComprador = dto.NombreComprador;
            this.CorreoComprador = dto.CorreoComprador;
            this.BajaLogica = dto.BajaLogica;
            this.Direccion = dto.Direccion;
            this.Telefono = dto.Telefono;
            this.Aprobado = dto.Aprobado;

            foreach (DTOVentaProducto dTOVentaProducto in dto.ProductosVendidos)
            {
                VentaProducto ventaProducto = new VentaProducto();
                ventaProducto.cargarDeDTO(dTOVentaProducto);
                this.ProductosVendidos.Add(ventaProducto);
            }
        }

        public DTOVenta darDto()
        {
            DTOVenta dto = new DTOVenta();
            dto.IdVenta = this.IdVenta;
            dto.MontoTotal = this.MontoTotal;
            dto.NombreComprador = this.NombreComprador;
            dto.CorreoComprador = this.CorreoComprador;
            dto.BajaLogica = this.BajaLogica;
            dto.Direccion = this.Direccion;
            dto.Telefono = this.Telefono;
            dto.Aprobado = this.Aprobado;

            foreach (VentaProducto ventaProducto in this.ProductosVendidos)
            {
                dto.ProductosVendidos.Add(ventaProducto.darDto());
            }

            return dto;
        }
    }
}