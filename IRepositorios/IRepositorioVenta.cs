using DTOS;
using DTOS.DTOSProductoFrontBack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepositorios
{
    public interface IRepositorioVenta : IRepositorioT<DTOVenta>
    {
        public bool Confirmar(long idVenta);

        public bool Cancelar(long idVenta);

        public List<DTODetallePedido> TraerDetallePedido(long idVenta);
    }
}
