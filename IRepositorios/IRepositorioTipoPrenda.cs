using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepositorios
{
    public interface IRepositorioTipoPrenda : IRepositorioT<DTOTipoPrenda>
    {
        public bool VerificarExistenciaCategoria(DTOTipoPrenda DTOCategoria);

        DTOTipoPrenda BuscarPorNombreDePrenda(DTOTipoPrenda dtoTipoPrenda);

    }
}
