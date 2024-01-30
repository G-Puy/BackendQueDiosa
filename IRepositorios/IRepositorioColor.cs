using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepositorios
{
    public interface IRepositorioColor : IRepositorioT<DTOColor>
    {
        DTOColor BuscarPorNombre(DTOColor dtoColor);

        public bool EnUso(DTOColor dtoColor);

        bool NombreOcupado(DTOColor dto);
    }
}
