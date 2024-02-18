﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.DTOSProductoFrontBack
{
    public class DTOOrderDataPersona
    {
        public string nombre { get; set; } = "";
        public string apellido { get; set; } = "";
        public string departamento { get; set; } = "";
        public string ciudad { get; set; } = "";
        public string barrio { get; set; } = "";
        public string direccion { get; set; } = "";
        public string mail { get; set; } = "";
        public string telefono { get; set; } = "";
        public bool enviar { get; set; } = false;
        public string notas { get; set; } = "";

    }
}
