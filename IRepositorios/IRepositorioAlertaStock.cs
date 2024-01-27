using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepositorios
{
    public interface IRepositorioAlertaStock : IRepositorioT<DTOAlertaStock>
    {
        public bool Leer(long id);
        public int Contar(long id);
    }
}
