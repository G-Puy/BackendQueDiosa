using DTOS;
using IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios
{
    public class RepositorioCategoria : RepositorioBase, IRepositorioCategoria
    {
        public bool Alta(DTOCategoria obj)
        {
            throw new NotImplementedException();
        }

        public bool BajaLogica(DTOCategoria obj)
        {
            throw new NotImplementedException();
        }

        public DTOCategoria BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(DTOCategoria obj)
        {
            throw new NotImplementedException();
        }

        public bool Modificar(DTOCategoria obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DTOCategoria> TraerTodos()
        {
            throw new NotImplementedException();
        }

        public bool VerificarExistenciaCategoria(DTOCategoria DTOCategoria)
        {
            throw new NotImplementedException();
        }
    }
}
