﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.DTOSProductoFrontBack
{
    public class DTOOrderDataProducto
    {
        public long Id { get; set; } = 0;
        public long IdColor { get; set; } = 0;
        public long IdTalle { get; set; } = 0;
        public int Cantidad { get; set; } = 0;
    }
}
