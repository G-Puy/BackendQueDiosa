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
    }
}
