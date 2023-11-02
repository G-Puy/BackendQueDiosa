using DTOS;
using IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios
{
    public class RepositorioLogin : RepositorioBase, IRepositorioLogin
    {
        public bool Alta(DTOUsuario obj)
        {
            throw new NotImplementedException();
        }

        public bool BajaLogica(DTOUsuario obj)
        {
            throw new NotImplementedException();
        }

        public DTOUsuario BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(DTOUsuario obj)
        {
            throw new NotImplementedException();
        }

        public bool Login(DTOUsuario dtoUsuario)
        {
            throw new NotImplementedException();
        }

        public bool Modificar(DTOUsuario obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DTOUsuario> TraerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
