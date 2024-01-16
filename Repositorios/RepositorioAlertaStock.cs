using DTOS;
using IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios
{
    public class RepositorioAlertaStock : RepositorioBase, IRepositorioAlertaStock
    {
        public bool Alta(DTOAlertaStock obj)
        {
            throw new NotImplementedException();
        }

        public bool BajaLogica(DTOAlertaStock obj)
        {
            throw new NotImplementedException();
        }

        public DTOAlertaStock BuscarPorId(DTOAlertaStock obj)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(DTOAlertaStock obj)
        {
            throw new NotImplementedException();
        }

        public bool Modificar(DTOAlertaStock obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DTOAlertaStock> TraerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
