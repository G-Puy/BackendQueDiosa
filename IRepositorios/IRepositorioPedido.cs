using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepositorios
{
    public interface IRepositorioPedido: IRepositorioT<DTOAlertaPedido>
    {
        public bool Entregado(long id);
    }
}
