using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    internal class Usuario
    {
        public long IdUsuario { get; set; } = 0;
        public string NombreDeUsuario { get; set; } = "";
        public string Contraseña { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public bool BajaLogica { get; set; } = false;
        public int TipoUsuario { get; set; } = 0;

        public void cargarDeDTO(DTOUsuario dtoUsuario)
        {
            this.IdUsuario = dtoUsuario.IdUsuario;
            this.NombreDeUsuario = dtoUsuario.NombreDeUsuario;
            this.Contraseña = dtoUsuario.Contraseña;
            this.Nombre = dtoUsuario.Nombre;
            this.Apellido = dtoUsuario.Apellido;
            this.Telefono = dtoUsuario.Telefono;
            this.Correo = dtoUsuario.Correo;
            this.BajaLogica = dtoUsuario.BajaLogica;
            this.TipoUsuario = dtoUsuario.TipoUsuario;

        }
        public DTOUsuario darDto()
        {
            DTOUsuario dto = new DTOUsuario();

            dto.IdUsuario = this.IdUsuario;
            dto.NombreDeUsuario = this.NombreDeUsuario;
            dto.Contraseña = this.Contraseña;
            dto.Nombre = this.Nombre;
            dto.Apellido = this.Apellido;
            dto.Telefono = this.Telefono;
            dto.Correo = this.Correo;
            dto.BajaLogica = this.BajaLogica;
            dto.TipoUsuario = this.TipoUsuario;

            return dto;
        }

    }


}
